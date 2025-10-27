using UnityEngine;

namespace AbilitySystem.Ability
{
    public class DeathSentenceAbility : AbilityData
    {
        /* Editor Values */
        [SerializeField] private DamageEffect m_damageEffect;
        
        public DamageEffect DamageEffect => m_damageEffect;

        public override AbilityData Clone()
        {
            DeathSentenceAbility clone = (DeathSentenceAbility)this.MemberwiseClone();

            return clone;
        }
        
        
        /* Runtime Values */
        private TowerWaves m_towerWaves;

        public override void Init(AbilityInitData initData)
        {
            m_towerWaves = initData.Caster.GetComponent<TowerWaves>();
        }

        public override bool TryActivate(GameObject target, GameObject caster, int level = 1)
        {
            Unit randomUnit = m_towerWaves.GetRandomUnit();
            if (randomUnit == null)
                return false;

            randomUnit.EffectsContainer.ApplyEffect(caster, m_damageEffect, level);
            
            return true;
        }
    }
}