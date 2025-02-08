using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code.Player;
using Code;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    //[SerializeField] private PlayerAttack playerAttack;
    //[SerializeField] private PlayerMove playerMove;
    [SerializeField] private GameObject deathFx;
    private GameState _gameState;
    private bool _isDead;

    private void Start()
    {
        _gameState = FindObjectOfType<GameState>();
        if (_gameState == null)
            Debug.LogError("GameState not found in the scene!");

        playerHealth.HealthChanged += HealthChanged;
    }
    
    private void OnDestroy()
    {
        playerHealth.HealthChanged -= HealthChanged;
    }

    private void HealthChanged()
    {
        if(!_isDead && playerHealth.Current <= 0)
            Die();
    }

    private void Die()
    {   
        _isDead = true;
        //playerMove.enabled = false;
        //playerAttack.enabled = false;
        _gameState.ChangeState(GameStates.Lose);
        Instantiate(deathFx, transform.position, Quaternion.identity);
    }
}
