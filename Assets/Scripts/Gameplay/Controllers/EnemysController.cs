using System;
using System.Collections.Generic;
using Core.Service;
using Settings;
using UnityEngine;
using Zenject;

public class EnemysController : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnData> enemySpawnData;
    [SerializeField] private ParticleSystem deathParticles;
    private DiContainer _container;
    private List<PatrollingEnemyBase> _patrollingEnemies;
    private IAppStateService _appStateService;

    [Inject]
    private void Construct(DiContainer container, IAppStateService appStateService)
    {
        _appStateService = appStateService;
        _container = container;
    }

    public void Awake()
    {
        _patrollingEnemies = new List<PatrollingEnemyBase>();
        
        _appStateService.AppStateChangedEvent += AppStateChangedEventHandler;
    }

    private void OnDestroy()
    {
        _appStateService.AppStateChangedEvent -= AppStateChangedEventHandler;
    }

    private void AppStateChangedEventHandler()
    {
        if (_appStateService.AppState == Enumerators.AppState.Game)
        {
            foreach (var spawnData in enemySpawnData)
            {
                var enemy = _container.InstantiatePrefabForComponent<PatrollingEnemyBase>(spawnData.EnemyPrefab, transform);
                enemy.SetPosition(spawnData.SpawnPoint.position);
                _patrollingEnemies.Add(enemy);
                enemy.DeathEvent += DeathEventHandler;
            }
        }
        else
        {
            Reset();
        }
    }

    private void Reset()
    {
        foreach (var enemy in _patrollingEnemies)
        {
            enemy.DestroySelf();
        }
        _patrollingEnemies.Clear();
    }

    private void DeathEventHandler(PatrollingEnemyBase obj)
    {
        if (_appStateService.AppState != Enumerators.AppState.Game)
            return;
            
        _patrollingEnemies.Remove(obj);
        deathParticles.transform.position = obj.transform.position;
        deathParticles.Play();
        
        if(_patrollingEnemies.Count == 0)
        {
            _appStateService.ChangeAppState(Enumerators.AppState.Win);
        }
    }
}

[Serializable]
public class EnemySpawnData
{
    public PatrollingEnemyBase EnemyPrefab;
    public Transform SpawnPoint;
}