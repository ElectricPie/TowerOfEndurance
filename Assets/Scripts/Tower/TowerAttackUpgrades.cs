using System;
using AbilitySystem.Ability.Attributes;
using AbilitySystem.Ability.AttributeSets;
using AbilitySystem.Effect;
using UnityEngine;

namespace Tower
{
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

        public float DamageUpgradeCost { get; private set; }
        public float SpeedUpgradeCost { get; private set; }

        private void Awake()
        {
            if (m_playerMoney == null)
                throw new Exception($"TowerAttackUpgrade on {name} is missing reference to PlayerMoney script");
        }

        private void Start()
        {
            // Damage setup
            m_towerAttributeSet.GetAttribute(m_damageLevelAttributeId).OnCurrentValueChangedEvent += newValue =>
            {
                DamageUpgradeCost = CalculateUpgradeCost(Mathf.CeilToInt(newValue));
                OnDamageCostChanged.Invoke(DamageUpgradeCost);
            };
            DamageUpgradeCost = m_towerAttributeSet.GetAttributeValue(m_damageLevelAttributeId);
            OnDamageCostChanged.Invoke(DamageUpgradeCost);

            // Fire rate setup
            m_towerAttributeSet.GetAttribute(m_fireRateLevelAttributeId).OnCurrentValueChangedEvent += newValue =>
            {
                SpeedUpgradeCost = CalculateUpgradeCost(Mathf.CeilToInt(newValue));
                OnSpeedCostChanged.Invoke(SpeedUpgradeCost);
            };
            SpeedUpgradeCost = m_towerAttributeSet.GetAttributeValue(m_fireRateLevelAttributeId);
            OnSpeedCostChanged.Invoke(SpeedUpgradeCost);
        }

        private float CalculateUpgradeCost(int currentLevel)
        {
            return Mathf.Ceil(m_upgradeInitialCost * Mathf.Pow(m_costMultiplier, currentLevel - 1));
        }

        public void UpgradeDamage()
        {
            // Cost is rounded up to remove any decimals and to ensure the cost always goes up
            if (!m_playerMoney.RemoveMoney(DamageUpgradeCost) && UIErrorMessage.Instance != null)
            {
                UIErrorMessage.Instance.ShowError("Insignificant money for upgrade");
                return;
            }

            m_towerEffectContainer.ApplyEffect(gameObject, m_damageUpgradeEffect, 1);
        }

        public void UpgradeSpeed()
        {
            // Cost is rounded up to remove any decimals and to ensure the cost always goes up
            if (!m_playerMoney.RemoveMoney(SpeedUpgradeCost) && UIErrorMessage.Instance != null)
            {
                UIErrorMessage.Instance.ShowError("Insignificant money for upgrade");
                return;
            }

            m_towerEffectContainer.ApplyEffect(gameObject, m_fireRateUpgradeEffect, 1);
        }
    }
}