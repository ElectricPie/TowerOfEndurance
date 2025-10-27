using AbilitySystem.Ability;
using UnityEngine;

namespace Ui.Ability
{
    [CreateAssetMenu(fileName = "New Ability Buy Scriptable", menuName = "Abilities/New Buy Scriptable")]
    public class BuyAbilityScriptableObject : ScriptableObject
    {
        [SerializeField] private AbilityScriptableObject m_abilityScriptableObject;
        [SerializeField] private int m_cost = 20;
        [SerializeField, TextArea] private string m_description = "No Description";

        public AbilityScriptableObject AbilityScriptableObject => m_abilityScriptableObject;
        public int Cost => m_cost;
        public string Description => m_description;
    }
}