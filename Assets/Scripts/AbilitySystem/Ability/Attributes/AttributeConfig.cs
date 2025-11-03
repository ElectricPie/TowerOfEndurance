using System;
using UnityEngine;

namespace AbilitySystem.Ability.Attributes
{
    [Serializable]
    public class AttributeConfig
    {
        [SerializeField] private AttributeIdScriptableObject m_id;
        [SerializeField] private int m_initialValue;

        public AttributeIdScriptableObject ID => m_id;
        public int InitialValue => m_initialValue;
    }
}