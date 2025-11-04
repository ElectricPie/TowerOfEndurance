using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AbilitySystem.Effect.EffectProperties
{
    [Serializable]
    public class PeriodicEffectValues {
        [SerializeField, BoxGroup("Periodic Values")] 
        private ScalableFloat m_duration;
        [SerializeField, BoxGroup("Periodic Values")] 
        private ScalableFloat m_period;
        [SerializeField, BoxGroup("Periodic Values")]
        private bool m_triggerOnApplication;

        public ScalableFloat Duration => m_duration;
        public ScalableFloat Period => m_period;
        public bool TriggerOnApplication => m_triggerOnApplication;
    }
}