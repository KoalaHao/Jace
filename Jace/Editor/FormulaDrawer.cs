using Jace.Runtime;
using UnityEditor;
using UnityEngine;
using Veewo.Framework.UnityExtensions;

namespace JaceEditor
{
    [CustomPropertyDrawer(typeof(Formula))]
    public class FormulaDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var originalColor = GUI.color;
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var fieldRect = position;
            fieldRect.width -= 30;

            if (property.serializedObject.targetObject is IFormulaDomainContainer container)
            {
                var formula = EditorHelper.GetTargetObjectOfProperty(property) as Formula;
                if (container!=null && !container.Domain.IsValid(formula))
                {
                    GUI.color = Color.red;
                }
            }
            
            
            EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("expression"), GUIContent.none);
            GUI.color = originalColor;

            var buttonRect = position;
            buttonRect.width = 30;
            buttonRect.x += fieldRect.width;
            EditorGUI.PropertyField(buttonRect, property.FindPropertyRelative("fallback"), GUIContent.none);
            
            EditorGUI.EndProperty();
        }
    }
}