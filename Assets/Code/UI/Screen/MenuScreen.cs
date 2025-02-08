using System;
using System.Collections;
using System.Collections.Generic;
using Code.Wave;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using Zenject;

namespace Code.UI.Screens
{
    public class MenuScreen : WindowBase
    {
        [SerializeField] private Button _playBtn, _exitBtn;
        [SerializeField] private PanelManager _panelManager;
        [SerializeField] private ARPlaneManager _planeManager;

        private WaveSpawner _waveSpawner;
        private GameState _gameState;

        [Inject]
        private void Construct(WaveSpawner waveSpawner, GameState gameState, PanelManager panelManager)
        {
            _waveSpawner = waveSpawner;
            _gameState = gameState;
            _panelManager = panelManager;
        }

        private void OnEnable()
        {
            _playBtn.onClick.AddListener(async() => await OnPlayButtonPressed());
            _exitBtn.onClick.AddListener(ExitGame);
        }

        private void ExitGame() => Application.Quit();
        private async UniTask OnPlayButtonPressed()
        {
            _playBtn.enabled = false;
            _planeManager.enabled = true;
            _gameState.ChangeState(GameStates.Game);
            _panelManager.CloseAllPanels();
            await _waveSpawner.StartNextWave();
        }

        private void OnDestroy()
        {
            _playBtn.onClick.RemoveListener(async() => await OnPlayButtonPressed());
            _exitBtn.onClick.RemoveListener(ExitGame);
        }
    }
}