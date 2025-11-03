using System;
using System.Collections.Generic;

namespace AbilitySystem.Ability.Attributes
{
    public class AttributeData
    {
        private float m_currenValue;
        
        public float BaseValue;
        public float CurrentValue => m_currenValue;

        public event Action<float> OnBaseValueChangedEvent = delegate { };
        public event Action<float> OnCurrentValueChangedEvent = delegate { };

        public readonly List<AttributeModifier> Modifiers = new List<AttributeModifier>();

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