using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Core.Util;
using Qoqqi.Inspector.Runtime;
using UnityEditor;
using UnityEngine;

namespace Qoqqi.Inspector.Editor {
    [CustomPropertyDrawer(typeof(NullableField))]
    public class NullableFieldDrawer : PropertyDrawer {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            var nullableObject = GetValue(property);
            
            return nullableObject.isNull
                ? EditorGUIUtility.singleLineHeight
                : EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var fieldType = fieldInfo.FieldType;

            if (!typeof(NullableObject).IsAssignableFrom(fieldType)) {
                LogError(fieldType);
                return;
            }

            var toggleRect = GetToggleRect(position);
            var nullableObject = GetValue(property);
            var isNullProperty = property.FindPropertyRelative("isNull");
            var isNull = nullableObject.isNull;
            var isNullLabel = new GUIContent("--\t\tIs Null");

            if (isNull) {
                EditorGUI.LabelField(position, label);
                EditorGUI.PropertyField(toggleRect, isNullProperty, isNullLabel, true);
                return;
            }

            EditorGUI.PropertyField(position, property, label, true);
        }

        [SuppressMessage("ReSharper", "StringIndexOfIsCultureSpecific.1")]
        private NullableObject GetValue(SerializedProperty property) {
            var path = property.propertyPath.Replace(".Array.data[", "[");
            object targetObject = property.serializedObject.targetObject;
            var elements = path.Split('.');

            foreach (var element in elements.Take(elements.Length)) {
                if (element.Contains("[")) {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "")
                            .Replace("]", ""));
                    targetObject = GetValue(targetObject, elementName, index);
                }
                else {
                    targetObject = GetValue(targetObject, element);
                }
            }

            // ReSharper disable once PossibleInvalidCastException
            return (NullableObject) targetObject;
        }

        public object GetValue(object source, string name, int index) {
            var enumerable = GetValue(source, name) as IEnumerable;
            var enumerator = enumerable?.GetEnumerator();
            
            while (index-- >= 0)
                enumerator?.MoveNext();
            
            return enumerator?.Current;
        }

        public object GetValue(object source, string name) {
            if (source == null)
                return null;

            var type = source.GetType();
            var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (f == null) {
                var p = type.GetProperty(name,
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p == null)
                    return null;

                return p.GetValue(source, null);
            }

            return f.GetValue(source);
        }

        private static Rect GetToggleRect(Rect position) {
            var dropdownRect = position;

            dropdownRect.x += EditorGUIUtility.labelWidth + 2;
            dropdownRect.width -= EditorGUIUtility.labelWidth + 2;
            dropdownRect.height = EditorGUIUtility.singleLineHeight;

            return dropdownRect;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private static void LogError(Type type) {
            Debug.LogError(type.Name + " must by subtype of " + nameof(NullableObject));
        }
    }
}