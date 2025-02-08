using System;
using System.Collections;
using System.Collections.Generic;
using Code.Enemy;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    [SerializeField] private EnemySO _enemySO;
    [SerializeField] private TMP_Text _damageText;
    private int _current;
    public event Action HealthChanged;

    public int Current
    {
        get => _current;
        set
        {
            _current = Mathf.Clamp(value, 0, Max);
            HealthChanged?.Invoke();
        }
    }

    public int Max
    {
        get => _enemySO.Health;
        set => _enemySO.Health = value; 
    }

    private void Awake() => _current = _enemySO.Health;

    public void TakeDamage(int damage)
    {
        if(Current <= 0)
            return;
        
        Current -= damage;

        _damageText.text = $"-{(int)damage}";
        RepresentDamage();

        HealthChanged?.Invoke();
    }

    private void RepresentDamage()
    {
        _damageText.color = Color.white;
        _damageText.transform.localScale = Vector3.one;

        Vector3 targetPosition = _damageText.transform.position + new Vector3(0, 1, 0);
        _damageText.transform.DOMove(targetPosition, 1f).SetEase(Ease.OutQuad);
        _damageText.DOFade(0, 1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => _damageText.text = string.Empty);
    }
}
