using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Abilities/New Ability")]
public class AbilityScriptableObject : ScriptableObject
{
    [SerializeField]
    private string m_label;
    
    [SerializeReference] private AbilityData m_abilityData;

    public string Label => m_label;
    public AbilityData AbilityData => m_abilityData;
    
    protected void OnEnable()
    {
        if (string.IsNullOrEmpty(m_label))
        {
            m_label = name;
        }
    }
}

[Serializable]
public class AbilityData
{
    [SerializeField] 
    private AbilityTrigger m_trigger = AbilityTrigger.OnBasicAttackFired;
    public AbilityTrigger Trigger => m_trigger;
    
    public virtual AbilityData Clone()
    {
        return new AbilityData
        {
            m_trigger = this.m_trigger,
        };
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