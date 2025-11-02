using System.Collections.Generic;

namespace AbilitySystem.Ability.Attributes
{
    public class AttributeData
    {
        public float BaseValue;
        public float CurrentValue;

        public readonly List<AttributeModifier> Modifiers = new List<AttributeModifier>();
    }
}