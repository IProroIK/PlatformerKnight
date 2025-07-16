using UnityEngine;

namespace Gameplay.Enemy
{
    public class PatrollingEnemy : PatrollingEnemyBase
    {
        [SerializeField] private float _speedToPlayer = 50f;

        private Rigidbody2D _rb;
        private GameObject _player;

        protected override void Start()
        {
            base.Start();
            _rb = GetComponent<Rigidbody2D>();
        }

        protected override void Update()
        {
            if (_player != null)
            {
                IsPatrolling = false;
                FollowPlayer(_player);
            }
            else if (IsPatrolling)
            {
                Patrol();
            }
        }

        private void FollowPlayer(GameObject player)
        {
            if (player == null || _rb == null) return;

            Vector2 direction = ((Vector2)player.transform.position - _rb.position).normalized;
            Vector2 nextPos = _rb.position + direction * _speedToPlayer * Time.deltaTime;

            _rb.MovePosition(nextPos);

            // Flip
            if (direction.x != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Sign(direction.x) * Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
        }


        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);

            if (((1 << collision.gameObject.layer) & playerLayer) == 0) return;

            Attack(collision.gameObject);
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & playerLayer) == 0) return;

            IsPatrolling = false;
            _player = other.gameObject;
        }

        protected override void OnTriggerExit2D(Collider2D other)
        {
        }

        protected override void Attack(GameObject player)
        {
            base.Attack(player);
        }

        protected override void Patrol()
        {
            base.Patrol();
        }
    }
}