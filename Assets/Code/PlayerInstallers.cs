using Code.Player;
using UnityEngine;
using Zenject;

namespace Code.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private GameObject playerPrefab;

        public override void InstallBindings()
        {
            PlayerMovement playerMovement = Container.InstantiatePrefabForComponent<PlayerMovement>(playerPrefab);
            Container
                .Bind<PlayerMovement>()
                .FromInstance(playerMovement)
                .AsSingle();
        }
    }
}
