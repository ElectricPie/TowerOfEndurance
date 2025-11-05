using UnityEngine;

namespace AbilitySystem.Ability.BasicAttack
{
    public class BasicAttackData : MonoBehaviour
    {
        [SerializeField] private Vector3 m_projectileSpawnOffset = new Vector3(0.0f, 2.5f, 0.0f);

        public Vector3 ProjectileSpawnOffset => m_projectileSpawnOffset;
    }
}