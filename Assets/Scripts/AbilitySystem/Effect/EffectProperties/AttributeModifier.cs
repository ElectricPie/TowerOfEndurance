using System;
using AbilitySystem.Ability.Attributes;
using UnityEngine;

namespace AbilitySystem.Effect.EffectProperties
{
    [Serializable]
    public enum ModifierOperation
    {
        Add,
        Override
    }
    
    [Serializable]
    public class AttributeModifier
    {
        [SerializeField] private AttributeIdScriptableObject m_attributeId;
        [SerializeField] private ModifierOperation m_modifierOperation = ModifierOperation.Add;
        [SerializeField] private ModifierMagnitude m_modifierMagnitude = new ModifierMagnitude();

        public AttributeIdScriptableObject AttributeId => m_attributeId;
        public ModifierOperation Operation => m_modifierOperation;
        public ModifierMagnitude Magnitude => m_modifierMagnitude;
    }

}