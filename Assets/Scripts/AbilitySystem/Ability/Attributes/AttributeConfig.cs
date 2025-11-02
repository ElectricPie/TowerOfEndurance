using System;
using UnityEngine;

namespace AbilitySystem.Ability.Attributes
{
    [Serializable]
    public class AttributeConfig
    {
        [SerializeField] private string m_name;
        [SerializeField] private int m_initialValue;

        public string Name => m_name;
        public int InitialValue => m_initialValue;
    }
}