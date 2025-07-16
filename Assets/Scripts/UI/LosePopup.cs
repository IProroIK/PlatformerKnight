using Core.Items;
using Core.Service;
using Settings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LosePopup : MonoBehaviour, IUIElement
    {
        [SerializeField] private Button buttonQuit;
        [SerializeField] private Button buttonRestart;
        [SerializeField] private Button buttonClose;
        private IAppStateService _appStateService;

        [Inject]
        private void Construct(IAppStateService appStateService)
        {
            _appStateService = appStateService;
        }
        
        public void Init()
        {
            buttonQuit.onClick.AddListener(ButtonQuitClickHandler);     
            buttonRestart.onClick.AddListener(ButtonRestartClickHandler);     
            buttonClose.onClick.AddListener(ButtonCloseClickHandler);     
        }

        public void Show(object data = null)
        {
            transform.SetAsLastSibling();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void ButtonQuitClickHandler()
        {
            _appStateService.ChangeAppState(Enumerators.AppState.Main);
        }

        private void ButtonRestartClickHandler()
        {
            _appStateService.ChangeAppState(Enumerators.AppState.Game);
        }

        private void ButtonCloseClickHandler()
        {
            _appStateService.ChangeAppState(Enumerators.AppState.Main);
            Hide();
        }
    }
}