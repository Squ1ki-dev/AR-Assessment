using Code.Player;
using UnityEngine;
using Zenject;

namespace Code.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private EnemySO _enemyConfig;
        [SerializeField] private float _minDist;
        private PlayerMovement _player;

        private void Start() => _player = FindObjectOfType<PlayerMovement>();

        private void FixedUpdate()
        {
            if (_player == null)
                return;

            Vector3 direction = (_player.transform.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, _player.transform.position);

            if (distance <= _minDist)
                return;

            transform.Translate(direction * _enemyConfig.MovementSpeed * Time.fixedDeltaTime);
            Debug.Log($"[EnemyMovement] Moving towards {_player.name} at speed {_enemyConfig.MovementSpeed}");
        }
    }
}
