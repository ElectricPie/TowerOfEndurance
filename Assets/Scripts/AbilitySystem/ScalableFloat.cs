using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AbilitySystem
{
    [Serializable]
    public class ScalableFloat
    {
        [SerializeField] private bool m_useCurve = false;
        [SerializeField, HideIf("m_useCurve")] private float m_flatFloat;
        [SerializeField, ShowIf("m_useCurve")] private AnimationCurve m_curve;

        public bool UseCurve => m_useCurve;
        public float FlatFloat => m_flatFloat;
        public AnimationCurve Curve => m_curve;
    }
}