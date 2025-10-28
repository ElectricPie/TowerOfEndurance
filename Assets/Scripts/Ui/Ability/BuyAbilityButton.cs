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
        
        public BuyAbilityScriptableObject Ability => m_ability;

        protected void Start()
        {
            m_abilityNameText.text = m_ability.AbilityScriptableObject.Label;
        }

        protected void Awake()
        {
            m_abilityWidgetController = Hud.GameHudController.Instance.AbilityWidgetController;
        }
        
        public void Initialize(BuyAbilityScriptableObject ability)
        {
            m_ability = ability;
            m_abilityNameText.text = m_ability.AbilityScriptableObject.Label;
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