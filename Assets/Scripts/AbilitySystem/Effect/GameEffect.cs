using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public abstract class GameEffect
{
    [SerializeField] 
    private DurationPolicy m_durationPolicy = DurationPolicy.Instant;
    [SerializeField, ShowIf("m_durationPolicy", DurationPolicy.Periodic), InlineProperty, HideLabel] 
    private PeriodicEffectValues m_periodicEffectValues;

    public DurationPolicy DurationPolicy => m_durationPolicy;
    public PeriodicEffectValues PeriodicEffectValues => m_periodicEffectValues;
    
    // TODO: Think this should take attribute set for caster as it should only be called on effect container which
    //   should only be on object with attribute sets
    // TODO: Consider attribute sets as scriptable object with list for attribute names
    public abstract void Execute(GameObject caster, GameObject target, int level = 1);
    public virtual void OnApplication(GameObject target) { }
    public virtual void OnRemove() { }
}

[Serializable]
public class PeriodicEffectValues {
    [SerializeField, BoxGroup("Periodic")] 
    private AnimationCurve m_duration = AnimationCurve.Linear(1, 1, 5, 5);
    [SerializeField, BoxGroup("Periodic")] 
    private AnimationCurve m_period = AnimationCurve.Linear(1, 1, 5, 5);
    [SerializeField, BoxGroup("Periodic")]
    private bool m_triggerOnApplication;

    public float GetDurationAt(int level) => m_duration.Evaluate(level);
    public float GetPeriodAt(int level) => m_period.Evaluate(level);
    public bool TriggerOnApplication => m_triggerOnApplication;
}

public enum DurationPolicy
{
    Instant,
    Periodic
}