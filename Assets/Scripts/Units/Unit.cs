using Character;
using UnityEngine;

[RequireComponent(typeof(UnitHealth)), RequireComponent(typeof(UnitLiveCost)), RequireComponent(typeof(UnitMoney)), 
 RequireComponent(typeof(EffectsContainer)), RequireComponent(typeof(CharacterPackAppearance))]
public class Unit : MonoBehaviour
{
    public UnitHealth HealthComponent { get; private set; }
    public UnitLiveCost LivesCostComponent { get; private set; }
    public UnitMoney MoneyComponent { get; private set; }
    public EffectsContainer EffectsContainer { get; private set; }
    public CharacterPackAppearance CharacterAppearance { get; private set; }
    
    private void Awake()
    {
        HealthComponent = GetComponent<UnitHealth>();
        LivesCostComponent = GetComponent<UnitLiveCost>();
        MoneyComponent = GetComponent<UnitMoney>();
        EffectsContainer = GetComponent<EffectsContainer>();
        CharacterAppearance = GetComponentInChildren<CharacterPackAppearance>();
    }
}
