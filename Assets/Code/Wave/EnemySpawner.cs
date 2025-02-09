using System.Collections.Generic;
using Code.Services;
using Code.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Wave
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private int _winScreenIdx;
        [SerializeField] private Transform _enemyParent;

        private int _activeEnemyCount;
        public int ActiveEnemyCount => _activeEnemyCount;

        [Inject] private DiContainer _container;
        private PanelManager _panelManager;
        private IAssetLoader _assetLoader;
        private PlacementHandler _placementHandler;

        [Inject]
        public void Construct(PanelManager panelManager, IAssetLoader assetLoader, PlacementHandler placementHandler)
        {
            _panelManager = panelManager;
            _assetLoader = assetLoader;
            _placementHandler = placementHandler;
        }

        public async UniTask SpawnWave(int waveNumber)
        {
            Debug.Log($"Spawning wave {waveNumber}");
            int weakEnemies = GetWeakEnemyCount(waveNumber);
            int normalEnemies = GetNormalEnemyCount(waveNumber);

            if (IsBossWave(waveNumber))
                await SpawnBossWave(waveNumber);

            await SpawnEnemyGroup(Constants.WeakEnemy, weakEnemies);
            await SpawnEnemyGroup(Constants.NormalEnemy, normalEnemies);
        }

        private async UniTask SpawnBossWave(int waveNumber)
        {
            int bossWaveWeak = GetBossWeakEnemyCount(waveNumber);
            int bossWaveNormal = GetBossNormalEnemyCount(waveNumber);

            await SpawnEnemyGroup(Constants.WeakEnemy, bossWaveWeak);
            await SpawnEnemyGroup(Constants.NormalEnemy, bossWaveNormal);
            await SpawnSingleEnemy(Constants.BossAddress);
        }

        private async UniTask SpawnEnemyGroup(string address, int count)
        {
            for (int i = 0; i < count; i++)
            {
                await UniTask.Delay(1000);
                await SpawnSingleEnemy(address);
            }
        }

        private async UniTask SpawnSingleEnemy(string address)
        {
            Vector3 position = _placementHandler.GetSpawnPosition();
            if (position == Vector3.zero)
                return;

            GameObject enemy = await LoadAndInstantiateAsync(address, position);
            if (enemy != null)
            {
                _activeEnemyCount++;
                enemy.GetComponent<EnemyDeath>().Happened += HandleEnemyDeath;
            }
        }

        private async UniTask<GameObject> LoadAndInstantiateAsync(string address, Vector3 position)
        {
            GameObject prefab = await _assetLoader.LoadAssetAsync(address);
            
            return _container.InstantiatePrefab(prefab, position, Quaternion.identity, _enemyParent);
        }

        private void HandleEnemyDeath()
        {
            _activeEnemyCount--;
            if(_activeEnemyCount == 0)
                _panelManager.OpenPanelByIndex(_winScreenIdx);
        }

        private bool IsBossWave(int waveNumber) => waveNumber % 5 == 0;

        private int GetWeakEnemyCount(int waveNumber) => 5 + (waveNumber - 1) * 2;
        private int GetNormalEnemyCount(int waveNumber) => waveNumber >= 5 ? 3 + (waveNumber - 1) : 0;
        private int GetBossWeakEnemyCount(int waveNumber) => 10 + (waveNumber - 1) * 3;
        private int GetBossNormalEnemyCount(int waveNumber) => 5 + (waveNumber - 1) * 2;
    }
}