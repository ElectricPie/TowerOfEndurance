using System;
using System.Collections.Generic;
using AbilitySystem.Effect;
using UnityEngine;

namespace AbilitySystem.Ability
{
    [Serializable]
    public class PoisonTipAbilityData : AbilityData
    {
        [SerializeField] private GameEffectScriptableObject m_damageEffect;

        public GameEffectScriptableObject DamageEffect => m_damageEffect;

        public override AbilityInstance CreateAbilityInstance(AbilityInitData initData)
        {
            return new PoisonTipAbilityInstance(initData);
        }

        public override Dictionary<string, object> GetTooltipMap(int level, bool isMaxLevel)
        {
            if (!isMaxLevel && level >= 1)
            {
                level--;
            }
            
            Dictionary<string, object> tooltipMap = new Dictionary<string, object>
            {
                { "DamagePercent", (20 + 20 * level).ToString() },
                { "DamageModifierValue", (float)(0.2 + 0.2 * level) }, // To be multiplied by tower damage
            };

            if (m_damageEffect)
            {
                tooltipMap.Add("Period", m_damageEffect.PeriodicEffectValues.Period.GetValue(level));
                tooltipMap.Add("Duration", m_damageEffect.PeriodicEffectValues.Duration.GetValue(level));
            }

            return tooltipMap;
        }
    }

    public class PoisonTipAbilityInstance : AbilityInstance
    {
        private readonly PoisonTipAbilityData m_abilityData;

        public PoisonTipAbilityInstance(AbilityInitData initData) : base(initData)
        {
            if (initData.AbilityData is not PoisonTipAbilityData poisonTipAbilityData)
                throw new Exception("Tried to initialize Poison Tip ability with non PoisonTipAbilityData");
            m_abilityData = poisonTipAbilityData;
        }

        public override void TryActivate(GameObject target = null)
        {
            if (target == null || Source == null)
                return;

            EffectsContainer effectsContainer = target.GetComponent<EffectsContainer>();
            if (effectsContainer == null)
                return;

            effectsContainer.ApplyEffect(Source, m_abilityData.DamageEffect, Level);
        }
    }
}