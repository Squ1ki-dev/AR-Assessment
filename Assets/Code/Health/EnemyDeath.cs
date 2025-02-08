using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private EnemyHealth _health;
    public event Action Happened;

    private void Start() => _health.HealthChanged += HealthChanged;
    private void OnDestroy() => _health.HealthChanged -= HealthChanged;

    private void HealthChanged()
    {
        if(_health.Current <= 0)
            Die();
    }

    private void Die()
    {
        _health.HealthChanged -= HealthChanged;

        Destroy(this.gameObject);
        Happened?.Invoke();
    }
}
