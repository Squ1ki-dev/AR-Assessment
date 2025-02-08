using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Enemies
{
    [CreateAssetMenu(fileName = "EnemyConfig")]
    public class EnemySO : ScriptableObject
    {
        public int Health;
        public int XP;
        public float MovementSpeed;
        public GameObject PrefabReference;
    }
}