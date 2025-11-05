using System;

/// <summary>
/// This is a 
/// </summary>
[Serializable]
public abstract class AbilityData
{
    public abstract AbilityInstance CreateAbilityInstance(AbilityInitData initData);
}
