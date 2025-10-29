using System;
using Ui.WidgetControllers;
using UnityEngine;

namespace Ui.Ability
{
    public class RandomAbilityButton : MonoBehaviour
    {
        [SerializeField] private float m_randomAbilityCost = 5.0f;
        
        private AbilityWidgetController m_abilityWidgetController;

        private void Start()
        {
            m_abilityWidgetController = Hud.GameHudController.Instance.AbilityWidgetController;
        }

        public void OnClicked()
        {
            m_abilityWidgetController.TryBuyRandomAbility(m_randomAbilityCost);
        }
    }
}