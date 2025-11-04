using System;
using AbilitySystem.Ability.Attributes;
using AbilitySystem.Ability.AttributeSets;
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
    
    public abstract void Execute(GameObject caster, GameObject target, int level = 1);
    public virtual void OnApplication(GameObject target) { }
    public virtual void OnRemove() { }
}

[Serializable]
public class PeriodicEffectValues {
    // TODO: Replace with CurveFloat
    [SerializeField, BoxGroup("Periodic")] 
    private AnimationCurve m_duration = AnimationCurve.Linear(1, 1, 5, 5);
    // TODO: Replace with CurveFloat
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
public class CurveFloat
{
    [SerializeField] private bool m_useCurve = false;
    [SerializeField, HideIf("m_useCurve")] private float m_flatFloat;
    [SerializeField, ShowIf("m_useCurve")] private AnimationCurve m_curve;

    public bool UseCurve => m_useCurve;
    public float FlatFloat => m_flatFloat;
    public AnimationCurve Curve => m_curve;
}

[Serializable]
public class ModifierMagnitude
{
    [SerializeField] private CalculationType m_calculationType = CalculationType.Float;
    // TODO: Replace with CurveFloat
    [SerializeField, ShowIf("m_calculationType", CalculationType.Float)] private float m_flatValue;

    [SerializeField, ShowIf("m_calculationType", CalculationType.AttributeBacked)]
    private AttributeBackedMagnitude m_attributeBackedMagnitude;

    public CalculationType CalculationType => m_calculationType;
    public float FlatValue => m_flatValue;
    public AttributeBackedMagnitude AttributeBackedMagnitude => m_attributeBackedMagnitude;
}

[Serializable]
public enum AttributeSource
{
    Target,
    Source
} 

[Serializable]
public class AttributeBackedMagnitude
{
    [SerializeField] private AttributeSource m_attributeSource;
    [SerializeField] private AttributeIdScriptableObject m_backingAttributeId;
    [SerializeField] private CurveFloat m_coefficient;
    [SerializeField] private CurveFloat m_postAdditiveValue;

    public AttributeSource AttributeSource => m_attributeSource;
    public AttributeIdScriptableObject BackingAttributeId => m_backingAttributeId;
    public CurveFloat Coefficient => m_coefficient;
    public CurveFloat PostAdditiveValue => m_postAdditiveValue;
}

[Serializable]
public class AttributeModifier
{
    [SerializeField] private AttributeIdScriptableObject m_attributeId;
    [SerializeField] private ModifierOperation m_modifierOperation = ModifierOperation.Add;
    [SerializeField] private ModifierMagnitude m_modifierMagnitude = new ModifierMagnitude();

    public AttributeIdScriptableObject AttributeId => m_attributeId;
    public ModifierOperation Operation => m_modifierOperation;
    public ModifierMagnitude Magnitude => m_modifierMagnitude;
}
