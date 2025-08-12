using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class AbilityData
{
    [SerializeField] 
    private AbilityTrigger m_trigger = AbilityTrigger.OnBasicAttackFired;
    [SerializeField, ShowIf("m_trigger", AbilityTrigger.Timed)]
    private AnimationCurve m_triggerTime;
    
    public AbilityTrigger Trigger => m_trigger;
    public float TriggerTime(int level) => m_triggerTime.Evaluate(level);
    
    public virtual AbilityData Clone()
    {
        AbilityData clone = (AbilityData)this.MemberwiseClone();
        clone.m_triggerTime = this.m_triggerTime;
        return clone;
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