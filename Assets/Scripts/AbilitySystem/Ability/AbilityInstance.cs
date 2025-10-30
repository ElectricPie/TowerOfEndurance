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
    public AbilityData AbilityData { get; private set; }
    
    private readonly GameObject m_caster;
    
    public float GetCostForNextLevel()
    {
        if (Level >= AbilityData.MaxLevel)
            return -1.0f;
        
        return AbilityData.GetCostAt(Level + 1);
    }
    
    private AbilityInstance() { }
    public AbilityInstance(AbilityData abilityData, AbilityInitData initData)
    {
        AbilityData = abilityData.Clone();
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
