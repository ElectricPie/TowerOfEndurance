using AbilitySystem.Ability;
using TMPro;
using Ui.Ability;
using UnityEngine;

namespace Ui.Tooltip
{
    public class AbilityTooltipData : TooltipData
    {
        public AbilityScriptableObject AbilityScriptableObject { get; private set; }
        public AbilityData AbilityData => AbilityScriptableObject.AbilityData;

        private AbilityTooltipData() { }
        public AbilityTooltipData(AbilityScriptableObject buyAbilityScriptableObject)
        {
            AbilityScriptableObject = buyAbilityScriptableObject;
        }
    }

    public class AbilityTooltipWidget : TooltipWidget
    {
        [SerializeField] private TMP_Text m_titleText;
        [SerializeField] private TMP_Text m_descriptionText;
        
        public override void SetData(TooltipData data)
        {
            if (data is not AbilityTooltipData abilityTooltipData)
            {
                Debug.LogError("Invalid data type for AbilityTooltipWidget");
                return;
            }

            AbilityScriptableObject abilityScriptableObject = abilityTooltipData.AbilityScriptableObject;
            string resultTitle = abilityScriptableObject.Label;
            m_titleText.text = resultTitle;
            
            string resultDescription = FormatTooltipDescription(abilityTooltipData, abilityScriptableObject.AbilityData.Description);
            m_descriptionText.text = resultDescription;
        }
    }
}