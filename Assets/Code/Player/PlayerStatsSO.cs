using System.Collections.Generic;
using UnityEngine;

namespace Code.Player
{
    [CreateAssetMenu(fileName = "PlayerConfig")]
    public class PlayerStatsSO : ScriptableObject
    {
        public int MaxHP = 100;
        public float Speed;
        public float AttackRange;
        public float AttackSpeed;
        public int Damage;
        public float AttackCooldownDuration;
        public GameObject ProjectilePrefab;
    }
}