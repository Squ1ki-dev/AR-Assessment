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
    public class LoseScreen : WindowBase
    {
        [SerializeField] private Button _restartBtn, _exitBtn;
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
            _restartBtn.onClick.AddListener(RestartLevel);
            _exitBtn.onClick.AddListener(() => ExitToMenu());
        }

        private void ExitToMenu() 
        { 
            _panelManager.OpenPanelByIndex(0);
            _gameState.ChangeState(GameStates.Menu);
        }

        private void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnDestroy()
        {
            _restartBtn.onClick.RemoveListener(RestartLevel);
            _exitBtn.onClick.RemoveListener(() => ExitToMenu());
        }
    }
}