using System;
using AbilitySystem.Ability.Attributes;
using AbilitySystem.Ability.AttributeSets;
using Character;
using UnityEngine;

public class Unit : MonoBehaviour
{
    /// <summary>
    /// The object that was killed
    /// </summary>
    public event Action<GameObject> OnKilledEvent = delegate { };

    [SerializeField] private AttributeIdScriptableObject m_healthAttributeId;
    [SerializeField] private AttributeIdScriptableObject m_maxHealthAttributeId;
    
    public AttributeSet AttributeSet { get; private set; }
    public UnitLiveCost LivesCostComponent { get; private set; }
    public UnitMoney MoneyComponent { get; private set; }
    public EffectsContainer EffectsContainer { get; private set; }
    public CharacterPackAppearance CharacterAppearance { get; private set; }
    public bool IsDead { get; private set; }
    
    
    private void Awake()
    {
        AttributeSet = GetComponent<AttributeSet>();
        LivesCostComponent = GetComponent<UnitLiveCost>();
        MoneyComponent = GetComponent<UnitMoney>();
        EffectsContainer = GetComponent<EffectsContainer>();
        CharacterAppearance = GetComponentInChildren<CharacterPackAppearance>();
    }

    public void SetMaxHealth(float maxHealth)
    {
        AttributeSet.SetBaseValue(m_maxHealthAttributeId, maxHealth);
    }
    
    protected void Start()
    {
        AttributeSet.GetAttribute(m_healthAttributeId).OnCurrentValueChangedEvent += newValue =>
        {
            if (newValue <= 0 && !IsDead)
            {
                IsDead = true;
                OnKilledEvent.Invoke(gameObject);
                Destroy(gameObject);
            }
        };
    }
}
