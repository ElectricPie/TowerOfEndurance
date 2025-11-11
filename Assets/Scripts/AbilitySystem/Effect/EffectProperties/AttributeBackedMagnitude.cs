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
    public class AttributeBackedMagnitude
    {
        [SerializeField] private AttributeSource m_attributeSource;
        [SerializeField] private AttributeIdScriptableObject m_backingAttributeId;
        [SerializeField] private ScalableFloat m_coefficient;
        [SerializeField] private ScalableFloat m_postAdditiveValue;

        public AttributeSource AttributeSource => m_attributeSource;
        public AttributeIdScriptableObject BackingAttributeId => m_backingAttributeId;
        public ScalableFloat Coefficient => m_coefficient;
        public ScalableFloat PostAdditiveValue => m_postAdditiveValue;
    }
}