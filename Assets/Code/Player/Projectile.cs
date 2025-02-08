using UnityEngine;

namespace Code.Player
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifeTime;
        [SerializeField] private PlayerStatsSO _playerConfig;

        private void Start() => Destroy(gameObject, lifeTime);

        private void Update() => transform.Translate(Vector3.forward * _playerConfig.AttackSpeed * Time.deltaTime);

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Triggered by: " + other.gameObject.name);
            if(other.gameObject.GetComponent<EnemyHealth>())
            {
                Debug.Log($"Enemy has this comp");
                if (other.gameObject.TryGetComponent<IHealth>(out IHealth health))
                {
                    health.TakeDamage(_playerConfig.Damage);
                    ObjectPool.ReturnToPool(gameObject);
                }
            }
        }
    }
}