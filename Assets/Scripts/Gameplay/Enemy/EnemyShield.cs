using System;
using Tools;
using UnityEngine;

namespace Gameplay.Enemy
{
    public class EnemyShield : PatrollingEnemyBase
    {
        [SerializeField] private Transform shieldPoint;
        private GameObject _shield;
        private Collision2DEvents _collisionEvent;
        
        protected override void Start()
        {
            base.Start();
            
            var shieldPrefab = Resources.Load<GameObject>("Prefabs/Shield");
            _shield = Instantiate(shieldPrefab, shieldPoint.position, Quaternion.identity);
            _collisionEvent = _shield.GetComponent<Collision2DEvents>();
            _collisionEvent.OnCollisionEnter2DEvent += OnCollisionEnter2D;
        }

        protected override void Update()
        {
            base.Update();
            
            _shield.transform.position = shieldPoint.position;
        }

        private void OnDestroy()
        {
            _collisionEvent.OnCollisionEnter2DEvent -= OnCollisionEnter2D;
        }

        public override void DestroySelf()
        {
            base.DestroySelf();
            Destroy(_shield);
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);

            if (((1 << collision.gameObject.layer) & playerLayer) == 0) return;

            Attack(collision.gameObject);
        }

        protected override void Flip(int dir)
        {
            base.Flip(dir);
            
            var isFacingRight = dir > 0;

            float yRotation = isFacingRight ? 0f : 180f;
            _shield.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        }

        public override void Damage(float damage)
        {
            base.Damage(damage);
            Destroy(_shield);
        }
    }
}