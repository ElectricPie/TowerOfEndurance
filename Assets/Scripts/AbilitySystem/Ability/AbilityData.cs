using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public abstract class AbilityDataOld
{
    public virtual void Init(AbilityInitData initData) { }
    public virtual bool TryActivate(GameObject target, GameObject caster, int level = 1)
    {
        return false;
    }
    public virtual Dictionary<string, object> GetTooltipDataMap(int level)
    {
        return new Dictionary<string, object>();
    }
}


/// <summary>
/// This is a 
/// </summary>
[Serializable]
public abstract class AbilityData
{
    public virtual void Init(AbilityInitData initData) { }
    public virtual bool TryActivate(GameObject target, GameObject caster, int level = 1)
    {
        return false;
    }
    public virtual Dictionary<string, object> GetTooltipDataMap(int level)
    {
        return new Dictionary<string, object>();
    }
}


public enum AbilityTrigger
{
    [Description("Triggers when towers the basic attack is fired")]
    OnBasicAttackFired,
    [Description("Triggers when towers the basic attack hits an enemy")]
    OnBasicAttackHit,
    [Description("Triggers when any damage is dealt to a target")]
    OnAnyDamage,
    [Description("Triggers at set time intervals")]
    Timed
}