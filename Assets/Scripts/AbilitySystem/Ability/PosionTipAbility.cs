using System;
using System.Collections.Generic;
using AbilitySystem.Effect;
using UnityEngine;

namespace AbilitySystem.Ability
{
    [Serializable]
    public class PoisonTipAbilityData : AbilityData
    {
        /* Editor Values */
        [SerializeField] private GameEffectScriptableObject m_damageEffectNew;

        public override AbilityData Clone()
        {
            PoisonTipAbilityData clone = (PoisonTipAbilityData)MemberwiseClone();

            return clone;
        }

        /* Runtime Values */
        public override bool TryActivate(GameObject target, GameObject caster, int level = 1)
        {
            if (target == null || caster == null)
                return false;

            EffectsContainer effectsContainer = target.GetComponent<EffectsContainer>();
            if (effectsContainer == null)
                return false;

            effectsContainer.ApplyEffect(caster, m_damageEffectNew, level);
            return true;
        }

        public override Dictionary<string, object> GetTooltipDataMap(int level)
        {
            Dictionary<string, object> tooltipDataMap = new Dictionary<string, object>();
            tooltipDataMap.TryAdd("Cost", GetCostAt(level));
            // tooltipDataMap.TryAdd("Duration", m_damageEffect.PeriodicEffectValues.GetDurationAt(level));
            // tooltipDataMap.TryAdd("Period", m_damageEffect.PeriodicEffectValues.GetPeriodAt(level));
            // tooltipDataMap.TryAdd("DamagePercent", m_damageEffect.DamageModifierAt(level) * 100);
            // tooltipDataMap.TryAdd("DamageModifier", m_damageEffect.DamageModifierAt(level));
            return tooltipDataMap;
        }
    }
}