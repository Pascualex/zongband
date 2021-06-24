#nullable enable

using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

using Zongband.Utils;

namespace ZongbandEditor
{
    public abstract class CustomDrawer : PropertyDrawer
    {
        protected static int RowHeight = 20;

        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var rowPosition = new Rect(position.x, position.y, position.width, RowHeight);
            OnRows(rowPosition, property, label);
            EditorGUI.EndProperty();
        }

        protected abstract void OnRows(Rect position, SerializedProperty property, GUIContent label);

        protected void DrawField(Type type, string name, Rect position, SerializedProperty property, GUIContent? label = null)
        {
            var fieldInfo = type.GetField(name);
            SerializedProperty? field = null;
            if (fieldInfo != null) field = property.FindPropertyRelative(fieldInfo.Name);
            if (fieldInfo != null && field != null)
            {
                if (label == null) EditorGUI.PropertyField(position, field, true);
                else EditorGUI.PropertyField(position, field, label, true);
            }
            else EditorGUI.HelpBox(position, Warnings.Field(name), MessageType.Error);
        }

        protected void DrawSubfields(Type type, string name, Rect position, SerializedProperty property)
        {
            var fieldInfo = type.GetField(name);
            SerializedProperty? field = null;
            if (fieldInfo != null) field = property.FindPropertyRelative(fieldInfo.Name);
            if (fieldInfo != null && field != null)
            {
                foreach (var subfieldInfo in fieldInfo.FieldType.GetFields())
                {
                    DrawField(fieldInfo.FieldType, subfieldInfo.Name, position, field);
                    position.y += RowHeight;
                }
            }
            else EditorGUI.HelpBox(position, Warnings.Field(name), MessageType.Error);
        }

        protected float GetFieldHeight(Type type, string name, SerializedProperty property)
        {
            var fieldInfo = type.GetField(name);
            SerializedProperty? field = null;
            if (fieldInfo != null) field = property.FindPropertyRelative(fieldInfo.Name);
            if (field == null)
            {
                Debug.LogError(Warnings.Field(name));
                return 0;
            }
            return EditorGUI.GetPropertyHeight(field, true);
        }

        protected int GetSubfieldsCount(Type type, string name)
        {
            return type.GetField(name)?.FieldType.GetFields().Length ?? 0;
        }
    }
}
