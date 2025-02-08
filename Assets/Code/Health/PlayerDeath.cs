using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code.Player;
using Code;
using CodeBase.Player;

namespace Code.Health
{
    public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    //[SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private GameObject _deathFx;
    private GameState _gameState;
    private bool _isDead;

    public void Construct(GameState gameState)
    {
        _gameState = gameState;
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
        //playerAttack.enabled = false;
        _gameState.ChangeState(GameStates.Lose);
    }
}

}
