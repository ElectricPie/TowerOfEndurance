using System;
using Tower;

namespace Ui.WidgetControllers
{
    public class TowerUpgradeWidgetController : WidgetController
    {
        public event Action<float> OnDamageUpgradeCostChanged = delegate { };
        public event Action<float> OnSpeedUpgradeCostChanged = delegate { };
        
        private TowerAttackUpgrades m_towerAttackUpgrades;
        
        public override void BindCallbacksToDependencies()
        {
            m_towerAttackUpgrades = UnityEngine.Object.FindFirstObjectByType<TowerAttackUpgrades>();
            m_towerAttackUpgrades.OnDamageCostChanged += newCost =>
            {
                OnDamageUpgradeCostChanged(newCost);
            };
            m_towerAttackUpgrades.OnSpeedCostChanged += newCost =>
            {
                OnSpeedUpgradeCostChanged(newCost);
            };
        }

        public override void BroadcastInitialValues()
        {
            TowerAttackUpgrades towerAttackUpgrades = UnityEngine.Object.FindFirstObjectByType<TowerAttackUpgrades>();
            OnDamageUpgradeCostChanged(towerAttackUpgrades.DamageUpgradeCost);
            OnSpeedUpgradeCostChanged(towerAttackUpgrades.SpeedUpgradeCost);
        }
        
        public void UpgradeDamage()
        {
            m_towerAttackUpgrades.UpgradeDamage();
        }
        
        public void UpgradeSpeed()
        {
            m_towerAttackUpgrades.UpgradeSpeed();
        }
    }
}