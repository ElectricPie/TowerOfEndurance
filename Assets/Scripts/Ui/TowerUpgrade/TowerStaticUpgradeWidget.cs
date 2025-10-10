using Ui.WidgetControllers;
using UnityEngine;

namespace Ui.TowerUpgrade
{
    public class TowerStaticUpgradeWidget : Widget
    {
        [SerializeField] private TowerUpgradeButton m_damageUpgradeButton;
        [SerializeField] private TowerUpgradeButton m_speedUpgradeButton;

        private TowerUpgradeWidgetController m_towerUpgradeWidgetController;

        public void UpgradeDamage()
        {
            m_towerUpgradeWidgetController.UpgradeDamage();
        }
        
        public void UpgradeSpeed()
        {
            m_towerUpgradeWidgetController.UpgradeSpeed(); 
        }
        
        private void Awake()
        {
            m_damageUpgradeButton.OnClickedEvent += UpgradeDamage;
            m_speedUpgradeButton.OnClickedEvent += UpgradeSpeed;
            
            m_towerUpgradeWidgetController = Hud.HudController.Instance.TowerUpgradeWidgetController;
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