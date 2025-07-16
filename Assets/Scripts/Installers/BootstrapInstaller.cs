using Core.Service;
using Settings;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ICameraService>().To<CameraService>().AsSingle().NonLazy();
        Container.Bind<IAppStateService>().To<AppStateService>().AsSingle().NonLazy();
        Container.Bind<IUIService>().To<UIService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<InputService>().AsSingle().NonLazy();
        
        Container.Resolve<IAppStateService>().ChangeAppState(Enumerators.AppState.Main);
    }
}