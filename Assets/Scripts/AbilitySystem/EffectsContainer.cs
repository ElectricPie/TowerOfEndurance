using System;
using System.Collections;
using System.Collections.Generic;
using AbilitySystem.Ability.Attributes;
using AbilitySystem.Ability.AttributeSets;
using AbilitySystem.Effect;
using UnityEngine;

internal class PeriodicEffectContainer
{
    public readonly GameEffect Effect;
    public readonly int Level;
    public event Action OnExpiration;

    private readonly GameObject m_caster;
    private readonly GameObject m_target;
    private float ExpirationTime { get; set; }

    private PeriodicEffectContainer()
    {
    }

    public PeriodicEffectContainer(GameObject caster, GameObject target, GameEffect effect, int level,
        Action expirationCallback)
    {
        m_caster = caster;
        Effect = effect;
        m_target = target;
        Level = level;

        RefreshDuration();
        OnExpiration = expirationCallback;
        effect.OnApplication(target);

        if (Effect.PeriodicEffectValues.TriggerOnApplication)
        {
            Execute();
        }
    }

    public void RefreshDuration()
    {
        ExpirationTime = Time.time + Effect.PeriodicEffectValues.GetDurationAt(Level);
    }

    public bool HasExpired()
    {
        if (Time.time < ExpirationTime)
            return false;

        OnExpiration?.Invoke();
        Effect.OnRemove();
        return true;
    }

    public void Execute()
    {
        Effect.Execute(m_caster, m_target, Level);
    }
}

internal class GameEffectInstance
{
    private List<AttributeModifier> m_modifiers;

    public GameEffectInstance(GameEffectScriptableObject effectScriptableObject)
    {
        m_modifiers = effectScriptableObject.Modifiers;
    }
}

public class EffectsContainer : MonoBehaviour
{
    private readonly Dictionary<Type, PeriodicEffectContainer> m_effects =
        new Dictionary<Type, PeriodicEffectContainer>();

    private readonly List<GameEffectInstance> m_infiniteEffects = new List<GameEffectInstance>();

    private AttributeSet m_attributeSet;

    protected void Awake()
    {
        m_attributeSet = GetComponent<AttributeSet>();
    }

    public void ApplyEffect(GameObject source, GameEffect effect, int level = 1)
    {
        switch (effect.DurationPolicy)
        {
            case DurationPolicy.Instant:
                effect.OnApplication(gameObject);
                effect.Execute(source, gameObject, level);
                effect.OnRemove();
                break;
            case DurationPolicy.Periodic:
                SetupPeriodicEffect(source, effect, level);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ApplyEffect(GameObject source, GameEffectScriptableObject effect, int level)
    {
        AttributeSet sourceAttributeSet = source.GetComponent<AttributeSet>();
        
        switch (effect.DurationPolicy)
        {
            case DurationPolicy.Instant:
                foreach (AttributeModifier modifier in effect.Modifiers)
                {
                    m_attributeSet.AddInstantModifier(new AttributeModifierInstance(sourceAttributeSet, modifier, level));
                }
                break;
            case DurationPolicy.Periodic:
                break;
            case DurationPolicy.Infinite:
                foreach (AttributeModifier modifier in effect.Modifiers)
                {
                    m_attributeSet.AddPersistentModifier(new AttributeModifierInstance(sourceAttributeSet, modifier, level));
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetupPeriodicEffect(GameObject caster, GameEffect effect, int level)
    {
        if (m_effects.ContainsKey(effect.GetType()))
        {
            m_effects[effect.GetType()].RefreshDuration();
            return;
        }

        PeriodicEffectContainer effectContainer = new PeriodicEffectContainer(caster, gameObject, effect, level,
            () => { m_effects.Remove(effect.GetType()); });
        m_effects.Add(effect.GetType(), effectContainer);
        IEnumerator newTickCoroutine = PeriodicEffectCoroutine(effectContainer);
        StartCoroutine(newTickCoroutine);
    }

    private static IEnumerator PeriodicEffectCoroutine(PeriodicEffectContainer effectContainer)
    {
        while (true)
        {
            if (effectContainer.HasExpired())
            {
                yield break;
            }

            yield return new WaitForSeconds(
                effectContainer.Effect.PeriodicEffectValues.GetPeriodAt(effectContainer.Level));
            effectContainer.Execute();
        }
    }
}