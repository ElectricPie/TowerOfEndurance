using System;
using AbilitySystem.Ability.Attributes;
using Sirenix.OdinInspector;
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
        [TitleGroup("Magnitude")]
        [SerializeField, HideLabel] private ModifierMagnitude m_modifierMagnitude = new ModifierMagnitude();

        public AttributeIdScriptableObject AttributeId => m_attributeId;
        public ModifierOperation Operation => m_modifierOperation;
        public ModifierMagnitude Magnitude => m_modifierMagnitude;
    }

}