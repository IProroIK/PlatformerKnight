using Core.Items;
using Core.Service;
using Gameplay.Data;
using Settings;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Zenject.SpaceFighter;

public class PlayerShootController : MonoBehaviour
{
    public int CurrentBulletCount {get; private set;}
    public int MaxBulletCount {get; private set;}
    
    [SerializeField] private Transform shootPoint;
    private float _fireRate;
    private float _bulletDamage;
    private Bullet _bulletPrefab;

    private PlayerShootData _playerShootData;
    
    private float _fireCooldown;

    private IPoolService _poolService;
    private IAppStateService _appStateService;

    [Inject]
    private void Construct(IPoolService poolService, IAppStateService appStateService)
    {
        _appStateService = appStateService;
        _poolService = poolService;
    }
    
    private void Awake()
    {
        _playerShootData = Resources.Load<PlayerShootData>("Data/PlayerShootData");

        MaxBulletCount = _playerShootData.MaxBulletCount;
        _bulletPrefab = _playerShootData.BulletPrefab;
        _fireRate = _playerShootData.FireRate;
        _bulletDamage = _playerShootData.BulletDamage;
        Debug.Log(MaxBulletCount);

        _poolService.CreatePool<Bullet>(_bulletPrefab, 20);
        _appStateService.AppStateChangedEvent += AppStateChangedEventHandler;
    }

    private void Update()
    {
        _fireCooldown -= Time.deltaTime;
    }

    private void OnDestroy()
    {
        _appStateService.AppStateChangedEvent -= AppStateChangedEventHandler;
    }

    public void Shot()
    {
        if (!(_fireCooldown <= 0f)) return;
        
        Shoot();
        _fireCooldown = _fireRate;
    }

    private void Shoot()
    {
        if(CurrentBulletCount <= 0)
            return;
        
        var pool= _poolService.GetPool<Bullet>();
        var bullet = pool.Spawn(shootPoint.position, Quaternion.identity);
        
        bullet.transform.position = shootPoint.position;
        bullet.transform.rotation = shootPoint.rotation;

        var direction = shootPoint.right.normalized;
        bullet.Shot(direction);
        bullet.SetData(_bulletDamage);

        bullet.gameObject.SetActive(true);
        CurrentBulletCount--;
    }

    private void AppStateChangedEventHandler()
    {
        if (_appStateService.AppState == Enumerators.AppState.Game)
        {
            MaxBulletCount = _playerShootData.MaxBulletCount;
            CurrentBulletCount = MaxBulletCount;
        }
    }
}