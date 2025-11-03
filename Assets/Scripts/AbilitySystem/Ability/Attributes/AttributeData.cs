using System;
using System.Collections.Generic;
using AbilitySystem.Ability.AttributeSets;
using AbilitySystem.Effect;

namespace AbilitySystem.Ability.Attributes
{
    public class AttributeModifierInstance
    {
        public readonly AttributeSet Source;
        public readonly AttributeModifier Modifier;
        public readonly int Level;

        public AttributeModifierInstance(AttributeSet source, AttributeModifier attributeModifier, int level)
        {
            Source = source;
            Modifier = attributeModifier;
            Level = level;
        }
    }
    
    
    public class AttributeData
    {
        private float m_currenValue;
        
        public float BaseValue;
        public float CurrentValue => m_currenValue;

        public event Action<float> OnBaseValueChangedEvent = delegate { };
        public event Action<float> OnCurrentValueChangedEvent = delegate { };

        public readonly List<AttributeModifierInstance> Modifiers = new List<AttributeModifierInstance>();

        public void SetCurrentValue(float newValue, bool broadcastChange = true)
        {
            m_currenValue = newValue;

            if (broadcastChange)
            {
                BroadcastCurrentValue();
            }
        }

        public void BroadcastBaseValue()
        {
            OnBaseValueChangedEvent.Invoke(BaseValue);
        }
        public void BroadcastCurrentValue()
        {
            OnCurrentValueChangedEvent.Invoke(CurrentValue);
        }
    }
}