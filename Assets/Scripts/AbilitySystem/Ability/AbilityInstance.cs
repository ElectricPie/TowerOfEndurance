using UnityEngine;

public class AbilityInitData
{
    public GameObject Owner { get; } 
    
    private AbilityInitData() {}
    public AbilityInitData(GameObject owner)
    {
        Owner = owner;
    }
}

public sealed class AbilityInstance
{
    public int Level { get; private set; } = 1;

    public AbilityData AbilityData { get; private set; }
    private readonly GameObject m_owner;
    
    private AbilityInstance() { }
    public AbilityInstance(AbilityData abilityData, AbilityInitData initData)
    {
        AbilityData = abilityData.Clone();
        AbilityData.Init(initData);
        m_owner = initData.Owner;
    }
    
    public bool TryActivate(GameObject target = null)
    {
        return AbilityData.TryActivate(target, m_owner, Level);
    }

    public void SetLevel(int newLevel)
    {
        Level = Mathf.Max(1, newLevel);
    }
}
