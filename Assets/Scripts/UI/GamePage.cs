using System;
using Core.Items;
using Core.Service;
using Settings;
using TMPro;
using Tools;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class GamePage : MonoBehaviour, IUIElement
    {
        [SerializeField] private Button _moveRightButton;
        [SerializeField] private Button _moveLeftButton;
        [SerializeField] private Button _moveUpButton;
        
        [SerializeField] private Button _fireButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private TextMeshProUGUI _textBulletsCount;
        
        private PlayerMovementController _playerMovementController;
        private PlayerShootController _playerShootController;
        
        private bool _isHoldRight;
        private bool _isHoldLeft;
        private IUIService _uiService;

        private IAppStateService _appStateService;

        [Inject]
        private void Construct(PlayerMovementController playerMovementController, 
            PlayerShootController playerShootController, 
            IUIService uiService, IAppStateService appStateService)
        {
            _appStateService = appStateService;
            _uiService = uiService;
            _playerShootController = playerShootController;
            _playerMovementController = playerMovementController;
        }

        public void Init()
        {
            var handlerRight = _moveRightButton.AddComponent<PointerEventsHandler>();
            handlerRight.OnEnterEvent += MoveRightOnPointerEnter;
            handlerRight.OnExitEvent += MoveRightOnPointerExit;
            
            var handlerLeft = _moveLeftButton.AddComponent<PointerEventsHandler>();
            handlerLeft.OnEnterEvent += MoveLeftOnPointerEnter;
            handlerLeft.OnExitEvent += MoveLeftOnPointerExit;

            _moveUpButton.onClick.AddListener(MoveUpButtonHandler);
            _fireButton.onClick.AddListener(FireButtonHandler);
            _pauseButton.onClick.AddListener(PauseButtonHandler);
        }

        private void OnEnable()
        {
            _appStateService.AppStateChangedEvent += AppStateChangedEventHandler;
        }

        private void AppStateChangedEventHandler()
        {
            if(_appStateService.AppState == Enumerators.AppState.Game)
                _textBulletsCount.text = $"{_playerShootController.MaxBulletCount}/{_playerShootController.MaxBulletCount}";
        }

        public void Show(object data = null)
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_isHoldRight)
                _playerMovementController.SetMovementInput(1);
            if(_isHoldLeft)
                _playerMovementController.SetMovementInput(-1);
        }

        private void OnDestroy()
        {
            _appStateService.AppStateChangedEvent -= AppStateChangedEventHandler;
        }

        private void MoveUpButtonHandler()
        {
            _playerMovementController.Jump();
        }

        private void FireButtonHandler()
        {
            _playerShootController.Shot();
            _textBulletsCount.text = $"{_playerShootController.CurrentBulletCount}/{_playerShootController.MaxBulletCount}";
        }

        private void PauseButtonHandler()
        {
            _uiService.Show<PausePopup>();
        }
        
        private void MoveRightOnPointerEnter()
        {
            _isHoldRight = true;
        }

        private void MoveRightOnPointerExit()
        {
            _isHoldRight = false;
            _playerMovementController.SetMovementInput(0);
        }

        private void MoveLeftOnPointerEnter()
        {
            _isHoldLeft = true;
        }

        private void MoveLeftOnPointerExit()
        {
            _isHoldLeft = false;
            _playerMovementController.SetMovementInput(0);
        }
    }
}