using AbilitySystem.Ability.Attributes;
using UnityEngine;

namespace AbilitySystem.Ability.AttributeSets
{
    public class UnitAttributeSet : AttributeSet
    {
        [SerializeField] private AttributeIdScriptableObject m_incomingDamageAttributeId;
        [SerializeField] private AttributeIdScriptableObject m_healthAttributeId;

        protected override void AttributeValueChanged(AttributeIdScriptableObject attributeId, AttributeData attribute)
        {
            base.AttributeValueChanged(attributeId, attribute);

            if (attributeId == m_incomingDamageAttributeId)
            {
                AttributeData healthAttribute = GetAttribute(m_healthAttributeId);
                healthAttribute.SetCurrentValue(healthAttribute.CurrentValue - attribute.CurrentValue);
                attribute.SetCurrentValue(0, false);;
            }
        }
    }
}