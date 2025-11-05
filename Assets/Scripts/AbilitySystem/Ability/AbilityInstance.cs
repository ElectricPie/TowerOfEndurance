using AbilitySystem.Ability;
using UnityEngine;

public class AbilityInitData
{
    public GameObject Caster { get; protected set; } 
    
    private AbilityInitData() {}
    public AbilityInitData(GameObject caster)
    {
        Caster = caster;
    }
}

public sealed class AbilityInstance
{
    public int Level { get; private set; } = 1;
    public AbilityScriptableObject AbilityScriptableObject { get; }
    public AbilityData AbilityData { get; }
    
    private readonly GameObject m_caster;
    
    public float GetCostForNextLevel()
    {
        if (Level >= AbilityScriptableObject.MaxLevel)
            return -1.0f;
        
        return AbilityScriptableObject.GetCostAt(Level + 1);
    }
    
    private AbilityInstance() { }
    public AbilityInstance(AbilityScriptableObject ability, AbilityInitData initData)
    {
        AbilityScriptableObject = ability;
        AbilityData = ability.AbilityData.Clone();
        AbilityData.Init(initData);
        m_caster = initData.Caster;
    }

    public void Upgrade()
    {
        Level++;
    }
    
    public bool TryActivate(GameObject target = null)
    {
        return AbilityData.TryActivate(target, m_caster, Level);
    }

    public void SetLevel(int newLevel)
    {
        Level = Mathf.Max(1, newLevel);
    }
}
