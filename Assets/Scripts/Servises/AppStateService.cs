using System;
using Settings;
using UI;
using Zenject;

namespace Core.Service
{
    public class AppStateService : IAppStateService
    {
        public event Action AppStateChangedEvent;

        public Enumerators.AppState AppState { get; private set; } = Enumerators.AppState.Unknown;

        [Inject] private IUIService _uiService;

        public void ChangeAppState(Enumerators.AppState stateTo)
        {
            if (AppState == stateTo)
                return;
            
            AppState = stateTo;

            switch (stateTo)
            {
                case Enumerators.AppState.Main:
                    _uiService.HideAll();
                    _uiService.Show<HomePage>();
                    break;
                case Enumerators.AppState.Game:
                    _uiService.Show<GamePage>();
                    _uiService.Hide<HomePage>();
                    break;
                case Enumerators.AppState.Lose:
                    _uiService.Show<LosePopup>();
                    _uiService.Hide<GamePage>();
                    break;
                case Enumerators.AppState.Win:
                    _uiService.Show<WinPopup>();
                    _uiService.Hide<GamePage>();
                    break;
            }

            AppStateChangedEvent?.Invoke();
        }
    }
}