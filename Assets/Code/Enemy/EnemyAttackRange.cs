using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAttack))]
    public class EnemyAttackRange : AttackRange
    {
        [SerializeField] private EnemyAttack _attack;

        private void Start()
        {
            _attack.DisableAttack();
            TriggerObserver.TriggerEnter += TriggerEnter;
            TriggerObserver.TriggerExit += TriggerExit;
        }

        public override void TriggerEnter(Collider collider) => _attack.EnableAttack();
        public override void TriggerExit(Collider collider) => _attack.DisableAttack();
    }
}