using Core.Service;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Gameplay.Enemy
{
    public class EnemyShooter : PatrollingEnemyBase
    {
        [Header("Shooting")]
        [SerializeField] private Transform shootPoint;
        [SerializeField] private float fireRate = 1f;
        [SerializeField] private float bulletDamage = 5f;
        [SerializeField] private Bullet bulletPrefab;

        private float _fireCooldown;
        private IPoolService _poolService;

        [Inject]
        public void Construct(IPoolService poolService)
        {
            _poolService = poolService;
        }
        
        protected override void Update()
        {
            base.Update();

            if (PlayerInRange != null)
            {
                RotateToFacePlayerOnce(PlayerInRange.transform.position);

                _fireCooldown -= Time.deltaTime;
                if (_fireCooldown <= 0f)
                {
                    Shoot(PlayerInRange);
                    _fireCooldown = fireRate;
                }
            }
        }
        
        private void RotateToFacePlayerOnce(Vector3 playerPosition)
        {
            float directionX = playerPosition.x - transform.position.x;

            if (directionX == 0) return;

            float currentY = transform.eulerAngles.y;
            bool facingRight = currentY == 0f;
            bool playerIsToRight = directionX > 0;

            if (facingRight != playerIsToRight)
            {
                // Flip by 180 degrees on Y axis
                float newY = playerIsToRight ? 0 : 180f;
                Vector3 newRotation = transform.eulerAngles;
                newRotation.y = newY;
                transform.eulerAngles = newRotation;
            }
        }

        private void Shoot(GameObject player)
        {
            var pool = _poolService.GetPool<Bullet>();
            var bullet = pool.Spawn(shootPoint.position, Quaternion.identity);

            bullet.transform.position = shootPoint.position;
            bullet.transform.rotation = Quaternion.identity;

            Vector3 direction = (player.transform.position - shootPoint.position).normalized;
            bullet.Shot(direction);
            bullet.SetData(bulletDamage);
            bullet.gameObject.SetActive(true);
        }
    }
    
}