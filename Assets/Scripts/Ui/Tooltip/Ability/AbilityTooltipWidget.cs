using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

using TooltipDataMap = System.Collections.Generic.Dictionary<string, object>;

namespace Ui.Tooltip.Ability
{
    public class AbilityTooltipData : TooltipData
    {
        public AbilityData Ability { get; protected set; }
        public int Level { get; }
        protected TooltipDataMap PlaceholderValueMap = new TooltipDataMap();
        
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

        private static readonly Regex s_dataPlaceholderRegex = new Regex(@"\[(\w+)\]");
        
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
            // string resultDescription = FormatTooltipDescriptionWithTooltipData(abilityTooltipData, abilityData.Description);
            string resultDescription = FormatTooltipDescriptionWithTooltipDataMap(abilityData.GetTooltipDataMap(abilityTooltipData.Level), abilityData.Description);
            // Format any {} placeholders in the description
            resultDescription = FormatTooltipDescriptionWithObject(abilityTooltipData.Ability, resultDescription);
            m_descriptionText.text = resultDescription;
        }
        
        // /// <summary>
        // /// Replaces placeholders in the given string with corresponding values from the provided AbilityTooltipData.
        // /// </summary>
        // /// <param name="data">The AbilityTooltipData object containing the data to replace placeholders with.</param>
        // /// <param name="stringToFormat">The string containing placeholders to be replaced.</param>
        // /// <returns>
        // /// A new string with placeholders replaced by their corresponding values from the AbilityTooltipData.
        // /// If a placeholder key is not recognized, the original placeholder is retained in the string.
        // /// </returns>
        // private static string FormatTooltipDescriptionWithTooltipData(AbilityTooltipData data, string stringToFormat)
        // {
        //     string resultDescription = s_dataPlaceholderRegex.Replace(stringToFormat, match => 
        //     {
        //         string key = match.Groups[1].Value.ToLower();
        //         
        //         // Any new placeholders specific to AbilityTooltipData need to be added here
        //         return key switch
        //         {
        //             "cost" => Mathf.CeilToInt(data.Cost).ToString(),
        //             _ => match.Value
        //         };
        //     });
        //
        //     return resultDescription;
        // }

        /// <summary>
        /// Replaces placeholders in the given string with corresponding values from the provided AbilityTooltipData.
        /// </summary>
        /// <param name="map">A dictionary where the key is the place holder to replace and the value is the value to
        /// replace the placeholder</param>
        /// <param name="stringToFormat">The string containing placeholders to be replaced.</param>
        /// <returns>
        /// A new string with placeholders replaced by their corresponding values from the AbilityTooltipData.
        /// If a placeholder key is not recognized, the original placeholder is retained in the string.
        /// </returns>
        private static string FormatTooltipDescriptionWithTooltipDataMap(TooltipDataMap map, string stringToFormat)
        {
            string resultDescription = s_dataPlaceholderRegex.Replace(stringToFormat, match => 
            {
                string key = match.Groups[1].Value;
                map.TryGetValue(key, out object value);
                return value != null ? value.ToString() : match.Value;
            });

            return resultDescription;
        }
        
        
    }
}