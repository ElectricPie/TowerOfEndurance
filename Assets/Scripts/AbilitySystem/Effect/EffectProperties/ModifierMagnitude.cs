using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AbilitySystem.Effect.EffectProperties
{
    [Serializable]
    public enum CalculationType
    {
        ScalableFloat,
        AttributeBacked
    }
    
    [Serializable]
    public class ModifierMagnitude
    {
        [SerializeField] private CalculationType m_calculationType = CalculationType.ScalableFloat;
        [SerializeField, ShowIf("m_calculationType", CalculationType.ScalableFloat)] private ScalableFloat m_scalableFloat;
        [TitleGroup("Attribute Backed Magnitude")]
        [SerializeField, ShowIf("m_calculationType", CalculationType.AttributeBacked), HideLabel]
        private AttributeBackedMagnitude m_attributeBackedMagnitude;

        public CalculationType CalculationType => m_calculationType;
        public ScalableFloat ScalableFloat => m_scalableFloat;
        public AttributeBackedMagnitude AttributeBackedMagnitude => m_attributeBackedMagnitude;
    }
}