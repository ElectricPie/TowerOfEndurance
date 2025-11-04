using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AbilitySystem.Effect.EffectProperties
{
    [Serializable]
    public enum CalculationType
    {
        Float,
        AttributeBacked
    }
    
    [Serializable]
    public class ModifierMagnitude
    {
        [SerializeField] private CalculationType m_calculationType = CalculationType.Float;
        // TODO: Replace with CurveFloat
        [SerializeField, ShowIf("m_calculationType", CalculationType.Float)] private float m_flatValue;
        [TitleGroup("Attribute Backed Magnitude")]
        [SerializeField, ShowIf("m_calculationType", CalculationType.AttributeBacked), HideLabel]
        private AttributeBackedMagnitude m_attributeBackedMagnitude;

        public CalculationType CalculationType => m_calculationType;
        public float FlatValue => m_flatValue;
        public AttributeBackedMagnitude AttributeBackedMagnitude => m_attributeBackedMagnitude;
    }
}