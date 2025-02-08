using System.Collections;
using UnityEngine;
using TMPro;
using Zenject;
using Code.Enemies;
using Code;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

namespace Code.Wave
{
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _enemyParent;
        [SerializeField] private EnemySO _weakEnemySO;
        [SerializeField] private EnemySO _normalEnemySO;
        [SerializeField] private EnemySO _bossEnemySO;
        [SerializeField] private WaveSetupSO _waveSetup;

        [Inject] private DiContainer _container;

        private Pose _placementPose;
        [SerializeField] private ARRaycastManager aRRaycastManager;
        [SerializeField] private ARPlaneManager _arPlaneManager;
        private List<ARRaycastHit> hits = new List<ARRaycastHit>();

        private int _weakEnemyCount;
        private int _normalEnemyCount;
        private float _waveTime;
        private bool _waveActive;
        private bool placementPoseIsValid;

        public int CurrentWave => _waveSetup.CurrentWave;

        private void Start()
        {
            _waveSetup.CurrentWave = PlayerPrefs.GetInt(Constants.Level, 1);
            Debug.Log($"Wave {_waveSetup.CurrentWave}");
        }

        private void Update()
        {
            if (!_waveActive) return;

            if (_waveTime <= 0)
                EndWave();
        }

        private void UpdatePlacementPose()
        {
            if (_arPlaneManager.trackables.count == 0)
            {
                placementPoseIsValid = false;
                Debug.LogWarning("No AR planes detected.");
                return;
            }

            var screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            aRRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneEstimated);

            placementPoseIsValid = hits.Count > 0;
            if (placementPoseIsValid)
            {
                _placementPose = hits[0].pose;
                Debug.Log($"Valid placement found at {_placementPose.position}");
            }
            else
                Debug.LogWarning("No valid AR plane detected.");
        }

        private void SaveWaveNumber()
        {
            PlayerPrefs.SetInt(Constants.Level, _waveSetup.CurrentWave);
            PlayerPrefs.Save();
        }

        public async UniTask StartNextWave()
        {
            await UniTask.Delay(5000);
            
            if (_waveActive) return;

            _waveSetup.CurrentWave = PlayerPrefs.GetInt(Constants.Level, 1);
            _waveActive = true;
            await SpawnEnemies(_waveSetup.CurrentWave);

            _waveSetup.CurrentWave++;
        }

        private void EndWave()
        {
            _waveActive = false;
            SaveWaveNumber();
        }

        private async UniTask SpawnEnemies(int waveNumber)
        {
            _weakEnemyCount = _waveSetup.BaseWeakEnemyCount + (waveNumber - 1) * _waveSetup.EnemyIncrementPerWave;
            _normalEnemyCount = waveNumber >= 5
                ? _waveSetup.BaseNormalEnemyCount + (waveNumber - 1) * _waveSetup.EnemyIncrementPerWave
                : 0;

            if (IsBossWave(waveNumber))
                await SpawnBossEnemy(waveNumber);

            await SpawnEnemyGroup(_weakEnemySO, _weakEnemyCount);
            await SpawnEnemyGroup(_normalEnemySO, _normalEnemyCount);
        }

        private async UniTask SpawnBossEnemy(int waveNumber)
        {
            int bossWaveWeakEnemies = _waveSetup.BossWaveWeakEnemies + (waveNumber - 1) * _waveSetup.EnemyIncrementPerWave;
            int bossWaveNormalEnemies = _waveSetup.BossWaveNormalEnemies + (waveNumber - 1) * _waveSetup.EnemyIncrementPerWave;

            await SpawnEnemyGroup(_weakEnemySO, bossWaveWeakEnemies);
            await SpawnEnemyGroup(_normalEnemySO, bossWaveNormalEnemies);

            Vector3 bossPosition = GetValidSpawnPosition();
            GameObject bossEnemy = _container.InstantiatePrefab(_bossEnemySO.EnemyPrefab, bossPosition, Quaternion.identity, _enemyParent);
        }

        private async UniTask SpawnEnemyGroup(EnemySO enemySO, int count)
        {
            for (int i = 0; i < count; i++)
            {
                UpdatePlacementPose();

                if (!placementPoseIsValid) 
                {
                    Debug.LogWarning("No valid AR plane found, skipping enemy spawn.");
                    continue;
                }

                Vector3 position = GetValidSpawnPosition();
                if (position == Vector3.zero) 
                {
                    Debug.LogWarning("Failed to find a valid plane position.");
                    continue;
                }

                GameObject enemy = _container.InstantiatePrefab(enemySO.EnemyPrefab, position, Quaternion.identity, _enemyParent);
                await UniTask.Delay(1000);
            }
        }

        private Vector3 GetValidSpawnPosition()
        {
            if (!placementPoseIsValid || _arPlaneManager.trackables.count == 0) 
            {
                Debug.LogWarning("No valid spawn position found due to missing AR planes.");
                return Vector3.zero;
            }

            return _placementPose.position;
        }

        private bool IsBossWave(int waveNumber) => waveNumber % _waveSetup.BossWaveInterval == 0;
    }
}
