using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem.Effect
{
    [CreateAssetMenu(fileName = "New Game Effect", menuName = "Ability System/Game Effect", order = 0)]
    public class GameEffectScriptableObject : ScriptableObject
    {
        [SerializeField] private DurationPolicy m_durationPolicy = DurationPolicy.Instant;
        [SerializeField] private List<AttributeModifier> m_modifiers;

        public DurationPolicy DurationPolicy => m_durationPolicy;
        public List<AttributeModifier> Modifiers => m_modifiers;
    }
}