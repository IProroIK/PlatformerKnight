using System;
using Core.Items;
using UnityEngine;

namespace Gameplay
{
    public class Spikes : MonoBehaviour
    {
        private const float Damage = Mathf.Infinity;
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.transform.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.Damage(Damage);
            }
        }
    }
}