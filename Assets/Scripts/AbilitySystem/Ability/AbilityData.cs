using System;
using System.ComponentModel;
using EditorAttributes;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class AbilityData
{
    [SerializeField] private string m_label = "New Ability";
    [SerializeField, TooltipTextArea] private string m_description = "No Description";
    [SerializeField] private AnimationCurve m_cost = AnimationCurve.Linear(1, 10, 10, 100);
    [SerializeField] private AbilityTrigger m_trigger = AbilityTrigger.OnBasicAttackFired;
    [SerializeField, ShowIf("m_trigger", AbilityTrigger.Timed)]
    private AnimationCurve m_triggerTime;
    
    public string Label => m_label;
    public string Description => m_description;
    public AbilityTrigger Trigger => m_trigger;
    
    
    public float GetCostAt(int level) => m_cost.Evaluate(level);
    public float GetTriggerTimeAt(int level) => m_triggerTime.Evaluate(level);
    
    public virtual AbilityData Clone()
    {
        AbilityData clone = (AbilityData)MemberwiseClone();
        clone.m_triggerTime = m_triggerTime;
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
    [Description("Triggers when towers the basic attack is fired")]
    OnBasicAttackFired,
    [Description("Triggers when towers the basic attack hits an enemy")]
    OnBasicAttackHit,
    [Description("Triggers when any damage is dealt to a target")]
    OnAnyDamage,
    [Description("Triggers at set time intervals")]
    Timed
}