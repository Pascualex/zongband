#nullable enable

using UnityEngine;
using UnityEditor;
using System;

using Zongband.Game.Abilities;

namespace ZongbandEditor
{
    [CustomPropertyDrawer(typeof(Effect))]
    public class EffectDrawer : CustomDrawer
    {
        private static readonly Type Type = typeof(Effect);

        protected override void OnRows(Rect position, SerializedProperty property, GUIContent label)
        {
            position.x += 5;
            position.width -= 5;
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
            position.x += 5;
            position.width -= 5;

            var controlID = GUIUtility.GetControlID(FocusType.Passive);
            var newLabel = new GUIContent("Effect " + (int.Parse(label.text.Split(' ')[1]) + 1));
            var positionLabel = EditorGUI.PrefixLabel(position, controlID, newLabel);
            DrawField(Type, "Type", positionLabel, property, GUIContent.none);
            position.y += RowHeight;

            if (!property.isExpanded) return;

            EditorGUI.indentLevel += 1;
            var effectType = GetEffectType(property);
            if (effectType == EffectType.Attack)
                DrawSubfields(Type, "AttackPrms", position, property);
            else if (effectType == EffectType.Projectile)
                DrawSubfields(Type, "ProjectilePrms", position, property);
            else if (effectType == EffectType.Sequential || effectType == EffectType.Parallel)
            {
                position.y += 5;
                DrawField(Type, "Effects", position, property);
            }
            EditorGUI.indentLevel -= 1;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded) return RowHeight;
            var effectType = GetEffectType(property);
            if (effectType == EffectType.Attack)
                return (1 + GetSubfieldsCount(Type, "AttackPrms")) * RowHeight;
            else if (effectType == EffectType.Projectile)
                return (1 + GetSubfieldsCount(Type, "ProjectilePrms")) * RowHeight;
            else if (effectType == EffectType.Sequential || effectType == EffectType.Parallel)
                return RowHeight + GetFieldHeight(Type, "Effects", property) + 10;
            else return 1;
        }

        private EffectType GetEffectType(SerializedProperty property)
        {
            var typeField = property.FindPropertyRelative(nameof(Effect.Type));
            if (typeField == null)
            {
                Debug.Log(nameof(Effect.Type));
                return EffectType.Attack;
            }
            return (EffectType)typeField.enumValueIndex;
        }
    }
}
