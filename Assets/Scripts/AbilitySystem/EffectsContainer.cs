using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AbilitySystem.Ability.Attributes;
using AbilitySystem.Ability.AttributeSets;
using AbilitySystem.Effect;
using UnityEngine;

// TODO: Reimplement fx on application/removal 
internal class PeriodicEffectContainer
{
    public readonly GameEffectScriptableObject Effect;
    public readonly int Level;
    public event Action OnExpiration;

    public readonly AttributeSet Source;
    private float ExpirationTime { get; set; }

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
        ExpirationTime = Time.time + Effect.PeriodicEffectValues.GetDurationAt(Level);
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

    // TODO: Remove one all update to work with new attribute system
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
                // SetupPeriodicEffect(source, effect, level);
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
        if (effectContainer.Effect.PeriodicEffectValues.TriggerOnApplication)
        {
            foreach (AttributeModifier modifier in effectContainer.Effect.Modifiers)
            {
                m_attributeSet.AddInstantModifier(new AttributeModifierInstance(effectContainer.Source, modifier, effectContainer.Level));
            } 
        }
        
        while (!effectContainer.HasExpired())
        {
            yield return new WaitForSeconds(effectContainer.Effect.PeriodicEffectValues.GetPeriodAt(effectContainer.Level));
            foreach (AttributeModifier modifier in effectContainer.Effect.Modifiers)
            {
                m_attributeSet.AddInstantModifier(new AttributeModifierInstance(effectContainer.Source, modifier, effectContainer.Level));
            }
        }

        m_periodicEffects.Remove(effectContainer);
    }
}