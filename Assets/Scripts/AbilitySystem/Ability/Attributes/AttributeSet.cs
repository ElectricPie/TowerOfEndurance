using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem.Ability.Attributes
{
    public class AttributeData
    {
        public float Value = 0;

        public AttributeData(AttributeConfig config)
        {
            Value = config.InitialValue;
        }
    }
    
    public class AttributeSet : MonoBehaviour
    {
        [SerializeField] private List<AttributeSetScriptableObject> m_attributeLists;

        private readonly Dictionary<string, AttributeData> m_attributes = new Dictionary<string, AttributeData>();
        
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
                    
                    AttributeData newAttributeData = new AttributeData(attribute);
                    m_attributes.Add(attribute.Name, newAttributeData);
                }
            }
        }
    }
}