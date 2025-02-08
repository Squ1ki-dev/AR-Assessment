using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Enemy
{
    [CreateAssetMenu(fileName = "EnemyConfig")]
    public class EnemySO : ScriptableObject
    {
        public int Health;
        public int XP;
        public float MovementSpeed;
        public float AttackSpeed;
        public int Damage;
        public GameObject PrefabReference;
    }
}