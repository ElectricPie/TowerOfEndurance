using System;
using System.Collections;
using System.Collections.Generic;
using AbilitySystem.Ability.Attributes;
using AbilitySystem.Ability.AttributeSets;
using AbilitySystem.Effect;
using AbilitySystem.Effect.EffectProperties;
using UnityEngine;

namespace AbilitySystem.Ability.Data
{
    [Serializable]
    public class RapidFireAbilityData : AbilityData
    {
        [SerializeField] private GameEffectScriptableObject m_fireRateEffect;
        [SerializeField] private ScalableFloat m_duration;

        public GameEffectScriptableObject FireRateEffect => m_fireRateEffect;
        public ScalableFloat Duration => m_duration;

        public override AbilityInstance CreateAbilityInstance(AbilityInitData initData)
        {
            return new RapidFireAbilityInstance(initData);
        }
    }

    public class RapidFireAbilityInstance : AbilityInstance
    {
        private readonly RapidFireAbilityData m_abilityData;
        private readonly AttributeSet m_towerAttributeSet;
        private readonly MonoBehaviour m_coroutineRunner;
        private bool m_isActive;

        public RapidFireAbilityInstance(AbilityInitData initData) : base(initData)
        {
            if (initData.AbilityData is not RapidFireAbilityData rapidFireAbilityData)
                throw new Exception("Tried to initialize RapidFire ability with non RapidFireAbilityData");
            
            m_abilityData = rapidFireAbilityData;
            m_towerAttributeSet = initData.Source.GetComponent<AttributeSet>();
            m_coroutineRunner = initData.Source.GetComponent<MonoBehaviour>();
        }

        public override void TryActivate(GameObject target = null)
        {
            if (m_isActive)
                return;

            m_isActive = true;

            List<AttributeModifierInstance> appliedMods = new List<AttributeModifierInstance>();
            foreach (AttributeModifier modifier in m_abilityData.FireRateEffect.Modifiers)
            {
                AttributeModifierInstance mod = new AttributeModifierInstance(m_towerAttributeSet, modifier, Level);
                m_towerAttributeSet.AddInfiniteModifier(mod);
                appliedMods.Add(mod);
            }

            m_coroutineRunner.StartCoroutine(RevertAfterDuration(appliedMods));
        }

        private IEnumerator RevertAfterDuration(List<AttributeModifierInstance> mods)
        {
            yield return new WaitForSeconds(m_abilityData.Duration.GetValue(Level));
            foreach (AttributeModifierInstance mod in mods)
                m_towerAttributeSet.RemoveInfiniteModifier(mod);
            
            m_isActive = false;
        }
    }
}