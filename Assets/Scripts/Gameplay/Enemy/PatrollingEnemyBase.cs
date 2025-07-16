using System;
using UnityEngine;
using Core.Items;

[RequireComponent(typeof(Collider2D))]
public class PatrollingEnemyBase : MonoBehaviour, IDamageable
{
    public event Action<PatrollingEnemyBase> DeathEvent;

    [Header("Patrol Settings")]
    [SerializeField] protected float patrolSpeed = 2f;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float checkDistance = 0.2f;

    [Header("Combat Settings")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected LayerMask playerLayer;

    protected GameObject PlayerInRange;
    protected bool IsPatrolling = true;

    private int _patrolDirection = 1;
    private bool _isFacingRight = true;

    protected virtual void Start()
    {
        
    }
    
    protected virtual void Update()
    {
        if (IsPatrolling)
            Patrol();
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;  
    }

    public virtual void DestroySelf()
    {
        Destroy(gameObject);
    }

    protected virtual void Patrol()
    {
        transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);

        if (IsHittingWall() || !IsGroundAhead())
        {
            _patrolDirection *= -1;
            Flip(_patrolDirection);
        }
    }

    protected bool IsHittingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * _patrolDirection, checkDistance, groundLayer);
    }

    protected bool IsGroundAhead()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, checkDistance, groundLayer);
    }

    protected virtual void Attack(GameObject player)
    {
        var damageable = player.GetComponent<IDamageable>();
        damageable?.Damage(damage);
    }

    public virtual void Damage(float damage)
    {
        DestroySelf();
        DeathEvent?.Invoke(this);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) == 0) return;

        IsPatrolling = false;
        PlayerInRange = other.gameObject;
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == PlayerInRange)
        {
            PlayerInRange = null;
            IsPatrolling = true;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        
    }

    protected virtual void Flip(int dir)
    {
        if (dir == 0) return;

        _isFacingRight = dir > 0;
        float yRotation = _isFacingRight ? 0f : 180f;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
