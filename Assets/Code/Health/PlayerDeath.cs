using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code.Player;
using Code;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    //[SerializeField] private PlayerAttack playerAttack;
    //[SerializeField] private PlayerMove playerMove;
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
        //playerMove.enabled = false;
        //playerAttack.enabled = false;
        _gameState.ChangeState(GameStates.Lose);
        Instantiate(_deathFx, transform.position, Quaternion.identity);
    }
}
