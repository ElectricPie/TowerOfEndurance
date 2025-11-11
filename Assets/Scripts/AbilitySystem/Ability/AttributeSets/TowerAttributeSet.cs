using UnityEngine;

namespace AbilitySystem.Ability.AttributeSets
{
    public class TowerAttributeSet : AttributeSet
    { 
        [SerializeField]
        private AnimationCurve m_damageCurve;
        [SerializeField]
        private int m_damageLevel = 1;

        [SerializeField]
        private AnimationCurve m_fireRateCurve;
        [SerializeField]
        private int m_fireRateLevel = 1;
        

        public float Damage => m_damageCurve.Evaluate(m_damageLevel);
        public int DamageLevel => m_damageLevel;

        public float FireRate => 1 / m_fireRateCurve.Evaluate(m_fireRateLevel);
        public int FireRateLevel => m_fireRateLevel;

        public float DamageAt(int level)
        {
            return m_damageCurve.Evaluate(level);
        }

        public void IncreaseDamageLevel()
        {
            m_damageLevel++;
        }

        public float FireRateAt(int level)
        {
            return 1 / m_fireRateCurve.Evaluate(level);
        }
        
        public void IncreaseFireRateLevel()
        {
            m_fireRateLevel++;
        }
    }
}