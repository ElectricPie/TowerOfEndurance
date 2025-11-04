using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AbilitySystem.Effect.EffectProperties
{
    [Serializable]
    public class PeriodicEffectValues {
        // TODO: Replace with CurveFloat
        [SerializeField, BoxGroup("Periodic Values")] 
        private AnimationCurve m_duration = AnimationCurve.Linear(1, 1, 5, 5);
        // TODO: Replace with CurveFloat
        [SerializeField, BoxGroup("Periodic Values")] 
        private AnimationCurve m_period = AnimationCurve.Linear(1, 1, 5, 5);
        [SerializeField, BoxGroup("Periodic Values")]
        private bool m_triggerOnApplication;

        public float GetDurationAt(int level) => m_duration.Evaluate(level);
        public float GetPeriodAt(int level) => m_period.Evaluate(level);
        public bool TriggerOnApplication => m_triggerOnApplication;
    }
}