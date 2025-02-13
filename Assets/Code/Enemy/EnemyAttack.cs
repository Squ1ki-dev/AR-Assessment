using System.Collections;
using System.Linq;
using UnityEngine;
using CandyCoded.HapticFeedback;
using Code.Enemy;
using Code;
using Zenject;
using Code.Player;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float Cleavage;
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private Transform attackPoint;

    private PlayerMovement _player;

    private const float EffectiveDistance = 0.2f;
    private float _attackCooldown;
    private bool _isAttacking;
    private bool _attackIsActive;
    private int _layerMask;
    private Collider[] _hits = new Collider[1];

    private void Awake() => _layerMask = 1 << LayerMask.NameToLayer("Player");

    private void Start() 
    {
        _player = FindObjectOfType<PlayerMovement>();
        StartCoroutine(AutoAttackRoutine());
    }

    private void UpdateCooldown()
    {
        if (!CooldownIsUp())
            _attackCooldown -= Time.deltaTime;
    }

    private IEnumerator AutoAttackRoutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => _attackIsActive && CanAttack());
            StartAttack();
            yield return new WaitForSeconds(enemySO.AttackSpeed);
        }
    }

    private void StartAttack()
    {
        if (_player.transform.position == null) return;

        transform.LookAt(_player.transform);
        _isAttacking = true;

        Invoke(nameof(OnAttack), 0.2f);
        Invoke(nameof(OnAttackEnded), enemySO.AttackSpeed);
    }

    private void OnAttack()
    {
        if (Hit(out Collider hit))
        {
            Debug.Log($"Hit Player: {hit.name}");
            hit.transform.GetComponent<IHealth>()?.TakeDamage(enemySO.Damage);
            HapticFeedback.LightFeedback();
        }
    }

    private void OnAttackEnded()
    {
        _attackCooldown = enemySO.AttackSpeed;
        _isAttacking = false;
    }

    private bool Hit(out Collider hit)
    {
        int hitsCount = Physics.OverlapSphereNonAlloc(StartPoint, Cleavage, _hits, _layerMask);
        hit = _hits.FirstOrDefault();
        return hitsCount > 0;
    }

    private Vector3 StartPoint => transform.position + transform.forward * EffectiveDistance;

    public void EnableAttack() => _attackIsActive = true;
    public void DisableAttack() => _attackIsActive = false;

    private bool CooldownIsUp() => _attackCooldown <= 0;
    private bool CanAttack() => _attackIsActive && !_isAttacking && CooldownIsUp();

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(StartPoint, Cleavage);
    }
#endif
}
