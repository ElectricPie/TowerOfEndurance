using System;
using System.Collections.Generic;
using AbilitySystem.Effect;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AbilitySystem.Ability
{
    [Serializable]
    public class ArtilleryAbilityData : AbilityData
    {
        /* Editor Values */
        [SerializeField] private AnimationCurve m_triggerChanceCurve;
        [SerializeField, Min(0)] private int m_targetCount;
        [SerializeField] private GameEffectScriptableObject m_damageEffect;

        public override AbilityData Clone()
        {
            ArtilleryAbilityData clone = (ArtilleryAbilityData)MemberwiseClone();
            clone.m_triggerChanceCurve = m_triggerChanceCurve;

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

        public override Dictionary<string, object> GetTooltipDataMap(int level)
        {
            Dictionary<string, object> tooltipDataMap = new Dictionary<string, object>();
            // tooltipDataMap.TryAdd("Cost", GetCostAt(level));
            // tooltipDataMap.TryAdd("DamagePercent", m_damageEffect.DamageModifierAt(level) * 100);
            // tooltipDataMap.TryAdd("DamageModifier", m_damageEffect.DamageModifierAt(level));
            tooltipDataMap.TryAdd("TargetCount", m_targetCount);
            tooltipDataMap.TryAdd("TriggerChance", m_triggerChanceCurve.Evaluate(level));
            return tooltipDataMap;
        }
    }
}