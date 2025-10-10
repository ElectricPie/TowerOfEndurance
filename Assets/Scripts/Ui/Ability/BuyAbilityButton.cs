using Sirenix.OdinInspector;
using TMPro;
using Ui.WidgetControllers;
using UnityEngine;

namespace Ui.Ability
{
    public class BuyAbilityButton : MonoBehaviour
    {
        [SerializeField] private BuyAbilityScriptableObject m_ability;
        [SerializeField, RequiredIn(PrefabKind.PrefabInstance)] private TMP_Text m_abilityNameText;
        
        private AbilityWidgetController m_abilityWidgetController;
        
        protected void Awake()
        {
            m_abilityNameText.text = m_ability.AbilityScriptableObject.Label;
            
            m_abilityWidgetController = Hud.HudController.Instance.AbilityWidgetController;
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

            m_abilityNameText.text = m_ability.AbilityScriptableObject.Label;
        }
    }
}