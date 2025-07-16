using Core.Service;
using Settings;
using UnityEngine;
using Zenject;

namespace Gameplay.Controllers
{
    public class TutorController : MonoBehaviour
    {
        private IAppStateService _appStateService;

        [Inject]
        private void Construct(IAppStateService appStateService)
        {
            _appStateService = appStateService;
            _appStateService.AppStateChangedEvent += AppStateChangedEventHandler;
        }
        
        private void OnDestroy()
        {
            _appStateService.AppStateChangedEvent -= AppStateChangedEventHandler;
        }

        private void AppStateChangedEventHandler()
        {
            if(_appStateService.AppState == Enumerators.AppState.Game)
                gameObject.SetActive(true);
            else
                gameObject.SetActive(false);
        }
    }
}