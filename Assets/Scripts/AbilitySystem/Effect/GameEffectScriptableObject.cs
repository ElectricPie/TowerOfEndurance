using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AbilitySystem.Effect
{
    [CreateAssetMenu(fileName = "New Game Effect", menuName = "Ability System/Game Effect", order = 0)]
    public class GameEffectScriptableObject : ScriptableObject
    {
        [SerializeField] private DurationPolicy m_durationPolicy = DurationPolicy.Instant;
        [SerializeField, ShowIf("m_durationPolicy", DurationPolicy.Periodic)] private PeriodicEffectValues m_periodicEffectValues;
        [SerializeField] private List<AttributeModifier> m_modifiers;

        public DurationPolicy DurationPolicy => m_durationPolicy;
        public PeriodicEffectValues PeriodicEffectValues => m_periodicEffectValues;
        public List<AttributeModifier> Modifiers => m_modifiers;
    }
}