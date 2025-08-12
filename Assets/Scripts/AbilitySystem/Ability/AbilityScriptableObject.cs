using UnityEngine;

namespace AbilitySystem.Ability
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "Abilities/New Ability")]
    public class AbilityScriptableObject : ScriptableObject
    {
        [SerializeField]
        private string m_label;
    
        [SerializeReference] private AbilityData m_abilityData;

        public string Label => m_label;
        public AbilityData AbilityData => m_abilityData;
    
        protected void OnEnable()
        {
            if (string.IsNullOrEmpty(m_label))
            {
                m_label = name;
            }
        }
    }

}