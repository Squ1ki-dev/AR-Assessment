using System.Collections.Generic;
using UnityEngine;

namespace Code.Player
{
    [CreateAssetMenu(fileName = "PlayerConfig")]
    public class PlayerStatsSO : ScriptableObject
    {
        public int Level;
        public int MaxHP = 100;
        public float Speed;
        public float AttackRange;
        public float AttackSpeed;
        public float Damage;
        public float AttackCooldownDuration;
    }

    [System.Serializable]
    public struct PlayerSkins
    {
        public int ID;
        public RuntimeAnimatorController runtimeAnimatorController;
    }
}