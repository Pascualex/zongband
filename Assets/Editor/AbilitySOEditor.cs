#nullable enable

using UnityEngine;
using UnityEditor;

using Zongband.Game.Abilities;

namespace ZongbandEditor
{
    // [CustomEditor(typeof(AbilitySO))]
    public class AbilitySOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var AbilitySO = (AbilitySO)target;

            // base.OnInspectorGUI();
            DrawObject("Script", MonoScript.FromScriptableObject(AbilitySO), false);
            DrawField(nameof(AbilitySO.ManaCost));
            DrawField(nameof(AbilitySO.EffectsDefinitions));
        }

        private void DrawObject(string label, Object obj, bool enabled = true)
        {
            GUI.enabled = enabled;
            EditorGUILayout.ObjectField(label, obj, obj.GetType(), false);
            GUI.enabled = true;
        }

        private void DrawField(string path, int indent = 0)
        {
            EditorGUI.indentLevel += indent;
            var property = serializedObject.FindProperty(path);
            if (property != null) EditorGUILayout.PropertyField(property, false);
            else EditorGUILayout.HelpBox($"\"{path}\" property not found", MessageType.Error);
            EditorGUI.indentLevel -= indent;
        }
    }
}
