using System.Reflection;
using System.Text.RegularExpressions;
using AbilitySystem.Ability;
using Sirenix.Utilities;
using Ui.Ability;
using UnityEngine;

namespace Ui.Tooltip
{
    public class AbilityTooltipData : TooltipData
    {
        public BuyAbilityScriptableObject BuyAbilityScriptableObject { get; private set; }
        public AbilityScriptableObject AbilityScriptableObject => BuyAbilityScriptableObject.AbilityScriptableObject;
        public AbilityData AbilityData => AbilityScriptableObject.AbilityData;

        private AbilityTooltipData() { }
        public AbilityTooltipData(BuyAbilityScriptableObject buyAbilityScriptableObject, string description)
        {
            BuyAbilityScriptableObject = buyAbilityScriptableObject;
            Description = description;
        }
    }

    public class AbilityTooltipWidget : TooltipWidget
    {
        public override void SetData(TooltipData data)
        {
            if (data is not AbilityTooltipData abilityTooltipData)
            {
                Debug.LogError("Invalid data type for AbilityTooltipWidget");
                return;
            }

            string resultDescription = GetProcessedString(abilityTooltipData);

            Debug.Log(resultDescription);
        }
    }
}