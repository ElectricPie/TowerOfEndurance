using System;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class DamageEffect : GameEffect
{
    [SerializeField] private AnimationCurve m_damageCurve;
    [SerializeField] private GameObject m_visualFx;

    private GameObject m_visualFxInstance;
    
    public AnimationCurve DamageCurve => m_damageCurve;
    public GameObject VisualFx => m_visualFx;


    public override void Execute(GameObject caster, GameObject target, int level = 1)
    {
        float damage = DamageCurve.Evaluate(level);
        target.GetComponent<UnitHealth>()?.Damage(damage, caster);
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