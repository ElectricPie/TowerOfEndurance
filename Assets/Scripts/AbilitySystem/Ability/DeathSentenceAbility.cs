using System;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem.Ability
{
    [Serializable]
    public class DeathSentenceAbility : AbilityData
    {
        /* Editor Values */
        [SerializeReference] private DamageEffect m_damageEffect;

        public override AbilityData Clone()
        {
            DeathSentenceAbility clone = (DeathSentenceAbility)MemberwiseClone();

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

        public override Dictionary<string, object> GetTooltipDataMap(int level)
        {
            Dictionary<string, object> tooltipDataMap = new Dictionary<string, object>();
            tooltipDataMap.TryAdd("Cost", GetCostAt(level));
            float attacks = m_damageEffect.PeriodicEffectValues.GetDurationAt(level) / m_damageEffect.PeriodicEffectValues.GetPeriodAt(level);
            tooltipDataMap.TryAdd("Attacks", attacks);
            tooltipDataMap.TryAdd("DamagePercent", m_damageEffect.DamageModifierAt(level) * 100);
            tooltipDataMap.TryAdd("DamageModifier", m_damageEffect.DamageModifierAt(level));
            tooltipDataMap.TryAdd("TriggerTime", GetTriggerTimeAt(level));
            return tooltipDataMap;
        }
    }
}