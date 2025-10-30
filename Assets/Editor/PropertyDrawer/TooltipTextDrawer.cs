using System.Text.RegularExpressions;
using EditorAttributes;
using UnityEditor;
using UnityEngine;

namespace Editor.PropertyDrawer
{
    [CustomPropertyDrawer(typeof(TooltipTextAreaAttribute))]
    public class TooltipTextDrawer : UnityEditor.PropertyDrawer
    {
        private static readonly Regex s_classPlaceholderRegex = new Regex(@"\{([^}]*)\}", RegexOptions.Compiled);
        private static readonly Regex s_tooltipDataPlaceholderRegex = new Regex(@"\[([^}]*)\]", RegexOptions.Compiled);
        private static readonly string s_invalidTypeMessage = L10n.Tr("<color=red>Error: Use TooltipTextDrawer with string.</color>");
        private const string TEXT_CONTROL_NAME = "TooltipTextArea";
        private const string CLASS_PLACEHOLDER_COLOR = "cyan";
        private const string TOOLTIP_DATA_PLACEHOLDER_COLOR = "yellow";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginProperty(position, label, property);

                // Draw label
                EditorGUI.LabelField(
                    new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                    label
                );
                // Text area rect
                Rect textAreaRect = new Rect(
                    position.x,
                    position.y + EditorGUIUtility.singleLineHeight + 2,
                    position.width,
                    position.height - EditorGUIUtility.singleLineHeight - 2
                );

                // Replace placeholders with colored versions for display
                string rawText = property.stringValue;
                string coloredText = s_classPlaceholderRegex.Replace(rawText, $"<color="+CLASS_PLACEHOLDER_COLOR+">{$1}</color>");
                coloredText = s_tooltipDataPlaceholderRegex.Replace(coloredText, $"<color="+TOOLTIP_DATA_PLACEHOLDER_COLOR+">[$1]</color>");

                GUIStyle style = new GUIStyle(EditorStyles.textArea)
                {
                    richText = true,
                    wordWrap = true
                };

                // Determine if we're currently editing
                bool isFocused = GUI.GetNameOfFocusedControl() == TEXT_CONTROL_NAME;

                // Detect mouse click to enter edit mode
                if (!isFocused && Event.current.type == EventType.MouseDown && textAreaRect.Contains(Event.current.mousePosition))
                {
                    GUI.FocusControl(TEXT_CONTROL_NAME);
                    Event.current.Use();
                }

                // Draw the box background
                GUI.Box(textAreaRect, GUIContent.none);

                GUI.SetNextControlName(TEXT_CONTROL_NAME);
                if (isFocused)
                {
                    // EDIT MODE: draw normal text area
                    string newText = EditorGUI.TextArea(textAreaRect, rawText, EditorStyles.textArea);
                    if (newText != rawText)
                    {
                        property.stringValue = newText;
                    }
                }
                else
                {
                    // VIEW MODE: draw colored version (not editable)
                    EditorGUI.SelectableLabel(textAreaRect, coloredText, style);
                }

                EditorGUI.EndProperty();
            }
            else
            {
                // Error handling for non-string properties
                // Create style for rich text
                GUIStyle style = new GUIStyle(EditorStyles.textArea)
                {
                    richText = true,
                    fontSize = 20,
                    wordWrap = true,
                    alignment = TextAnchor.MiddleCenter,
                    
                };
                EditorGUI.LabelField(position, label.text, s_invalidTypeMessage, style);
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            TooltipTextAreaAttribute tooltipTextAreaAttribute = attribute as TooltipTextAreaAttribute;
            return EditorGUIUtility.singleLineHeight * tooltipTextAreaAttribute.Height; // Adjust height
        }
    }
}