using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Zenject;
using Cysharp.Threading.Tasks;
using Code.Enemy;
using Code.Services;
using Code.UI;

namespace Code.Wave
{
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private WaveSetupSO _waveSetup;

        private EnemySpawner _enemySpawner;

        [Inject]
        public void Construct(EnemySpawner enemySpawner)
        {
            _enemySpawner = enemySpawner;
        }

        private bool _waveActive;
        public int CurrentWave => _waveSetup.CurrentWave;

        private void Start()
        {
            _waveSetup.CurrentWave = PlayerPrefs.GetInt(Constants.Level, 1);
            Debug.Log($"Starting Wave {_waveSetup.CurrentWave}");
        }

        private void Update()
        {
            if (_waveActive && _enemySpawner.ActiveEnemyCount <= 0)
                EndWave();
        }

        public async UniTask StartNextWave()
        {
            if (_waveActive) return;
            
            await UniTask.Delay(1500);
            _waveSetup.CurrentWave = PlayerPrefs.GetInt(Constants.Level, 1);
            _waveActive = true;

            await _enemySpawner.SpawnWave(_waveSetup.CurrentWave);

            _waveSetup.CurrentWave++;
        }

        private void EndWave()
        {
            _waveActive = false;
            SaveWaveNumber();
            ///_panelManager.OpenPanelByIndex(_loseScreenIdx);
        }

        private void SaveWaveNumber()
        {
            PlayerPrefs.SetInt(Constants.Level, _waveSetup.CurrentWave);
            PlayerPrefs.Save();
        }
    }

}
