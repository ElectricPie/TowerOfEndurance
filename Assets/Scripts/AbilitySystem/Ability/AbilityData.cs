using System;
using UnityEngine;

[Serializable]
public class AbilityData
{
    [SerializeField] 
    private AbilityTrigger m_trigger = AbilityTrigger.OnBasicAttackFired;
    public AbilityTrigger Trigger => m_trigger;
    
    public virtual AbilityData Clone()
    {
        return (AbilityData)this.MemberwiseClone();
    }
    
    public virtual void Init(AbilityInitData initData) { }

    public virtual bool TryActivate(GameObject target, GameObject caster, int level = 1)
    {
        return false;
    }
}

public enum AbilityTrigger
{
    OnBasicAttackFired,
    OnBasicAttackHit,
    OnAnyDamage,
    Timed
}