using System;
using AbilitySystem.Ability;
using UnityEngine;

public class AbilityInitData
{
    public GameObject Source { get; } 
    public AbilityData AbilityData { get; }
    
    private AbilityInitData() {}
    public AbilityInitData(GameObject source, AbilityData abilityData)
    {
        Source = source;
        AbilityData = abilityData;
    }
}


[Serializable]
public abstract class AbilityInstance
{
    public abstract void Init(AbilityInitData initData);
    public abstract bool TryActivate(GameObject target, GameObject caster, int level = 1);
}

public sealed class AbilityInstanceOld
{
    public int Level { get; private set; } = 1;
    public AbilityScriptableObject AbilityScriptableObject { get; }
    public AbilityDataOld AbilityDataOld => AbilityScriptableObject.AbilityDataOld;
    
    private readonly GameObject m_caster;
    
    public float GetCostForNextLevel()
    {
        if (Level >= AbilityScriptableObject.MaxLevel)
            return -1.0f;
        
        return AbilityScriptableObject.GetCostAt(Level + 1);
    }
    
    private AbilityInstanceOld() { }
    public AbilityInstanceOld(AbilityScriptableObject ability, AbilityInitData initData)
    {
        AbilityScriptableObject = ability;
        AbilityDataOld.Init(initData);
        m_caster = initData.Source;
    }

    public void Upgrade()
    {
        Level++;
    }
    
    public bool TryActivate(GameObject target = null)
    {
        return AbilityDataOld.TryActivate(target, m_caster, Level);
    }

    public void SetLevel(int newLevel)
    {
        Level = Mathf.Max(1, newLevel);
    }
}
