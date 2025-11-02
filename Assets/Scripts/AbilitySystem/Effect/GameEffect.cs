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
    Periodic,
    Infinite
}


[Serializable]
public enum ModifierOperation
{
    Add,
    Override
}

[Serializable]
public enum CalculationType
{
    Float,
    AttributeBacked
}

[Serializable]
public class ModifierMagnitude
{
    [SerializeField] private CalculationType m_calculationType = CalculationType.Float;
    [SerializeField, ShowIf("m_calculationType", CalculationType.Float)] private float m_flatValue;

    [SerializeField, ShowIf("m_calculationType", CalculationType.AttributeBacked)]
    private AttributeBackedMagnitude m_attributeBackedMagnitude;

    public CalculationType CalculationType => m_calculationType;
    public float FlatValue => m_flatValue;
    public AttributeBackedMagnitude AttributeBackedMagnitude => m_attributeBackedMagnitude;
}

[Serializable]
public class AttributeBackedMagnitude
{
    [SerializeField] private string m_backingAttribute = "";
    [SerializeField] private float m_coefficient = 1.0f;
    [SerializeField] private float m_postAdditiveValue = 0.0f;

    public string BackingAttribute => m_backingAttribute;
    public float Coefficient => m_coefficient;
    public float PostAdditiveValue => m_postAdditiveValue;
}

[Serializable]
public class AttributeModifier
{
    [SerializeField] private string m_attribute = "";
    [SerializeField] private ModifierOperation m_modifierOperation = ModifierOperation.Add;
    [SerializeField] private ModifierMagnitude m_modifierMagnitude = new ModifierMagnitude();

    public string AttributeName => m_attribute;
    public ModifierOperation Operation => m_modifierOperation;
    public ModifierMagnitude Magnitude => m_modifierMagnitude;
}
