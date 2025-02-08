using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;
using Zenject;
using Cysharp.Threading.Tasks;

namespace Code.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private PlayerStatsSO _playerConfig;

        private float shootTimer = 0f;

        private void Update()
        {
            shootTimer += Time.deltaTime;

            if (shootTimer >= _playerConfig.AttackCooldownDuration)
            {
                Shoot();
                shootTimer = 0f;
            }
        }

        private void Shoot()
        {
            if (_playerConfig.ProjectilePrefab == null || firePoint == null)
                return;

            GameObject projectile = ObjectPool.SpawnObject(_playerConfig.ProjectilePrefab, firePoint.position, firePoint.rotation);

            ReturnToPoolAfterDelay(projectile, 5000).Forget();
        }

        private async UniTaskVoid ReturnToPoolAfterDelay(GameObject obj, int delayMilliseconds)
        {
            await UniTask.Delay(delayMilliseconds);
            ObjectPool.ReturnToPool(obj);
        }
    }
}