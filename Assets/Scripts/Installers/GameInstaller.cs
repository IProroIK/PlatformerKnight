using Core.Service;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private PlayerMovementController _playerMovementController;
    [SerializeField] private PlayerShootController _playerShootController;
    
    public override void InstallBindings()
    {
        Container.Bind<IPoolService>().To<PoolService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PlayerMovementController>()
            .FromInstance(_playerMovementController)
            .AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerShootController>()
            .FromInstance(_playerShootController)
            .AsSingle();
    }
}

