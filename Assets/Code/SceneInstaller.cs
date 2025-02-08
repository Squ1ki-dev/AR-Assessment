using System;
using System.Collections;
using System.Collections.Generic;
using Code.Services;
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
        public override void InstallBindings()
        {
            BindWave();
            BindGameState();
            BindUI();
            BindAssetProvider();
        }

        private void BindAssetProvider()
        {
            Container
                .Bind<IAssetLoader>()
                .To<AddressableAssetProvider>()
                .AsTransient();
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
    }
}
