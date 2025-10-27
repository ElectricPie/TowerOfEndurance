using AbilitySystem.Ability;
using TMPro;
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
        public AbilityTooltipData(BuyAbilityScriptableObject buyAbilityScriptableObject)
        {
            BuyAbilityScriptableObject = buyAbilityScriptableObject;
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

            string resultTitle = abilityTooltipData.AbilityScriptableObject.Label;
            m_titleText.text = resultTitle;
            
            string resultDescription = FormatTooltipDescription(abilityTooltipData, abilityTooltipData.BuyAbilityScriptableObject.Description);
            m_descriptionText.text = resultDescription;
        }
    }
}