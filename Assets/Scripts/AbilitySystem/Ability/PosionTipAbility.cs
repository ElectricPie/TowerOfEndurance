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