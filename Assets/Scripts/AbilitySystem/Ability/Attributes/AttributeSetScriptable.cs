using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem.Ability.Attributes
{
    [CreateAssetMenu(fileName = "New Attribute Set", menuName = "Ability System/Attribute Set", order = 0)]
    public class AttributeSetScriptableObject : ScriptableObject
    {
        [SerializeField] private List<AttributeConfig> m_attributes;

        public List<AttributeConfig> Attributes => m_attributes;

        // TODO: Initialise attribute set on object with a new class called AttributeSetInstance
        //   AttributeSetInstance is initialised with the values from this scriptable object
        //   GameEffects can then just take a string value to use for looking up attributes
    }
}