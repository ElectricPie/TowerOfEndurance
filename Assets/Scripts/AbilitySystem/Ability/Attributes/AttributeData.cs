using System;
using System.Collections.Generic;

namespace AbilitySystem.Ability.Attributes
{
    public class AttributeData
    {
        public float BaseValue;
        public float CurrentValue;

        public event Action<float> OnBaseValueChangedEvent = delegate { };
        public event Action<float> OnCurrentValueChangedEvent = delegate { };

        public readonly List<AttributeModifier> Modifiers = new List<AttributeModifier>();

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