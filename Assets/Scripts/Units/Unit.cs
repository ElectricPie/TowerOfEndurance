using System;
using AbilitySystem.Ability.Attributes;
using Character;
using UnityEngine;

[RequireComponent(typeof(AttributeSet)), RequireComponent(typeof(UnitLiveCost)), RequireComponent(typeof(UnitMoney)), 
 RequireComponent(typeof(EffectsContainer)), RequireComponent(typeof(CharacterPackAppearance))]
public class Unit : MonoBehaviour
{
    /// <summary>
    /// The object that was killed
    /// </summary>
    public event Action<GameObject> OnKilledEvent = delegate { };

    [SerializeField] private AttributeIdScriptableObject m_healthAttributeId;
    
    public AttributeSet AttributeSet { get; private set; }
    public UnitLiveCost LivesCostComponent { get; private set; }
    public UnitMoney MoneyComponent { get; private set; }
    public EffectsContainer EffectsContainer { get; private set; }
    public CharacterPackAppearance CharacterAppearance { get; private set; }
    
    
    private void Awake()
    {
        AttributeSet = GetComponent<AttributeSet>();
        LivesCostComponent = GetComponent<UnitLiveCost>();
        MoneyComponent = GetComponent<UnitMoney>();
        EffectsContainer = GetComponent<EffectsContainer>();
        CharacterAppearance = GetComponentInChildren<CharacterPackAppearance>();
    }
    
    protected void Start()
    {
        AttributeSet.GetAttribute(m_healthAttributeId).OnCurrentValueChangedEvent += newValue =>
        {
            if (newValue <= 0)
            {
                OnKilledEvent.Invoke(gameObject);
                Destroy(gameObject);
            }
        };
    }
}
