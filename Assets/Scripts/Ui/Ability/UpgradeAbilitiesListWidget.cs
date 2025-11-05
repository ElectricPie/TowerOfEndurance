using Ui.WidgetControllers;
using UnityEngine;

namespace Ui.Ability
{
    public class UpgradeAbilitiesListWidget : MonoBehaviour
    {
        [SerializeField] private Transform m_upgradeAbilitiesListContent;
        [SerializeField] private UpgradeAbilityButton m_abilityUpgradeButtonPrefab;
        
        private AbilityWidgetController m_abilityWidgetController;

        private void Start()
        {
            m_abilityWidgetController = Hud.GameHudController.Instance.AbilityWidgetController;
            m_abilityWidgetController.OnAbilityPurchasedEvent += OnAbilityPurchased;
        }

        private void OnAbilityPurchased(AbilityInstanceOld newAbility)
        {
            UpgradeAbilityButton newButton = Instantiate(m_abilityUpgradeButtonPrefab, m_upgradeAbilitiesListContent);
            newButton.Initialize(newAbility);
        }
    }
}