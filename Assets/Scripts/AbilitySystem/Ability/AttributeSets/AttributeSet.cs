using System;
using System.Collections.Generic;
using AbilitySystem.Ability.Attributes;
using AbilitySystem.Effect.EffectProperties;
using Sirenix.OdinInspector;
using UnityEngine;
using Waves;

namespace AbilitySystem.Ability.AttributeSets
{
    public class AttributeSet : MonoBehaviour
    {
        [SerializeField] private List<AttributeSetScriptableObject> m_attributeLists;

        private readonly Dictionary<AttributeIdScriptableObject, AttributeData> m_attributes =
            new Dictionary<AttributeIdScriptableObject, AttributeData>();

        [ShowInInspector, ReadOnly, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        private Dictionary<AttributeIdScriptableObject, AttributeData> DebugAttributes => m_attributes;

        private readonly Dictionary<AttributeModifierInstance, (AttributeData Attribute, Action<float> Handler)>
            m_backingAttributeSubscriptions =
                new Dictionary<AttributeModifierInstance, (AttributeData Attribute, Action<float> Handler)>();

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

        public void SetBaseValue(AttributeIdScriptableObject attributeId, float value)
        {
            AttributeData attribute = GetAttribute(attributeId);
            if (attribute == null)
                return;

            attribute.BaseValue = value;
            RecalculateAttribute(attribute);
            AttributeValueChanged(attributeId, attribute);
        }

        public void AddInstantModifier(AttributeModifierInstance mod)
        {
            AttributeData attribute = GetAttribute(mod.Modifier.AttributeId);
            if (attribute == null)
                return;

            switch (mod.Modifier.Operation)
            {
                case ModifierOperation.Add:
                    attribute.BaseValue += CalculateModMagnitude(mod);
                    break;
                case ModifierOperation.Override:
                    attribute.BaseValue = CalculateModMagnitude(mod);
                    break;
                case ModifierOperation.Multiply:
                    attribute.BaseValue *= CalculateModMagnitude(mod);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            RecalculateAttribute(attribute);
            AttributeValueChanged(mod.Modifier.AttributeId, attribute);
        }

        public void AddInfiniteModifier(AttributeModifierInstance mod)
        {
            AttributeData attribute = GetAttribute(mod.Modifier.AttributeId);
            if (attribute == null)
                return;

            if (mod.Modifier.Magnitude.CalculationType == CalculationType.AttributeBacked)
            {
                AttributeData backingAttribute = GetBackingAttribute(mod);
                if (backingAttribute != null)
                {
                    Action<float> handler = _ =>
                    {
                        RecalculateAttribute(attribute);
                        AttributeValueChanged(mod.Modifier.AttributeId, attribute);
                    };
                    backingAttribute.OnCurrentValueChangedEvent += handler;
                    m_backingAttributeSubscriptions[mod] = (backingAttribute, handler);
                }
            }

            attribute.Modifiers.Add(mod);
            RecalculateAttribute(attribute);
            AttributeValueChanged(mod.Modifier.AttributeId, attribute);
        }

        public void RemoveInfiniteModifier(AttributeModifierInstance mod)
        {
            AttributeData attribute = GetAttribute(mod.Modifier.AttributeId);
            if (attribute == null)
                return;

            if (m_backingAttributeSubscriptions.TryGetValue(mod, out var subscription))
            {
                subscription.Attribute.OnCurrentValueChangedEvent -= subscription.Handler;
                m_backingAttributeSubscriptions.Remove(mod);
            }

            attribute.Modifiers.Remove(mod);
            RecalculateAttribute(attribute);
            AttributeValueChanged(mod.Modifier.AttributeId, attribute);
        }


        protected virtual void AttributeValueChanged(AttributeIdScriptableObject attributeId, AttributeData attribute)
        {
        }


        protected void RecalculateAttribute(AttributeData attribute)
        {
            float addSum = 0.0f;
            float multiplyProduct = 1.0f;
            float? overrideValue = null;

            foreach (AttributeModifierInstance mod in attribute.Modifiers)
            {
                switch (mod.Modifier.Operation)
                {
                    case ModifierOperation.Add:
                        addSum += CalculateModMagnitude(mod);
                        break;
                    case ModifierOperation.Override:
                        overrideValue = CalculateModMagnitude(mod);
                        break;
                    case ModifierOperation.Multiply:
                        multiplyProduct *= CalculateModMagnitude(mod);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            float baseForCalculation = overrideValue ?? attribute.BaseValue;
            attribute.SetCurrentValue((baseForCalculation + addSum) * multiplyProduct, false);

            attribute.BroadcastBaseValue();
            attribute.BroadcastCurrentValue();
        }

        private AttributeData GetBackingAttribute(AttributeModifierInstance mod)
        {
            if (m_backingAttributeSubscriptions.TryGetValue(mod, out var subscription))
                return subscription.Attribute;

            AttributeBackedMagnitude backedMagnitude = mod.Modifier.Magnitude.AttributeBackedMagnitude;
            if (backedMagnitude.AttributeSource == AttributeSource.Target)
                return GetAttribute(backedMagnitude.BackingAttributeId);

            return mod.Source != null ? mod.Source.GetAttribute(backedMagnitude.BackingAttributeId) : null;
        }

        private float CalculateModMagnitude(AttributeModifierInstance mod)
        {
            ModifierMagnitude modMagnitude = mod.Modifier.Magnitude;
            int level = mod.Level;

            switch (modMagnitude.CalculationType)
            {
                case CalculationType.ScalableFloat:
                    return modMagnitude.ScalableFloat.GetValue(level);
                case CalculationType.AttributeBacked:
                    AttributeBackedMagnitude backedMagnitude = modMagnitude.AttributeBackedMagnitude;
                    AttributeData backingAttribute = GetBackingAttribute(mod);

                    if (backingAttribute == null)
                    {
                        Debug.LogWarning(
                            $"Attempted to use backing attribute {backedMagnitude.BackingAttributeId} on {gameObject.name}",
                            this);
                        return 0;
                    }

                    float backingAttributeValue = backingAttribute.CurrentValue;

                    switch (backedMagnitude.Mode)
                    {
                        case AttributeBackedMode.Multiply:
                            float value = backingAttributeValue * backedMagnitude.Coefficient.GetValue(level);
                            value += backedMagnitude.PostAdditiveValue.GetValue(level);
                            return value;
                        case AttributeBackedMode.LevelLookup:
                            return backedMagnitude.Coefficient.GetValue(Mathf.RoundToInt(backingAttributeValue));
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void Awake()
        {
            InitializeAttributes();
        }

        protected void OnDestroy()
        {
            foreach (var subscription in m_backingAttributeSubscriptions.Values)
                subscription.Attribute.OnCurrentValueChangedEvent -= subscription.Handler;

            m_backingAttributeSubscriptions.Clear();
        }

        private void InitializeAttributes()
        {
            foreach (AttributeSetScriptableObject attributeList in m_attributeLists)
            {
                foreach (AttributeConfig attribute in attributeList.Attributes)
                {
                    if (m_attributes.ContainsKey(attribute.ID))
                    {
                        Debug.LogWarning($"Attempting to add duplicate \"{attribute.ID}\" attribute on {gameObject}",
                            this);
                        continue;
                    }

                    AttributeData newAttributeData = new AttributeData();
                    newAttributeData.BaseValue = attribute.InitialValue;
                    newAttributeData.SetCurrentValue(attribute.InitialValue);
                    m_attributes.Add(attribute.ID, newAttributeData);
                }
            }
        }
    }
}