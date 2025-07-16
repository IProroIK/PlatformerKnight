using Core.Items;
using Core.Service;
using Settings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class PausePopup : MonoBehaviour, IUIElement
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button exitButton;
        private IAppStateService _appStateService;

        [Inject]
        private void Construct(IAppStateService appStateService)
        {
            _appStateService = appStateService;
        }
        
        public void Init()
        {
            closeButton.onClick.AddListener(CloseButtonClickedHandler);
            continueButton.onClick.AddListener(ContinueButtonClickedHandler);
            exitButton.onClick.AddListener(ExitButtonClickedHandler);
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

        private void CloseButtonClickedHandler()
        {
            Hide();
        }

        private void ContinueButtonClickedHandler()
        {
            Hide();
        }

        private void ExitButtonClickedHandler()
        {
            _appStateService.ChangeAppState(Enumerators.AppState.Main);
        }
    }
}