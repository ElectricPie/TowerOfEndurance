using System;
using AbilitySystem.Ability.Attributes;
using UnityEngine;

namespace AbilitySystem.Effect.EffectProperties
{
    [Serializable]
    public enum AttributeSource
    {
        Target,
        Source
    } 

    [Serializable]
    public enum AttributeBackedMode
    {
        Multiply,
        LevelLookup
    }
    
    [Serializable]
    public class AttributeBackedMagnitude
    {
        [SerializeField] private AttributeSource m_attributeSource;
        [SerializeField] private AttributeBackedMode m_mode;
        [SerializeField] private AttributeIdScriptableObject m_backingAttributeId;
        [SerializeField] private ScalableFloat m_coefficient;
        [SerializeField] private ScalableFloat m_postAdditiveValue;

        public AttributeSource AttributeSource => m_attributeSource;
        public AttributeBackedMode Mode => m_mode;
        public AttributeIdScriptableObject BackingAttributeId => m_backingAttributeId;
        public ScalableFloat Coefficient => m_coefficient;
        public ScalableFloat PostAdditiveValue => m_postAdditiveValue;
    }
}