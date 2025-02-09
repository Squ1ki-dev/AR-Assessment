using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code.Player;
using Code;
using Code.UI;

namespace Code.Health
{
    public class PlayerDeath : MonoBehaviour
    {
        [SerializeField] private int _loseScreenIdx;
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private PlayerAttack playerAttack;
        [SerializeField] private PlayerMovement _playerMovement;
        private GameState _gameState;
        private PanelManager _panelManager;
        private bool _isDead;

        public void Construct(GameState gameState, PanelManager panelManager)
        {
            _gameState = gameState;
            _panelManager = panelManager;
        }

        private void OnEnable() => _playerHealth.HealthChanged += HealthChanged;
        private void OnDestroy() => _playerHealth.HealthChanged -= HealthChanged;

        private void HealthChanged()
        {
            if(!_isDead && _playerHealth.Current <= 0)
                Die();
        }

        private void Die()
        {   
            _isDead = true;
            _playerMovement.enabled = false;
            playerAttack.enabled = false;
            _panelManager.OpenPanelByIndex(_loseScreenIdx);
            _gameState.ChangeState(GameStates.Lose);
        }
    }
}
