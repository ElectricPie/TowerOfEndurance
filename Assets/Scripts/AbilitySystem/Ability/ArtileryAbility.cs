using UnityEngine;
using Random = UnityEngine.Random;

namespace AbilitySystem.Ability
{
    public class ArtilleryAbilityData : AbilityData
    {
        /* Editor Values */
        [SerializeField] private AnimationCurve m_triggerChanceCurve;
        [SerializeField, Min(0)] private int m_targetCount;
        [SerializeField] private DamageEffect m_damageEffect = new DamageEffect();
        
        public int TargetCount => m_targetCount;
        public DamageEffect DamageEffect => m_damageEffect;

        public override AbilityData Clone()
        {
            ArtilleryAbilityData clone = (ArtilleryAbilityData)this.MemberwiseClone();
            clone.m_triggerChanceCurve = this.m_triggerChanceCurve;
            clone.m_damageEffect = this.m_damageEffect;

            return clone;
        }

        private float GetTriggerChance(int level)
        {
            return m_triggerChanceCurve.Evaluate(level);
        }

        
        /* Runtime Values */
        private TowerWaves m_towerWaves;
        
        public override void Init(AbilityInitData initData)
        {
            m_towerWaves = initData.Caster.GetComponent<TowerWaves>();
        }

        // Ignoring target for this one
        public override bool TryActivate(GameObject target, GameObject caster, int level = 1)
        {
            // Trigger chance needs to be below the abilities trigger chance
            int triggerChance = Random.Range(0, 100);
            if (triggerChance > GetTriggerChance(level))
                return false;

            for (int i = 0; i < m_targetCount; i++)
            {
                Unit randomTarget = m_towerWaves.GetRandomUnit();
                randomTarget.EffectsContainer.ApplyEffect(caster, m_damageEffect, level);
            }

            return true;
        }
    }
}