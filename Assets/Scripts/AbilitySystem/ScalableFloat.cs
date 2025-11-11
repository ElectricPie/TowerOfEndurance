using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AbilitySystem
{
    [Serializable]
    public enum ScalableFloatType
    {
        Float,
        Curve
    }
    
    [Serializable]
    public class ScalableFloat
    {
        [SerializeField] private ScalableFloatType m_type = ScalableFloatType.Float;
        [SerializeField, HideIf("m_type", ScalableFloatType.Curve)] private float m_value = 1;
        [SerializeField, HideIf("m_type", ScalableFloatType.Float)] private AnimationCurve m_curve = AnimationCurve.Linear(1, 1, 5, 5);

        public float GetValue(int level = 0)
        {
            return m_type == ScalableFloatType.Float ? m_value : m_curve.Evaluate(level);
        }
    }
}