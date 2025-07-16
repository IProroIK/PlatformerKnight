using System;
using Core.Items;
using Core.Service;
using Settings;
using UnityEngine;
using Zenject;

public class PlayerHealthController : MonoBehaviour, IDamageable
{
    private static readonly int PlayerState = Animator.StringToHash("playerState");
    public event Action PlayerDeathEvent;
    
    [SerializeField] private Animator animator;
    private IAppStateService _appStateService;

    [Inject]
    private void Construct(IAppStateService appStateService)
    {
        _appStateService = appStateService;
    }
    
    public void Damage(float damage)
    {
        if(_appStateService.AppState != Enumerators.AppState.Game)
            return;
        
        animator.SetInteger(PlayerState, (int)Enumerators.PlayerAnimationState.Death);
        _appStateService.ChangeAppState(Enumerators.AppState.Lose);
        PlayerDeathEvent?.Invoke();    
    }
}
