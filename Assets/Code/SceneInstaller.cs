using System;
using System.Collections;
using System.Collections.Generic;
using Code.Enemies;
using Code.UI;
using Code.Wave;
using UnityEngine;
using Zenject;

namespace Code
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private WaveSpawner _waveSpawner;
        [SerializeField] private GameState _gameState;
        [SerializeField] private PanelManager _panelManager;
        [SerializeField] private Transform _enemyTarget;

        public override void InstallBindings()
        {
            BindWave();
            BindEnemyTarget();
            BindGameState();
            BindUI();
        }

        private void BindGameState()
        {
            Container
                .Bind<GameState>()
                .FromInstance(_gameState)
                .AsSingle();
        }

        private void BindUI()
        {
            Container
                .Bind<PanelManager>()
                .FromInstance(_panelManager)
                .AsSingle();
        }

        private void BindWave()
        {
            Container
                .Bind<WaveSpawner>()
                .FromInstance(_waveSpawner)
                .AsSingle();
        }

        private void BindEnemyTarget()
        {
            // Container
            //     .Bind<Transform>()
            //     .FromInstance(_enemyTarget)
            //     .AsSingle()
            //     .WhenInjectedInto<EnemyMovement>();
        }
    }
}
