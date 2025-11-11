using System;
using System.Collections.Generic;
using AbilitySystem.Effect;
using AbilitySystem.Effect.EffectProperties;
using UnityEngine;

namespace AbilitySystem.Ability
{
    [Serializable]
    public class DeathSentenceAbilityData : AbilityData
    {
        [SerializeField] private GameEffectScriptableObject m_damageEffect;

        public GameEffectScriptableObject DamageEffect => m_damageEffect;
        public override AbilityInstance CreateAbilityInstance(AbilityInitData initData)
        {
            return new DeathSentenceAbilityInstance(initData);
        }

        public override Dictionary<string, object> GetTooltipMap(int level)
        {
            Dictionary<string, object> tooltipMap = new Dictionary<string, object>();

            if (m_damageEffect)
            {
                PeriodicEffectValues periodicEffectValues = m_damageEffect.PeriodicEffectValues;
                tooltipMap.Add("AttackAmount",  periodicEffectValues.Duration.GetValue(level) /  periodicEffectValues.Period.GetValue(level));
            }

            return tooltipMap;
        }
    }

    public class DeathSentenceAbilityInstance : AbilityInstance
    {
        private readonly DeathSentenceAbilityData m_abilityData;
        private readonly TowerWaves m_towerWaves;

        public DeathSentenceAbilityInstance(AbilityInitData initData) : base(initData)
        {
            if (initData.AbilityData is not DeathSentenceAbilityData deathSentenceAbilityData)
                throw new Exception("Tried to initialize Death Sentence ability with non DeathSentenceAbilityData");
            
            m_abilityData = deathSentenceAbilityData;
            m_towerWaves = initData.Source.GetComponent<TowerWaves>();
        }
        
        public override void TryActivate(GameObject target = null)
        {
            Unit randomUnit = m_towerWaves.GetRandomUnit();
            if (randomUnit == null)
                return;

            randomUnit.EffectsContainer.ApplyEffect(Source, m_abilityData.DamageEffect, Level);
        }
    }
}