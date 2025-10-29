using TMPro;
using Ui.Tooltip.Ability;
using Ui.WidgetControllers;
using UnityEngine;

namespace Ui.Ability
{
    public class UpgradeAbilityButton : MonoBehaviour, IAbilityTooltipInterface
    {
        [SerializeField] private TMP_Text m_abilityNameText;
        
        private AbilityWidgetController m_abilityWidgetController;
        private AbilityInstance m_ability;
        
        protected void Awake()
        {
            m_abilityWidgetController = Hud.GameHudController.Instance.AbilityWidgetController;
        }
        
        public void Initialize(AbilityInstance abilityInstance)
        {
            m_ability = abilityInstance;
            m_abilityNameText.text = m_ability.AbilityData.Label;
        }

        public AbilityData GetAbilityData()
        {
            return m_ability.AbilityData;
        }
    }
}