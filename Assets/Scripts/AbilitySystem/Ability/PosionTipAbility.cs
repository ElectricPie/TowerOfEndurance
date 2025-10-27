using UnityEngine;

public class PoisonTipAbilityData : AbilityData
{
    /* Editor Values */
    [SerializeField] private DamageEffect m_damageEffect;
    
    public DamageEffect DamageEffect => m_damageEffect;

    public override AbilityData Clone()
    {
        PoisonTipAbilityData clone = (PoisonTipAbilityData)this.MemberwiseClone();
        clone.m_damageEffect = this.m_damageEffect;

        return clone;
    }

    
    /* Runtime Values */
    public override bool TryActivate(GameObject target, GameObject caster, int level = 1)
    {
        if (target == null || caster == null)
            return false;
  
        EffectsContainer effectsContainer = target.GetComponent<EffectsContainer>();
        if (effectsContainer == null)
            return false;
        
        effectsContainer.ApplyEffect(caster, m_damageEffect, level);
        return true;
    }
}