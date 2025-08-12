using UnityEngine;

namespace AbilitySystem.Ability.Attributes
{
    public class TowerAttributeSet : AttributeSet
    { 
        [SerializeField]
        private  AnimationCurve m_damageCurve;
        [SerializeField]
        private int m_damageLevel = 1;
        public float FireRate = 1.0f;

        public float Damage => m_damageCurve.Evaluate(m_damageLevel);
        public int DamageLevel => m_damageLevel;

        public float DamageAt(int level)
        {
            return m_damageCurve.Evaluate(level);
        }

        public void IncreaseDamageLevel()
        {
            m_damageLevel++;
        }
    }
}