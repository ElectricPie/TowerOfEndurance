using System;
using System.Collections.Generic;

[Serializable]
public abstract class AbilityData
{
    public abstract AbilityInstance CreateAbilityInstance(AbilityInitData initData);

    public virtual Dictionary<string, object> GetTooltipMap(int level)
    {
        return new Dictionary<string, object>();
    }
}
