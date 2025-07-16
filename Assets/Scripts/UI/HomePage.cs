using Core.Items;
using Core.Service;
using Settings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class HomePage : MonoBehaviour, IUIElement
    {
        [SerializeField] private Button startButton;
        private IAppStateService _appStateService;

        [Inject]
        private void Construct(IAppStateService appStateService)
        {
            _appStateService = appStateService;
        }
        
        public void Init()
        {
            startButton.onClick.AddListener(StartButtonEventHandler);
        }

        public void Show(object data = null)
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void StartButtonEventHandler()
        {
            _appStateService.ChangeAppState(Enumerators.AppState.Game);
        }
    }
}