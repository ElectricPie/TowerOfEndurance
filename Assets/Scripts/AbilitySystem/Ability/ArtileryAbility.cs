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
        [SerializeField] private AnimationCurve m_triggerChanceCurve;
        [SerializeField, Min(0)] private int m_targetCount = 2;
        [SerializeField] private GameEffectScriptableObject m_damageEffect;
        
        public AnimationCurve TriggerChance => m_triggerChanceCurve;
        public int TargetCount => m_targetCount;
        public GameEffectScriptableObject DamageEffect => m_damageEffect;
        
        public override AbilityInstance CreateAbilityInstance(AbilityInitData initData)
        {
            return new ArtilleryAbilityInstance(initData);
        }

        public override Dictionary<string, object> GetTooltipMap(int level)
        {
            Dictionary<string, object> tooltipMap = new Dictionary<string, object>
            {
                { "DamagePercent", 340 + 40 * level},
                { "DamageModifierValue", (float)(3.4 + 0.4 * level) }, // To be multiplied by tower damage
                { "TargetCount", m_targetCount }
            };

            return tooltipMap;
        }
    }

    public class ArtilleryAbilityInstance : AbilityInstance
    {
        private readonly ArtilleryAbilityData m_abilityData;
        private readonly TowerWaves m_towerWaves;

        public ArtilleryAbilityInstance(AbilityInitData initData) : base(initData)
        {
            if (initData.AbilityData is not ArtilleryAbilityData artilleryAbilityData)
                throw new Exception("Tried to initialize Artillery ability with non ArtilleryAbilityData");

            m_abilityData = artilleryAbilityData;
            m_towerWaves = initData.Source.GetComponent<TowerWaves>();
        }

        public override void TryActivate(GameObject target = null)
        {
            // Trigger chance needs to be below the abilities trigger chance
            int triggerChance = Random.Range(0, 100);
            if (triggerChance > m_abilityData.TriggerChance.Evaluate(Level))
                return;

            for (int i = 0; i < m_abilityData.TargetCount; i++)
            {
                Unit randomTarget = m_towerWaves.GetRandomUnit();
                randomTarget.EffectsContainer.ApplyEffect(Source, m_abilityData.DamageEffect, Level);
            }
        }
    }
}