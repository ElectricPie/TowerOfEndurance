using UnityEngine;

namespace AbilitySystem.Ability.Attributes
{
    [CreateAssetMenu(fileName = "New Attribute Set", menuName = "Ability System/Attribute/Attribute Id", order = 0)]
    public class AttributeIdScriptableObject : ScriptableObject
    {
        [SerializeField] private string m_name;
        public string Name => m_name;
    }
}