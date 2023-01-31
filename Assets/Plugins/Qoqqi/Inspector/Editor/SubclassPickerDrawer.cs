using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Qoqqi.Inspector.Runtime;
using UnityEditor;
using UnityEngine;

namespace Qoqqi.Inspector.Editor {
    [CustomPropertyDrawer(typeof(SubclassPicker))]
    public class SubclassPickerDrawer : PropertyDrawer {
        private bool IsRequired => ((SubclassPicker) attribute).IsRequired;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var fieldType = GetFieldType();
            var selectedTypeName = property.managedReferenceValue?.GetType().Name ?? "Empty";
            var dropdownRect = GetDropdownRect(position);

            if (EditorGUI.DropdownButton(dropdownRect, new GUIContent(selectedTypeName), FocusType.Keyboard)) {
                RenderContextMenu(property, fieldType, selectedTypeName);
            }

            EditorGUI.PropertyField(position, property, label, true);
        }

        private Type GetFieldType() {
            return fieldInfo.FieldType.IsArray
                    ? fieldInfo.FieldType.GetElementType()
                    : fieldInfo.FieldType;
        }

        private static Rect GetDropdownRect(Rect position) {
            var dropdownRect = position;

            dropdownRect.x += EditorGUIUtility.labelWidth + 2;
            dropdownRect.width -= EditorGUIUtility.labelWidth + 2;
            dropdownRect.height = EditorGUIUtility.singleLineHeight;

            return dropdownRect;
        }

        private void RenderContextMenu(SerializedProperty property, Type fieldType, string selectedTypeName) {
            var menu = new GenericMenu();

            if (!IsRequired) {
                AddEmptyOption(property, menu);
            }

            foreach (var type in GetClasses(fieldType)) {
                AddTypeOption(property, menu, type, selectedTypeName);
            }

            menu.ShowAsContext();
        }

        private static void AddEmptyOption(SerializedProperty property, GenericMenu menu) {
            menu.AddItem(new GUIContent("None"), property.managedReferenceValue == null, () => {
                property.managedReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();
            });
        }

        private static void AddTypeOption(
                SerializedProperty property, GenericMenu menu, Type type, string selectedTypeName
        ) {
            menu.AddItem(new GUIContent(type.Name), selectedTypeName == type.Name, () => {
                var constructorInfo = type.GetConstructor(Type.EmptyTypes);

                if (constructorInfo == null) {
                    LogError(type);
                    return;
                }

                property.managedReferenceValue = constructorInfo.Invoke(null);
                property.serializedObject.ApplyModifiedProperties();
            });
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private static void LogError(Type type) {
            Debug.LogError(type.Name + " must have parameterless constructor to be used here");
        }

        private static IEnumerable<Type> GetClasses(Type baseType) {
            return Assembly.GetAssembly(baseType)
                    .GetTypes()
                    .Where(t => IsConstructable(baseType, t));
        }

        private static bool IsConstructable(Type baseType, Type typeToCheck) {
            return typeToCheck.IsClass
                   && !typeToCheck.IsAbstract
                   && baseType.IsAssignableFrom(typeToCheck)
                   && typeToCheck.GetConstructor(Type.EmptyTypes) != null;
        }
    }
}