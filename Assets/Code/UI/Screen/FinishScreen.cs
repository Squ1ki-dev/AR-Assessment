using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UnityEngine.SceneManagement;
using Code;
using Code.Wave;
using System.Linq;
using Code.Player;
using Unity.Burst.CompilerServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Code.UI.Screens
{
    public class FinishScreen : WindowBase
    {
        [SerializeField] private Button _nextWaveBtn, _exitBtn;
        [SerializeField] private WaveSetupSO _waveConfig;
        private WaveSpawner _waveSpawner;
        private PanelManager _panelManager;
        private GameState _gameState;

        [Inject]
        private void Construct(GameState gameState, WaveSpawner waveSystem, PanelManager panelManager)
        {
            _gameState = gameState;
            _waveSpawner = waveSystem;
            _panelManager = panelManager;
        }

        private void OnEnable()
        {
            _nextWaveBtn.onClick.AddListener(HandleNextWave);
            _exitBtn.onClick.AddListener(() => ExitToMenu());
        }

        private void ExitToMenu() 
        { 
            _panelManager.OpenPanelByIndex(0);
            _gameState.ChangeState(GameStates.Menu);
        }

        private void HandleNextWave()
        {
            _waveConfig.CurrentWave++;
            PlayerPrefs.SetInt(Constants.Level, _waveConfig.CurrentWave);
            Debug.Log($"Current Wave " + _waveConfig.CurrentWave);
            _waveSpawner.StartNextWave();
        }

        private void OnDestroy()
        {
            _nextWaveBtn.onClick.RemoveListener(HandleNextWave);
            _exitBtn.onClick.RemoveListener(() => ExitToMenu());
        }
    }
}