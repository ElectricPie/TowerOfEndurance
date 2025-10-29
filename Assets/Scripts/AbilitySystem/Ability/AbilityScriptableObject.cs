using UnityEngine;

namespace AbilitySystem.Ability
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "Abilities/New Ability")]
    public class AbilityScriptableObject : ScriptableObject
    {
        [SerializeReference] private AbilityData m_abilityData;

        public string Label => m_abilityData.Label;
        public AbilityData AbilityData => m_abilityData;
    }
}