using EditorAttributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AbilitySystem.Ability
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "Abilities/New Ability")]
    public class AbilityScriptableObject : ScriptableObject
    {
        [SerializeField] private string m_label = "New Ability";
        [SerializeField, TooltipTextArea] private string m_description = "No Description";
        [SerializeField] private AnimationCurve m_cost = AnimationCurve.Linear(1, 10, 10, 100);
        [SerializeField] private AbilityTrigger m_trigger = AbilityTrigger.OnBasicAttackFired;
        [SerializeField, ShowIf("m_trigger", AbilityTrigger.Timed)]
        private AnimationCurve m_triggerTime;
        [SerializeField] private int m_maxLevel = 5;

        [SerializeReference] private AbilityData m_abilityData;
        
        public string Label => m_label;
        public string Description => m_description;
        public AbilityTrigger Trigger => m_trigger;
        public int MaxLevel => m_maxLevel;
        public AbilityData AbilityData => m_abilityData;
        
        public float GetCostAt(int level) => m_cost.Evaluate(level);
        public float GetTriggerTimeAt(int level) => m_triggerTime.Evaluate(level);
        
        [Header("Old Values")]
        [SerializeReference] private AbilityDataOld m_abilityDataOld;

        // public string Label => m_abilityData.Label;
        public AbilityDataOld AbilityDataOld => m_abilityDataOld;
    }
}