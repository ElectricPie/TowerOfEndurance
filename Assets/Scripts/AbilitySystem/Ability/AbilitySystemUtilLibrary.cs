using AbilitySystem.Effect;
using Sirenix.Utilities;
using UnityEngine;

namespace AbilitySystem.Ability
{
    public static class AbilitySystemUtilLibrary
    {
        public static void ApplyEffectToTarget(GameEffectScriptableObject effect, GameObject target, GameObject source, int level)
        {
            if (target == null || source == null)
                return;

            EffectsContainer effectsContainer = target.GetComponent<EffectsContainer>();
            if (effectsContainer == null)
                return;

            effectsContainer.ApplyEffect(source, effect, level);
        }
    }
}