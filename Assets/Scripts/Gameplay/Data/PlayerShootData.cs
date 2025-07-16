using UnityEngine;

namespace Gameplay.Data
{
    [CreateAssetMenu (fileName = "PlayerShootData", menuName = "Player/PlayerShootData")]
    public class PlayerShootData : ScriptableObject
    {
        public int MaxBulletCount;
        public float BulletDamage;
        public float FireRate;
        public Bullet BulletPrefab;
    }
}