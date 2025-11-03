using System;
using System.Collections.Generic;

namespace AbilitySystem.Ability.Attributes
{
    public class AttributeData
    {
        public float BaseValue;
        public float CurrentValue;

        public event Action<float> OnValueChangedEvent = delegate { };

        public readonly List<AttributeModifier> Modifiers = new List<AttributeModifier>();
    }
}