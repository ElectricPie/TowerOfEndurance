using Ui.WidgetControllers;
using UnityEngine;

namespace Ui.TowerUpgrade
{
    public class TowerStaticUpgradeWidget : Widget
    {
        [SerializeField] private TowerUpgradeButton m_damageUpgradeButton;
        [SerializeField] private TowerUpgradeButton m_speedUpgradeButton;

        private TowerUpgradeWidgetController m_towerUpgradeWidgetController;
        
        private void Awake()
        {
            m_damageUpgradeButton.OnClickedEvent += () =>
            {
                m_towerUpgradeWidgetController.UpgradeDamage();
            };
            m_speedUpgradeButton.OnClickedEvent += () =>
            {
                m_towerUpgradeWidgetController.UpgradeSpeed();
            };
            
            m_towerUpgradeWidgetController = Hud.GameHudController.Instance.TowerUpgradeWidgetController;
            m_towerUpgradeWidgetController.OnDamageUpgradeCostChanged += newCost =>
            {
                m_damageUpgradeButton.UpdateCost(newCost);
            };
            m_towerUpgradeWidgetController.OnSpeedUpgradeCostChanged += newCost =>
            {
                m_speedUpgradeButton.UpdateCost(newCost);
            };
            m_towerUpgradeWidgetController.BroadcastInitialValues();
        }
    }
}