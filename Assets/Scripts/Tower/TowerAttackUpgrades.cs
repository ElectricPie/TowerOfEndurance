using System;
using AbilitySystem.Ability.Attributes;
using UnityEngine;

public class TowerAttackUpgrades : MonoBehaviour
{
    public event Action<float> OnDamageCostChanged = delegate { };
    public event Action<float> OnSpeedCostChanged = delegate { };
    
    [SerializeField] private PlayerMoney m_playerMoney;
    [SerializeField] private TowerAttributeSet m_attributeSet;
    
    [SerializeField] private float m_costMultiplier = 1.15f;
    [SerializeField] private float m_upgradeInitialCost = 5.0f;

    public float DamageUpgradeCost => CalculateUpgradeCost(m_attributeSet.DamageLevel);
    public float SpeedUpgradeCost => CalculateUpgradeCost(m_attributeSet.FireRateLevel);

    private void Awake()
    {
        if (m_playerMoney == null)
            throw new Exception($"TowerAttackUpgrade on {name} is missing reference to PlayerMoney script");
    }

    private void Start()
    {
        BroadcastDamageValues(m_upgradeInitialCost);
        BroadcastFireRateValues(m_upgradeInitialCost);
    }
    
    private float CalculateUpgradeCost(int currentLevel)
    {
        return Mathf.Ceil(m_upgradeInitialCost * Mathf.Pow(m_costMultiplier, currentLevel - 1));
    }

    public void UpgradeDamage()
    {
        // Cost is rounded up to remove any decimals and to ensure the cost always goes up
        float upgradeCost = CalculateUpgradeCost(m_attributeSet.DamageLevel);;
        
        if (!m_playerMoney.RemoveMoney(upgradeCost) && UIErrorMessage.Instance != null)
        {
            UIErrorMessage.Instance.ShowError("Insignificant money for upgrade");
            return;
        }

        m_attributeSet.IncreaseDamageLevel();
        
        float nextUpgradeCost = CalculateUpgradeCost(m_attributeSet.DamageLevel);
        BroadcastDamageValues(nextUpgradeCost);
    }

    public void UpgradeSpeed()
    {
        // Cost is rounded up to remove any decimals and to ensure the cost always goes up
        float upgradeCost = CalculateUpgradeCost(m_attributeSet.FireRateLevel);
        
        if (!m_playerMoney.RemoveMoney(upgradeCost) && UIErrorMessage.Instance != null)
        {
            UIErrorMessage.Instance.ShowError("Insignificant money for upgrade");
            return;
        }

        m_attributeSet.IncreaseFireRateLevel();
        
        float nextUpgradeCost = CalculateUpgradeCost(m_attributeSet.FireRateLevel);
        BroadcastFireRateValues(nextUpgradeCost);
    }

    private void BroadcastDamageValues(float upgradeCost)
    {
        // UpgradeChangeMessage damageUpgradeMessage = new UpgradeChangeMessage(
        //     upgradeCost,
        //     m_attributeSet.Damage, 
        //     m_attributeSet.DamageAt(m_attributeSet.DamageLevel + 1)
        //     );
        OnDamageCostChanged.Invoke(upgradeCost);
    }

    private void BroadcastFireRateValues(float upgradeCost)
    {
        // UpgradeChangeMessage speedUpgradeMessage = new UpgradeChangeMessage(
        //     upgradeCost,
        //     m_attributeSet.FireRate, 
        //     m_attributeSet.FireRateAt(m_attributeSet.FireRateLevel + 1)
        //     );
        OnSpeedCostChanged.Invoke(upgradeCost);
    }
}
