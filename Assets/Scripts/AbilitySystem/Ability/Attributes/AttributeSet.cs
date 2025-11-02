using System;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem.Ability.Attributes
{
    public class AttributeSet : MonoBehaviour
    {
        [SerializeField] private List<AttributeSetScriptableObject> m_attributeLists;

        private readonly Dictionary<string, AttributeData> m_attributes = new Dictionary<string, AttributeData>();

        public AttributeData GetAttribute(string attributeName)
        {
            m_attributes.TryGetValue(attributeName, out AttributeData attribute);
            return attribute;
        }

        public void AddInstantModifier(AttributeModifier mod)
        {
            AttributeData attribute = GetAttribute(mod.AttributeName);
            if (attribute == null)
                return;

            switch (mod.Operation)
            {
                case ModifierOperation.Add:
                    attribute.CurrentValue += mod.Magnitude.Value();
                    break;
                case ModifierOperation.Override:
                    attribute.CurrentValue = mod.Magnitude.Value();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddPersistentModifier(AttributeModifier mod)
        {
            AttributeData attribute = GetAttribute(mod.AttributeName);
            if (attribute == null)
                return;
            
            attribute.Modifiers.Add(mod);
            RecalculateAttribute(attribute);
        }

        private static void RecalculateAttribute(AttributeData attribute)
        {
            float baseValue = attribute.BaseValue;
            float addSum = 0.0f;
            float? overrideValue = null;

            foreach (AttributeModifier mod in attribute.Modifiers)
            {
                switch (mod.Operation)
                {
                    case ModifierOperation.Add:
                        addSum += mod.Magnitude.Value();
                        break;
                    case ModifierOperation.Override:
                        overrideValue = mod.Magnitude.Value();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            float newValue = baseValue + addSum;
            attribute.CurrentValue = overrideValue ?? newValue;
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
                    if (m_attributes.ContainsKey(attribute.Name))
                    {
                        Debug.LogWarning($"Attempting to add duplicate \"{attribute.Name}\" attribute on {gameObject}", this);
                        continue;
                    }
                    
                    AttributeData newAttributeData = new AttributeData();
                    m_attributes.Add(attribute.Name, newAttributeData);
                }
            }
        }
    }
}