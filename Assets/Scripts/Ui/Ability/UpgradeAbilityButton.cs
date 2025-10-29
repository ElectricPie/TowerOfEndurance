using TMPro;
using Ui.WidgetControllers;
using UnityEngine;

namespace Ui.Ability
{
    public class UpgradeAbilityButton : MonoBehaviour
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
            // TODO: Need to store name in ability
            m_abilityNameText.text = "TODO";
        }
    }
}