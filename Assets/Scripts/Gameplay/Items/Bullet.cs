using System;
using Core.Items;
using Settings;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    public event Action<IPoolable> Despawned;

    [SerializeField] private Rigidbody2D _rigidbody;
        
    private float _lifeTimeTimer;
    private float _damage;

    public void Shot(Vector3 direction)
    {
        _rigidbody.AddForce(direction * 35, ForceMode2D.Impulse);
    }

    public void SetData(float damage)
    {
        _damage = damage;
    }

    private void Update()
    {
        _lifeTimeTimer -= Time.deltaTime;

        if (_lifeTimeTimer <= 0)
        {
            Despawned?.Invoke(this);
        }            
    }

    private void OnEnable()
    {
        _lifeTimeTimer = Constants.BulletLifeTime;
    }

    private void OnDisable()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log(other.gameObject.name);

            damageable.Damage(_damage);
        }
            
        Despawned?.Invoke(this);
    }
}