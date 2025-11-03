using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem.Ability.Attributes
{
    [CreateAssetMenu(fileName = "New Attribute Set", menuName = "Ability System/Attribute/Attribute Set", order = 0)]
    public class AttributeSetScriptableObject : ScriptableObject
    {
        [SerializeField] private List<AttributeConfig> m_attributes;

        public List<AttributeConfig> Attributes => m_attributes;
    }
}