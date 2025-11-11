using System;
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
        [SerializeField] private AttributeIdScriptableObject m_towerDamageAttributeId;
        
        private AttributeSet m_towerAttributeSet;

        protected void Awake()
        {
            m_towerAttributeSet = GameObject.FindGameObjectWithTag("Player").GetComponent<AttributeSet>();
        }

        public override void SetData(TooltipData data)
        {
            if (data is not AbilityTooltipData abilityTooltipData)
            {
                Debug.LogError("Invalid data type for AbilityTooltipWidget");
                return;
            }

            AbilityScriptableObject ability = abilityTooltipData.Ability;
            string resultTitle = ability.Label;
            m_titleText.text = resultTitle;

            float towerDamage = m_towerAttributeSet.GetAttributeValue(m_towerDamageAttributeId);

            
            bool isMaxLevel = abilityTooltipData.Level >= ability.MaxLevel;
            int abilityLevel = abilityTooltipData.Level;
            if (!isMaxLevel && abilityLevel >= 1)
            {
                abilityLevel--;
            }
            
            Dictionary<string, object> tooltipDataMap = ability.AbilityData.GetTooltipMap(abilityLevel);
            // Allows ability tooltips to use tower damage 
            if (tooltipDataMap.TryGetValue("DamageModifierValue", out object damageModifier))
            {
                tooltipDataMap["DamageModifierValue"] = (float)damageModifier * towerDamage;
            }
            tooltipDataMap.Add("TowerDamage", towerDamage);
            tooltipDataMap.Add("Cost", ability.GetCostAt(abilityTooltipData.Level));
            tooltipDataMap.Add("TriggerTime", ability.GetTriggerTimeAt(abilityTooltipData.Level));
            
            // Format any [] placeholders in the description
            string resultDescription = FormatTooltipDescriptionWithTooltipDataMap(tooltipDataMap, ability.Description);
            m_descriptionText.text = resultDescription;
        }
    }
}