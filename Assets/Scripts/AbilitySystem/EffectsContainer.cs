using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AbilitySystem.Ability.Attributes;
using AbilitySystem.Ability.AttributeSets;
using AbilitySystem.Effect;
using AbilitySystem.Effect.EffectProperties;
using UnityEngine;

// TODO: Reimplement fx on application/removal 
internal class PeriodicEffectContainer
{
    public readonly GameEffectScriptableObject Effect;
    public readonly int Level;
    public event Action OnExpiration;

    public readonly AttributeSet Source;
    public float ExpirationTime { get; private set; }

    private PeriodicEffectContainer() { }
    public PeriodicEffectContainer(AttributeSet source, GameEffectScriptableObject effect, int level,
        Action expirationCallback)
    {
        Source = source;
        Effect = effect;
        Level = level;

        RefreshDuration();
        OnExpiration = expirationCallback;
        // effect.OnApplication(target);
    }

    public void RefreshDuration()
    { 
        ExpirationTime = Time.time + Effect.PeriodicEffectValues.Duration.GetValue(Level);
    }

    public bool HasExpired()
    {
        if (Time.time < ExpirationTime)
            return false;

        OnExpiration?.Invoke();
        // Effect.OnRemove();
        return true;
    }
}

public class EffectsContainer : MonoBehaviour
{
    private readonly List<PeriodicEffectContainer> m_periodicEffects = new List<PeriodicEffectContainer>();

    private AttributeSet m_attributeSet;

    protected void Awake()
    {
        m_attributeSet = GetComponent<AttributeSet>();
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
                SetupPeriodicEffect(sourceAttributeSet, effect, level);
                break;
            case DurationPolicy.Infinite:
                foreach (AttributeModifier modifier in effect.Modifiers)
                {
                    m_attributeSet.AddInfiniteModifier(new AttributeModifierInstance(sourceAttributeSet, modifier, level));
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetupPeriodicEffect(AttributeSet source, GameEffectScriptableObject effect, int level)
    {
        // Refresh duration if effect exists
        foreach (PeriodicEffectContainer periodicEffect in m_periodicEffects.Where(periodicEffect => periodicEffect.Effect == effect && periodicEffect.Source))
        {
            periodicEffect.RefreshDuration();
            return;
        }

        PeriodicEffectContainer effectContainer = new PeriodicEffectContainer(source, effect, level,
            () => { }); //m_effects.Remove(effect.GetType()); });
        m_periodicEffects.Add(effectContainer);
        IEnumerator newTickCoroutine = PeriodicEffectCoroutine(effectContainer);
        StartCoroutine(newTickCoroutine);
    }

    private IEnumerator PeriodicEffectCoroutine(PeriodicEffectContainer effectContainer)
    {
        const float tickTimeTolerance = 0.0001f;

        if (effectContainer.Effect.PeriodicEffectValues.TriggerOnApplication)
        {
            ApplyPeriodicTick(effectContainer);
        }

        float startTime = Time.time;
        float period = effectContainer.Effect.PeriodicEffectValues.Period.GetValue(effectContainer.Level);
        int ticksApplied = 0;

        while (true)
        {
            yield return null;

            float lastDueTime = Mathf.Min(Time.time, effectContainer.ExpirationTime) + tickTimeTolerance;
            while (startTime + (ticksApplied + 1) * period <= lastDueTime)
            {
                ticksApplied++;
                ApplyPeriodicTick(effectContainer);
            }

            if (effectContainer.HasExpired())
                break;
        }

        m_periodicEffects.Remove(effectContainer);
    }

    private void ApplyPeriodicTick(PeriodicEffectContainer effectContainer)
    {
        foreach (AttributeModifier modifier in effectContainer.Effect.Modifiers)
        {
            m_attributeSet.AddInstantModifier(new AttributeModifierInstance(effectContainer.Source, modifier, effectContainer.Level));
        }
    }
}