using Core.Service;
using Settings;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform _initialPosition;
        private IAppStateService _appStateService;

        [Inject]
        private void Construct(IAppStateService appStateService)
        {
            _appStateService = appStateService;
        }

        private void Awake()
        {
            _appStateService.AppStateChangedEvent += AppStateChangedEventHandler;
        }

        private void OnDestroy()
        {
            _appStateService.AppStateChangedEvent -= AppStateChangedEventHandler;
        }

        private void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        private void AppStateChangedEventHandler()
        {
            if (_appStateService.AppState == Enumerators.AppState.Game)
            {
                SetPosition(_initialPosition.position);
            }
        }
    }
}