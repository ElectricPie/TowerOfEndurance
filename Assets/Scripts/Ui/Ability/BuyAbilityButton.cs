using AbilitySystem.Ability;
using TMPro;
using Ui.Tooltip.Ability;
using Ui.WidgetControllers;
using UnityEngine;

namespace Ui.Ability
{
    public class BuyAbilityButton : MonoBehaviour, IAbilityTooltipInterface
    {
        [SerializeField] private AbilityScriptableObject m_ability;
        [SerializeField] private TMP_Text m_abilityNameText;

        private AbilityWidgetController m_abilityWidgetController;
        
        public AbilityScriptableObject Ability => m_ability;

        protected void Awake()
        {
            m_abilityWidgetController = Hud.GameHudController.Instance.AbilityWidgetController;
        }
        
        public void Initialize(AbilityScriptableObject ability)
        {
            m_ability = ability;
            m_abilityNameText.text = m_ability.Label;
            m_abilityWidgetController.RegisterBuyButton(m_ability, gameObject);
        }

        public void OnClicked()
        {
            m_abilityWidgetController.TryBuyAbility(m_ability, gameObject);
        }

        protected void OnValidate()
        {
            if (m_ability == null)
            {
                m_abilityNameText.text = "Unassigned Ability";
                return;
            }

            m_abilityNameText.text = m_ability.Label;
        }

        public AbilityData GetAbilityData()
        {
            return m_ability.AbilityData;
        }
    }
}
