using UnityEditor;
using UnityEngine;
using Core;


[CustomPropertyDrawer(typeof(FloatRangeSliderAttribute))]
public class FloatRangeSliderDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        int indent = EditorGUI.indentLevel;

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        EditorGUI.indentLevel = 0;

        SerializedProperty minProperty = property.FindPropertyRelative("min");
        SerializedProperty maxProperty = property.FindPropertyRelative("max");
        float minValue = minProperty.floatValue;
        float maxValue = maxProperty.floatValue;

        float fieldWidth = position.width / 5f - 4f;
        float sliderWidth = position.width / 1.7f;
        position.width = fieldWidth;

        minValue = EditorGUI.FloatField(position, minValue);

        position.x += fieldWidth + 4f;
        position.width = sliderWidth;

        FloatRangeSliderAttribute limit = attribute as FloatRangeSliderAttribute;
        EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, limit.Min, limit.Max);

        position.x += sliderWidth + 4f;
        position.width = fieldWidth;

        maxValue = EditorGUI.FloatField(position, maxValue);

        minValue = Mathf.Min(minValue, maxValue);
        minValue = Mathf.Max(minValue, limit.Min);

        maxValue = Mathf.Max(minValue, maxValue);
        maxValue = Mathf.Min(maxValue, limit.Max);

        minProperty.floatValue = minValue;
        maxProperty.floatValue = maxValue;

        EditorGUI.EndProperty();
        EditorGUI.indentLevel = indent;
    }
}
