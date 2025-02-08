using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Zenject;
using Cysharp.Threading.Tasks;
using Code.Enemy;
using Code.Services;

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

        private IAssetLoader _assetLoader;
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

        [Inject]
        public void Construct(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

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
            if (!_waveActive) return;

            _weakEnemyCount = _waveSetup.BaseWeakEnemyCount + (waveNumber - 1) * _waveSetup.EnemyIncrementPerWave;
            _normalEnemyCount = waveNumber >= 5
                ? _waveSetup.BaseNormalEnemyCount + (waveNumber - 1) * _waveSetup.EnemyIncrementPerWave
                : 0;

            if (IsBossWave(waveNumber))
                await SpawnBossEnemy(waveNumber);

            await SpawnEnemyGroup(Constants.WeakEnemy, _weakEnemyCount);
            await SpawnEnemyGroup(Constants.NormalEnemy, _normalEnemyCount);
        }

        private async UniTask SpawnBossEnemy(int waveNumber)
        {
            int bossWaveWeakEnemies = _waveSetup.BossWaveWeakEnemies + (waveNumber - 1) * _waveSetup.EnemyIncrementPerWave;
            int bossWaveNormalEnemies = _waveSetup.BossWaveNormalEnemies + (waveNumber - 1) * _waveSetup.EnemyIncrementPerWave;

            await SpawnEnemyGroup(Constants.WeakEnemy, bossWaveWeakEnemies);
            await SpawnEnemyGroup(Constants.NormalEnemy, bossWaveNormalEnemies);

            Vector3 bossPosition = GetValidSpawnPosition();
            await LoadAndInstantiateAsync(Constants.BossAddress, bossPosition);
        }

        private async UniTask SpawnEnemyGroup(string address, int count)
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

                await LoadAndInstantiateAsync(address, position);
                await UniTask.Delay(1000);
            }
        }

        private async UniTask LoadAndInstantiateAsync(string address, Vector3 position)
        {
            GameObject enemyPrefab = await _assetLoader.LoadAssetAsync(address);
            if (enemyPrefab == null)
            {
                Debug.LogError($"Failed to load Addressable asset at {address}");
                return;
            }
            _container.InstantiatePrefab(enemyPrefab, position, Quaternion.identity, _enemyParent);
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
