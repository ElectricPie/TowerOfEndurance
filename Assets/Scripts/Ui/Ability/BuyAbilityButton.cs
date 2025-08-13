using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Ui.Ability
{
    public class BuyAbilityButton : MonoBehaviour
    {
        [SerializeField] private BuyAbilityScriptableObject m_ability;
        [SerializeField, RequiredIn(PrefabKind.PrefabInstance)] private TMP_Text m_abilityNameText;
        
        private TowerAbilities m_towerAbilities;
        private PlayerMoney m_playerMoney;

        protected void Awake()
        {
            m_towerAbilities = FindFirstObjectByType<TowerAbilities>();
            m_playerMoney = FindFirstObjectByType<PlayerMoney>();

            m_abilityNameText.text = m_ability.AbilityScriptableObject.Label;
        }

        public void TryBuyAbility()
        {
            if (m_playerMoney.RemoveMoney(m_ability.Cost))
            {
                m_towerAbilities.AddAbility(m_ability.AbilityScriptableObject);
                Destroy(gameObject);
            }
        }

        protected void OnValidate()
        {
            if (m_ability == null)
            {
                m_abilityNameText.text = "Unassigned Ability";
                return;
            }

            m_abilityNameText.text = m_ability.AbilityScriptableObject.Label;
        }
    }
}