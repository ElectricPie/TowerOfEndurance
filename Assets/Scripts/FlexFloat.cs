using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public struct FlexFloat
{
    [SerializeField, EnumToggleButtons] private FloatType m_floatType;
    [SerializeField, ShowIf("m_floatType", FloatType.Float), LabelText("Value")] private float m_value;
    [SerializeField, ShowIf("m_floatType", FloatType.ScalableFloat), LabelText("Value")] private AnimationCurve m_valueCurve;
    
    public float Value(int level = 1)
    {
        return m_floatType == FloatType.ScalableFloat ? m_valueCurve.Evaluate(level) : m_value;
    }
}

public enum FloatType
{
    Float,
    ScalableFloat
}