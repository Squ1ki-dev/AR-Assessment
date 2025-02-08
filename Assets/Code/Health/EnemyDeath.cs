using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private EnemyHealth _health;
    [SerializeField] private GameObject _deathFx;
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

        StartCoroutine(DeathFX(_deathFx));
        ObjectPool.ReturnToPool(gameObject);
        Happened?.Invoke();
    }

    private IEnumerator DeathFX(GameObject deathVFX)
    {
        ObjectPool.SpawnObject(deathVFX, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        ObjectPool.ReturnToPool(deathVFX);
    }
}
