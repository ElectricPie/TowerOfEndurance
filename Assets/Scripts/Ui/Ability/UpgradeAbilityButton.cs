using AbilitySystem.Ability;
using TMPro;
using Ui.Tooltip.Ability;
using Ui.WidgetControllers;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Ability
{
    public class UpgradeAbilityButton : MonoBehaviour, IAbilityTooltipInterface
    {
        [SerializeField] private TMP_Text m_abilityNameText;
        [SerializeField] private Button m_button;
        
        private AbilityWidgetController m_abilityWidgetController;
        private AbilityInstanceOld m_ability;
        
        protected void Awake()
        {
            m_abilityWidgetController = Hud.GameHudController.Instance.AbilityWidgetController;
            
            m_button.onClick.AddListener(OnClicked);
        }
        
        public void Initialize(AbilityInstanceOld abilityInstanceOld)
        {
            m_ability = abilityInstanceOld;
            m_abilityNameText.text = m_ability.AbilityScriptableObject.Label;
        }

        public void OnClicked()
        {
            bool upgradedAbility = m_abilityWidgetController.TryUpgradeAbility(m_ability);
            if (upgradedAbility && m_ability.Level >= m_ability.AbilityScriptableObject.MaxLevel)
            {
                m_button.interactable = false;
            }
        }

        public AbilityScriptableObject GetAbilityData()
        {
            return m_ability.AbilityScriptableObject;
        }

        public float GetAbilityLevel()
        {
            return m_ability.Level < m_ability.AbilityScriptableObject.MaxLevel ? m_ability.Level + 1 : m_ability.AbilityScriptableObject.MaxLevel;
        }
    }
}