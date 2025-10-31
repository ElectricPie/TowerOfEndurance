using TMPro;
using UnityEngine;

namespace Ui.Tooltip.Ability
{
    public class AbilityTooltipData : TooltipData
    {
        public AbilityData Ability { get; }
        public int Level { get; }
        
        public AbilityTooltipData() {}
        public AbilityTooltipData(AbilityData ability, int level)
        {
            Ability = ability;
            Level = level;
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

            // Format any [] placeholders in the description
            string resultDescription = FormatTooltipDescriptionWithTooltipDataMap(abilityData.GetTooltipDataMap(abilityTooltipData.Level), abilityData.Description);
            // Format any {} placeholders in the description
            resultDescription = FormatTooltipDescriptionWithObject(abilityTooltipData.Ability, resultDescription);
            m_descriptionText.text = resultDescription;
        }
    }
}