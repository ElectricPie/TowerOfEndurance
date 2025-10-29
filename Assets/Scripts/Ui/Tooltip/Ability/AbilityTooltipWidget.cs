using TMPro;
using UnityEngine;

namespace Ui.Tooltip.Ability
{
    public class AbilityTooltipData : TooltipData
    {
        public AbilityData Ability { get; }

        private AbilityTooltipData() { }
        public AbilityTooltipData(AbilityData ability)
        {
            Ability = ability;
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

            AbilityData abilityData = abilityTooltipData.Ability;
            string resultTitle = abilityData.Label;
            m_titleText.text = resultTitle;
            
            string resultDescription = FormatTooltipDescription(abilityTooltipData.Ability, abilityData.Description);
            m_descriptionText.text = resultDescription;
        }
    }
}