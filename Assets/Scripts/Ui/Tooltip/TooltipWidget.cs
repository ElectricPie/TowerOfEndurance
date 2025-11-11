using System.Data;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Ui.Tooltip
{
    using TooltipDataMap = System.Collections.Generic.Dictionary<string, object>;
    
    /// <summary>
    /// Represents the base class for tooltip data
    /// </summary>
    public class TooltipData
    {
        protected TooltipDataMap PlaceholderValueMap = new TooltipDataMap();
    }

    public abstract class TooltipWidget : MonoBehaviour
    {
        private static readonly Regex s_placeholderExpressionRegex = new Regex(@"\{([^{}]+)\}");
        private static readonly Regex s_dataPlaceholderRegex = new Regex(@"\[(\w+)\]");
        private static readonly DataTable s_table = new DataTable();
        
        /// <summary>
        /// Abstract method to set the text of the tooltip widget.
        /// </summary>
        /// <param name="data">The tooltip data to be displayed.</param>
        public abstract void SetData(TooltipData data);
        
        /// <summary>
        /// Replaces placeholders in the given string with corresponding values from the provided AbilityTooltipData.
        /// </summary>
        /// <param name="map">A dictionary where the key is the placeholder to replace and the value is the value to
        /// replace the placeholder</param>
        /// <param name="stringToFormat">The string containing placeholders to be replaced.</param>
        /// <returns>
        /// A new string with placeholders replaced by their corresponding values from the AbilityTooltipData.
        /// If a placeholder key is not recognized, the original placeholder is retained in the string.
        /// </returns>
        protected static string FormatTooltipDescriptionWithTooltipDataMap(TooltipDataMap map, string stringToFormat)
        {
            string resultDescription = s_dataPlaceholderRegex.Replace(stringToFormat, match => 
            {
                string key = match.Groups[1].Value;
                map.TryGetValue(key, out object value);
                
                return value != null ? value.ToString() : match.Value;
            });

            return resultDescription;
        }

        protected static string ProcessExpressions(string stringToProcess)
        {
            string resultDescription = s_placeholderExpressionRegex.Replace(stringToProcess, match =>
            {
                string expression = match.Groups[1].Value;
                object computation = s_table.Compute(expression, string.Empty);
                // Limit the value to 2 decimal points
                return computation?.GetType() == typeof(float) ? ((float)computation).ToString("0.00") : computation?.ToString();
            });
            
            return resultDescription;
        }
    }
}