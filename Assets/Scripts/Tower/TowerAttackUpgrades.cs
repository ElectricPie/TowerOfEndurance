using System;
using AbilitySystem.Ability.Attributes;
using AbilitySystem.Effect;
using UnityEngine;

public class TowerAttackUpgrades : MonoBehaviour
{
    public event Action<float> OnDamageCostChanged = delegate { };
    public event Action<float> OnSpeedCostChanged = delegate { };
    
    [SerializeField] private PlayerMoney m_playerMoney;
    [SerializeField] private AttributeSet m_towerAttributeSet;
    [SerializeField] private EffectsContainer m_towerEffectContainer;
    
    [SerializeField] private float m_costMultiplier = 1.15f;
    [SerializeField] private float m_upgradeInitialCost = 5.0f;

    [SerializeField] private AttributeIdScriptableObject m_damageLevelAttributeId;
    [SerializeField] private AttributeIdScriptableObject m_fireRateLevelAttributeId;
    
    [SerializeField] private GameEffectScriptableObject m_damageUpgradeEffect;
    [SerializeField] private GameEffectScriptableObject m_fireRateUpgradeEffect;

    private int m_damageLevel = 1;
    
    public float DamageUpgradeCost => CalculateUpgradeCost(m_damageLevel);

    public float SpeedUpgradeCost =>
        CalculateUpgradeCost((int)m_towerAttributeSet.GetAttributeValue(m_fireRateLevelAttributeId));

    private void Awake()
    {
        if (m_playerMoney == null)
            throw new Exception($"TowerAttackUpgrade on {name} is missing reference to PlayerMoney script");
    }

    private void Start()
    {
        m_towerAttributeSet.GetAttribute(m_damageLevelAttributeId).OnCurrentValueChangedEvent += newValue =>
        {
            m_damageLevel = Mathf.FloorToInt(newValue);
        };
        m_towerAttributeSet.GetAttribute(m_fireRateLevelAttributeId).OnCurrentValueChangedEvent += newValue =>
        {
            OnSpeedCostChanged.Invoke(Mathf.FloorToInt(newValue));
        };
        
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
        float upgradeCost = CalculateUpgradeCost(m_damageLevel);
        
        if (!m_playerMoney.RemoveMoney(upgradeCost) && UIErrorMessage.Instance != null)
        {
            UIErrorMessage.Instance.ShowError("Insignificant money for upgrade");
            return;
        }

        m_towerEffectContainer.ApplyEffect(gameObject, m_damageUpgradeEffect);
        
        float nextUpgradeCost = CalculateUpgradeCost(m_damageLevel);
        BroadcastDamageValues(nextUpgradeCost);
    }

    public void UpgradeSpeed()
    {
        // Cost is rounded up to remove any decimals and to ensure the cost always goes up
        int fireRateLevel = (int)m_towerAttributeSet.GetAttributeValue(m_fireRateLevelAttributeId);
        float upgradeCost = CalculateUpgradeCost(fireRateLevel);
        if (!m_playerMoney.RemoveMoney(upgradeCost) && UIErrorMessage.Instance != null)
        {
            UIErrorMessage.Instance.ShowError("Insignificant money for upgrade");
            return;
        }

        m_towerEffectContainer.ApplyEffect(gameObject, m_fireRateUpgradeEffect);
        
        float nextUpgradeCost = CalculateUpgradeCost((int)m_towerAttributeSet.GetAttributeValue(m_fireRateLevelAttributeId));
        BroadcastFireRateValues(nextUpgradeCost);
    }

    private void BroadcastDamageValues(float upgradeCost)
    {
        OnDamageCostChanged.Invoke(upgradeCost);
    }

    private void BroadcastFireRateValues(float upgradeCost)
    {
        OnSpeedCostChanged.Invoke(upgradeCost);
    }
}
