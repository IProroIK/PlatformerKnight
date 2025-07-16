using Core.Items;
using Core.Service;
using Settings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class WinPopup : MonoBehaviour, IUIElement
    {
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button quitButton;
        private IAppStateService _appStateService;

        [Inject]
        private void Construct(IAppStateService appStateService)
        {
            _appStateService = appStateService;
        }
        
        public void Init()
        {
            nextLevelButton.onClick.AddListener(ButtonNextLevelClickHandler);
            quitButton.onClick.AddListener(ButtonQuitClickHandler);
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
            Hide();
        }

        private void ButtonNextLevelClickHandler()
        {
            _appStateService.ChangeAppState(Enumerators.AppState.Game);
            Hide();
        }
    }
}