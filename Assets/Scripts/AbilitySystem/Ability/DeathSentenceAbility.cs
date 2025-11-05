using System;
using System.Collections.Generic;
using AbilitySystem.Effect;
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
    }

    public class DeathSentenceAbilityInstance : AbilityInstance
    {
        private DeathSentenceAbilityData m_abilityData;
        private TowerWaves m_towerWaves;

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