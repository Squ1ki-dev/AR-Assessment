using System.Collections;
using UnityEngine;
using TMPro;
using Zenject;
using Code.Enemies;
using Code;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

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

        private int _weakEnemyCount;
        private int _normalEnemyCount;
        private float _waveTime;
        private bool _waveActive;
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

        private void SaveWaveNumber()
        {
            PlayerPrefs.SetInt(Constants.Level, _waveSetup.CurrentWave);
            PlayerPrefs.Save();
        }

        public async UniTask StartNextWave()
        {
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

            Vector3 bossPosition = GetRandomPositionAround(_spawnPoint.position, _waveSetup.MinSpawnRadius, _waveSetup.MaxSpawnRadius);
            GameObject bossEnemy = _container.InstantiatePrefab(_bossEnemySO.EnemyPrefab, bossPosition, Quaternion.identity, _enemyParent);
        }

        private async UniTask SpawnEnemyGroup(EnemySO enemySO, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 position = GetRandomPositionAround(_spawnPoint.position, _waveSetup.MinSpawnRadius, _waveSetup.MaxSpawnRadius);
                GameObject enemy = _container.InstantiatePrefab(enemySO.EnemyPrefab, position, Quaternion.identity, _enemyParent);
                await UniTask.Delay(1000);
            }
        }

        private Vector3 GetRandomPositionAround(Vector3 center, float minRadius, float maxRadius)
        {
            float radius = Random.Range(minRadius, maxRadius);
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

            float xOffset = Mathf.Cos(angle) * radius;
            float zOffset = Mathf.Sin(angle) * radius;

            return new Vector3(center.x + xOffset, center.y, center.z + zOffset);
        }

        private bool IsBossWave(int waveNumber) => waveNumber % _waveSetup.BossWaveInterval == 0;
    }
}
