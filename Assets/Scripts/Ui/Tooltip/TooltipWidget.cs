using System.Reflection;
using System.Text.RegularExpressions;
using Sirenix.Utilities;
using UnityEngine;

namespace Ui.Tooltip
{
    /// <summary>
    /// Represents the base class for tooltip data, containing a description string.
    /// </summary>
    public class TooltipData
    { 
        /// <summary>
        /// The description string containing placeholders to be processed.
        /// </summary>
        public string Description { get; protected set; }
    }
    
    public abstract class TooltipWidget : MonoBehaviour
    { 
        /// <summary>
        /// Abstract method to set the text of the tooltip widget.
        /// </summary>
        /// <param name="data">The tooltip data to be displayed.</param>
        public abstract void SetData(TooltipData data);

        /// <summary>
        /// Processes the description string by replacing placeholders with actual property values.
        /// Placeholders are in the format `{ClassName:PropertyName}`.
        /// </summary>
        /// <param name="data">
        /// A class inheriting from the `TooltipData` class containing the data to be processed.
        /// This parameter is expected to include a `Description` string with placeholders in the format `{ClassName:PropertyName}`.
        /// Any additional classes referenced in the placeholders must be accessible as properties of the `TooltipData`-derived class.
        /// </param>
        /// <returns>
        /// The processed description string with placeholders replaced by their corresponding property values.
        /// </returns>
        protected static string GetProcessedString(TooltipData data)
        {
            string resultDescription = data.Description;
            // Replace placeholders in the format {ClassName:PropertyName}
            resultDescription = Regex.Replace(resultDescription, @"\{(?:(\w+):)(\w+)\}", match =>
            {
                string className = match.Groups[1].Value; // Class to look into
                string propertyName = match.Groups[2].Value; // Property to fetch

                if (className.IsNullOrWhitespace() || propertyName.IsNullOrWhitespace())
                {
                    Debug.LogWarning("Invalid placeholder format");
                    return match.Value;
                }
                
                // Get class to look into
                PropertyInfo containerProp = data.GetType().GetProperty(className);
                if (containerProp == null)
                {
                    Debug.LogWarning($"Container '{className}' not found in AbilityTooltipData");
                    return match.Value;
                } 
                object containerValue = containerProp.GetValue(data);
                if (containerValue == null)
                {
                    Debug.LogWarning($"Container '{className}' is null");
                    return match.Value;
                }
                
                // Get property value from the class
                PropertyInfo propertyInfo = containerValue.GetType().GetProperty(propertyName);
                if (propertyInfo == null)
                {
                    Debug.LogWarning($"Property '{propertyName}' not found in container '{className}'");
                    return match.Value;
                }
                
                object value = propertyInfo.GetValue(containerValue);
                return value != null ? value.ToString() : "null";
            });
            
            return resultDescription;
        }
    }
}