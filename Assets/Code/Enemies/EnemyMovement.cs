using UnityEngine;
using Zenject;

namespace Code.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private EnemySO _enemyConfig;
        private Transform _player;

        [Inject]
        public void Construct(Transform target)
        {
            _player = target;
        }

        private void FixedUpdate()
        {
            if (_player == null)
                return;

            Vector3 direction = (_player.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, _player.position);

            if (distance <= 0.5f)
                return;
            transform.Translate(direction * _enemyConfig.MovementSpeed * Time.fixedDeltaTime);
            Debug.Log($"[EnemyMovement] Moving towards {_player.name} at speed {_enemyConfig.MovementSpeed}");
        }
    }
}
