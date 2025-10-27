using System.Reflection;
using System.Text.RegularExpressions;
using Sirenix.Utilities;
using UnityEngine;

namespace Ui.Tooltip
{
    /// <summary>
    /// Represents the base class for tooltip data
    /// </summary>
    public class TooltipData
    {
    }

    public abstract class TooltipWidget : MonoBehaviour
    {
        /// <summary>
        /// Abstract method to set the text of the tooltip widget.
        /// </summary>
        /// <param name="data">The tooltip data to be displayed.</param>
        public abstract void SetData(TooltipData data);

        /// <summary>
        /// Formats a string by replacing placeholders with actual property values.
        /// Placeholders are in the format `{ClassName:PropertyName}`.
        /// </summary>
        /// <param name="data">
        /// An instance of a class inheriting from `TooltipData` that contains the data to be processed.
        /// The class should expose properties that match the placeholders in the description string.
        /// </param>
        /// <param name="stringToFormat">
        /// The string containing placeholders to be replaced with actual values.
        /// </param>
        /// <returns>
        /// A string where placeholders in the stringToFormat have been replaced with their corresponding property values.
        /// </returns>
        protected static string FormatTooltipDescription(TooltipData data, string stringToFormat)
        {
            string resultDescription = stringToFormat;
            // Replace placeholders in the format {PropertyName.PropertyName...}
            resultDescription = Regex.Replace(resultDescription, @"\{((?:\w+(?:\.\w+)*))\}", match =>
            {
                string propertyPath = match.Groups[1].Value; // Full property path to look into
                if (propertyPath.IsNullOrWhitespace())
                {
                    Debug.LogWarning("Invalid placeholder format");
                    return match.Value;
                }

                string[] pathParts = propertyPath.Split('.');
                object containerValue = data;

                foreach (string part in pathParts)
                {
                    FieldInfo fieldInfo = containerValue.GetType().GetField(part, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (fieldInfo != null)
                    {
                        containerValue = fieldInfo.GetValue(containerValue);
                    }
                    else
                    {
                        PropertyInfo propertyInfo = containerValue.GetType().GetProperty(part);
                        if (propertyInfo == null)
                        {
                            Debug.LogWarning($"Member '{part}' not found in '{containerValue.GetType().Name}'");
                            return match.Value;
                        }

                        containerValue = propertyInfo.GetValue(containerValue);
                        if (containerValue == null)
                        {
                            Debug.LogWarning($"Member '{part}' is null in '{propertyInfo.GetType().Name}'");
                            return match.Value;
                        }
                    }
                }

                return containerValue != null ? containerValue.ToString() : "null";
            });

            return resultDescription;
        }
    }
}