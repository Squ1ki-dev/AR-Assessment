using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Code.Player;
using Code.Wave;

namespace Code.UI.Screens
{
    public class ChoiceScreen : WindowBase
    {
        [SerializeField] private int menuScreenIdx;
        [SerializeField] private Button _redOrbBtn, _blueOrbBtn, _backBtn;
        [SerializeField] private PlayerStatsSO _playerConfig;
        [SerializeField] private GameObject _redOrb, _blueOrb;

        private PanelManager _panelManager;
        private GameState _gameState;

        [Inject]
        private void Construct(GameState gameState, PanelManager panelManager)
        {
            _gameState = gameState;
            _panelManager = panelManager;
        }

        private void OnEnable()
        {
            _redOrbBtn.onClick.AddListener(ChosenRed);
            _blueOrbBtn.onClick.AddListener(ChosenBlue);
            _backBtn.onClick.AddListener(BackToMenu);
        }

        private void ChosenRed() => SetPlayerConfig(_redOrb, 10, 1);
        private void ChosenBlue() => SetPlayerConfig(_blueOrb, 20, 2);

        private void SetPlayerConfig(GameObject projectile, int damage, float attackSpeed)
        {
            _playerConfig.ProjectilePrefab = projectile;
            _playerConfig.Damage = damage;
            _playerConfig.AttackSpeed = attackSpeed;
            OnPlayButtonPressed();
        }

        private void OnPlayButtonPressed()
        {
            _panelManager.CloseAllPanels();
            _gameState.ChangeState(GameStates.PlayerPlacementState);
        }

        private void BackToMenu() => _panelManager.OpenPanelByIndex(menuScreenIdx);

        private void OnDisable()
        {
            _redOrbBtn.onClick.RemoveListener(ChosenRed);
            _blueOrbBtn.onClick.RemoveListener(ChosenBlue);
            _backBtn.onClick.RemoveListener(BackToMenu);
        }
    }
}
