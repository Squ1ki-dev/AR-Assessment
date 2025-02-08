using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private GameState _gameState;

        [Inject]
        private void Construct(GameState gameState, PanelManager panelManager)
        {
            _gameState = gameState;
            _panelManager = panelManager;
        }

        private void OnEnable()
        {
            _playBtn.onClick.AddListener(OnPlayButtonPressed);
            _exitBtn.onClick.AddListener(ExitGame);
        }

        private void ExitGame() => Application.Quit();
        private void OnPlayButtonPressed() => _panelManager.OpenPanelByIndex(3);

        private void OnDestroy()
        {
            _playBtn.onClick.RemoveListener(OnPlayButtonPressed);
            _exitBtn.onClick.RemoveListener(ExitGame);
        }
    }
}