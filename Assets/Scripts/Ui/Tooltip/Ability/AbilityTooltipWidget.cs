using System.Collections.Generic;
using AbilitySystem.Ability;
using AbilitySystem.Ability.Attributes;
using AbilitySystem.Ability.AttributeSets;
using TMPro;
using UnityEngine;

namespace Ui.Tooltip.Ability
{
    public class AbilityTooltipData : TooltipData
    {
        public AbilityScriptableObject Ability { get; }
        public int Level { get; }
        
        public AbilityTooltipData() {}
        public AbilityTooltipData(AbilityScriptableObject ability, int level)
        {
            Ability = ability;
            Level = level;
        }
    }

    public class AbilityTooltipWidget : TooltipWidget
    {
        [SerializeField] private TMP_Text m_titleText;
        [SerializeField] private TMP_Text m_descriptionText;

        private TowerAttributeSet m_towerAttributeSet;

        protected void Awake()
        {
            m_towerAttributeSet = FindFirstObjectByType<TowerAttributeSet>();
        }

        public override void SetData(TooltipData data)
        {
            if (data is not AbilityTooltipData abilityTooltipData)
            {
                Debug.LogError("Invalid data type for AbilityTooltipWidget");
                return;
            }

            AbilityScriptableObject abilityData = abilityTooltipData.Ability;
            string resultTitle = abilityData.Label;
            m_titleText.text = resultTitle;

            // Dictionary<string, object> tooltipDataMap = abilityData.GetTooltipDataMap(abilityTooltipData.Level);
            // tooltipDataMap.Add("TowerDamage", m_towerAttributeSet.Damage);
            // // Format any [] placeholders in the description
            // string resultDescription = FormatTooltipDescriptionWithTooltipDataMap(tooltipDataMap, abilityData.Description);
            // // Evaluate any expressions in {}
            // resultDescription = ProcessExpressions(resultDescription);
            // m_descriptionText.text = resultDescription;
        }
    }
}