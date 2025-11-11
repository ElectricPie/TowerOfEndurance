using AbilitySystem.Ability;
using UnityEngine;

public class AbilityInitData
{
    public GameObject Source { get; }
    public AbilityScriptableObject Ability { get; }
    public AbilityData AbilityData => Ability.AbilityData;
    
    private AbilityInitData() {}
    public AbilityInitData(GameObject source, AbilityScriptableObject ability)
    {
        Source = source;
        Ability = ability;
    }
}

public abstract class AbilityInstance
{
    public AbilityScriptableObject Ability { get; private set; }
    public int Level { get; private set; } = 1;
    protected GameObject Source { get; private set; }

    private AbilityInstance() {}
    public AbilityInstance(AbilityInitData initData)
    {
        Ability = initData.Ability;
        Source = initData.Source;
    }
    
    public abstract void TryActivate(GameObject target = null);

    public void Upgrade()
    {
        Level++;
    }

}
