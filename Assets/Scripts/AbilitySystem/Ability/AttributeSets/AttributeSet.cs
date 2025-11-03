using System;
using System.Collections.Generic;
using AbilitySystem.Ability.Attributes;
using UnityEngine;

namespace AbilitySystem.Ability.AttributeSets
{
    public class AttributeSet : MonoBehaviour
    {
        [SerializeField] private List<AttributeSetScriptableObject> m_attributeLists;

        private readonly Dictionary<AttributeIdScriptableObject, AttributeData> m_attributes = new Dictionary<AttributeIdScriptableObject, AttributeData>();

        public AttributeData GetAttribute(AttributeIdScriptableObject attributeId)
        {
            m_attributes.TryGetValue(attributeId, out AttributeData attribute);
            return attribute;
        }

        public float GetAttributeValue(AttributeIdScriptableObject attributeId)
        {
            AttributeData attribute = GetAttribute(attributeId);
            if (attribute == null)
                return 0.0f;
            
            return attribute.CurrentValue;
        } 
        
        public void AddInstantModifier(AttributeModifier mod)
        {
            AttributeData attribute = GetAttribute(mod.AttributeId);
            if (attribute == null)
                return;

            switch (mod.Operation)
            {
                case ModifierOperation.Add:
                    attribute.SetCurrentValue(attribute.CurrentValue + CalculateModMagnitude(mod.Magnitude), false);
                    break;
                case ModifierOperation.Override:
                    attribute.SetCurrentValue(CalculateModMagnitude(mod.Magnitude), false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            attribute.BroadcastCurrentValue();
            AttributeValueChanged(mod.AttributeId, attribute);
        }

        public void AddPersistentModifier(AttributeModifier mod)
        {
            AttributeData attribute = GetAttribute(mod.AttributeId);
            if (attribute == null)
                return;
            
            if (mod.Magnitude.CalculationType == CalculationType.AttributeBacked)
            {
                AttributeData backingAttribute = GetAttribute(mod.Magnitude.AttributeBackedMagnitude.BackingAttributeId);
                backingAttribute.OnCurrentValueChangedEvent += _ =>
                {
                    RecalculateAttribute(attribute);
                    AttributeValueChanged(mod.AttributeId, attribute);
                };
            }
            
            attribute.Modifiers.Add(mod);
            RecalculateAttribute(attribute);
            AttributeValueChanged(mod.AttributeId, attribute);
        }
        
        
        protected virtual void AttributeValueChanged(AttributeIdScriptableObject attributeId, AttributeData attribute) {}
        
            
        private void RecalculateAttribute(AttributeData attribute)
        {
            float baseValue = attribute.BaseValue;
            float addSum = 0.0f;
            float? overrideValue = null;

            foreach (AttributeModifier mod in attribute.Modifiers)
            {
                switch (mod.Operation)
                {
                    case ModifierOperation.Add:
                        addSum += CalculateModMagnitude(mod.Magnitude);
                        break;
                    case ModifierOperation.Override:
                        overrideValue = CalculateModMagnitude(mod.Magnitude);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            float newValue = baseValue + addSum;
            attribute.BaseValue = overrideValue ?? newValue;
            attribute.SetCurrentValue(attribute.BaseValue);
            
            attribute.BroadcastBaseValue();
            attribute.BroadcastCurrentValue();
        }

        private float CalculateModMagnitude(ModifierMagnitude modMagnitude)
        {
            switch (modMagnitude.CalculationType)
            {
                case CalculationType.Float:
                    return modMagnitude.FlatValue;
                case CalculationType.AttributeBacked:
                    AttributeBackedMagnitude backedMagnitude = modMagnitude.AttributeBackedMagnitude;
                    AttributeIdScriptableObject backingAttributeName = backedMagnitude.BackingAttributeId;
                    AttributeData backingAttribute = GetAttribute(backingAttributeName);
                    if (backingAttribute == null)
                    {
                        Debug.LogWarning($"Attempted to use backing attribute {backingAttributeName} on {gameObject.name}", this);
                        return 0;
                    }

                    float backingAttributeValue = backingAttribute.CurrentValue;
                    
                    float value = backingAttributeValue * CalculateCurveFloat(backedMagnitude.Coefficient, backingAttributeValue);
                    value += CalculateCurveFloat(backedMagnitude.PostAdditiveValue, backingAttributeValue);
                    return value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private float CalculateCurveFloat(CurveFloat curveFloat, float level = 1)
        {
            return curveFloat.UseCurve ? curveFloat.Curve.Evaluate(level) : curveFloat.FlatFloat;
        }
        
        protected void Awake()
        {
            InitializeAttributes();
        }

        private void InitializeAttributes()
        {
            foreach (AttributeSetScriptableObject attributeList in m_attributeLists)
            {
                foreach (AttributeConfig attribute in attributeList.Attributes)
                {
                    if (m_attributes.ContainsKey(attribute.ID))
                    {
                        Debug.LogWarning($"Attempting to add duplicate \"{attribute.ID}\" attribute on {gameObject}", this);
                        continue;
                    }
                    
                    AttributeData newAttributeData = new AttributeData();
                    m_attributes.Add(attribute.ID, newAttributeData);
                }
            }
        }
    }
}