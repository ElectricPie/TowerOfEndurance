using TMPro;
using UnityEngine;

namespace Ui.Ability
{
    public class BuyAbilityButton : MonoBehaviour
    {
        [SerializeField] private BuyAbilityScriptableObject m_ability;
        [SerializeField] private TMP_Text m_abilityNameText;
        
        private TowerAbilities m_towerAbilities;
        private PlayerMoney m_playerMoney;

        protected void Awake()
        {
            m_towerAbilities = FindFirstObjectByType<TowerAbilities>();
            m_playerMoney = FindFirstObjectByType<PlayerMoney>();

            m_abilityNameText.text = m_ability.AbilityData.Label;
        }

        public void TryBuyAbility()
        {
            if (m_playerMoney.RemoveMoney(m_ability.Cost))
            {
                m_towerAbilities.AddOnHitAbility(m_ability.AbilityData);
                Destroy(gameObject);
            }
        }
    }
    
    [CreateAssetMenu(fileName = "New Ability Buy Scriptable", menuName = "Abilities/New Buy Scriptable")]
    public class BuyAbilityScriptableObject : ScriptableObject
    {
        [SerializeField] private AbilityData m_abilityData;
        [SerializeField] private int m_cost = 20;

        public AbilityData AbilityData => m_abilityData;
        public int Cost => m_cost;
    }
}