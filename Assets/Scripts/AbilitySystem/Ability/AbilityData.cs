using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    [SerializeField]
    private string m_label;

    public string Label => m_label;
    
    protected void OnEnable()
    {
        if (string.IsNullOrEmpty(m_label))
        {
            m_label = name;
        }
    }

    public virtual bool Execute(GameObject target, GameObject caster, int level = 1) { return true; }

    public virtual AbilityInstance CreateAbilityInstance(AbilityInitData initData)
    {
        return new AbilityInstance(this, initData);
    }
}