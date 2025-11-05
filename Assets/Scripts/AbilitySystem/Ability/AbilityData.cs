using System;
using System.Collections.Generic;
using System.ComponentModel;
using EditorAttributes;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public abstract class AbilityData
{
    public virtual AbilityData Clone()
    {
        AbilityData clone = (AbilityData)MemberwiseClone();
        return clone;
    }
    
    public virtual void Init(AbilityInitData initData) { }

    public virtual bool TryActivate(GameObject target, GameObject caster, int level = 1)
    {
        return false;
    }

    public abstract Dictionary<string, object> GetTooltipDataMap(int level);
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