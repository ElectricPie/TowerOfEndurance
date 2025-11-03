using System;
using AbilitySystem.Ability.AttributeSets;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class DamageEffect : GameEffect
{
    [SerializeField] private AnimationCurve m_damageModifierCurve;
    [SerializeField] private GameObject m_visualFx;

    private GameObject m_visualFxInstance;
    
    public AnimationCurve DamageModifierCurve => m_damageModifierCurve;
    public GameObject VisualFx => m_visualFx;
    public float DamageModifierAt(int level) => m_damageModifierCurve.Evaluate(level);
    
    public override void Execute(GameObject caster, GameObject target, int level = 1)
    {
        TowerAttributeSet towerAttributeSet = caster.GetComponent<TowerAttributeSet>();
        if (towerAttributeSet == null)
            return;

        float damage = towerAttributeSet.Damage * m_damageModifierCurve.Evaluate(level);
        // target.GetComponent<UnitHealth>()?.Damage(damage, caster);
    }

    public override void OnApplication(GameObject target)
    {
        if (m_visualFx != null && m_visualFxInstance == null)
        {
            m_visualFxInstance = Object.Instantiate(VisualFx, target.transform);
        }
    }
    
    public override void OnRemove()
    {
        if (m_visualFxInstance != null)
        {
            Object.Destroy(m_visualFxInstance);
        }
    }
}