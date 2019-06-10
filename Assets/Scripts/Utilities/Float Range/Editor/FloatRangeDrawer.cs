using UnityEditor;
using UnityEngine;
using Core;

[CustomPropertyDrawer(typeof(FloatRange))]
public class FloatRangeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        position.width /= 2f;
        float labelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = position.width / 2.5f;

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;

        EditorGUI.PropertyField(position, property.FindPropertyRelative("min"));

        position.x += position.width;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("max"));

        EditorGUIUtility.labelWidth = labelWidth;
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
