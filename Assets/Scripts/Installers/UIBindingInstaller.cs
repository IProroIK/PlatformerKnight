using Core.Service;
using UI;
using UnityEngine;
using Zenject;

namespace Core.Bindings
{
    public class UIBindingInstaller : MonoInstaller
    {
        [SerializeField] private GamePage gamePage;
        [SerializeField] private HomePage homePage;
        [SerializeField] private PausePopup pausePopup;
        [SerializeField] private LosePopup losePopup;
        [SerializeField] private WinPopup winPopup;


        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GamePage>().FromInstance(gamePage).AsSingle();
            Container.BindInterfacesTo<HomePage>().FromInstance(homePage).AsSingle();
            Container.BindInterfacesTo<PausePopup>().FromInstance(pausePopup).AsSingle();
            Container.BindInterfacesTo<LosePopup>().FromInstance(losePopup).AsSingle();
            Container.BindInterfacesTo<WinPopup>().FromInstance(winPopup).AsSingle();

            Container.Resolve<IUIService>().Register<GamePage>(gamePage);
            Container.Resolve<IUIService>().Register<HomePage>(homePage);
            Container.Resolve<IUIService>().Register<PausePopup>(pausePopup);
            Container.Resolve<IUIService>().Register<LosePopup>(losePopup);
            Container.Resolve<IUIService>().Register<WinPopup>(winPopup);
        }
    }
}