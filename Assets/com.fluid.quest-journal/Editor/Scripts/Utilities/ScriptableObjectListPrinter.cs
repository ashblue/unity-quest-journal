using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CleverCrow.Fluid.QuestJournals.Editors.Utilities {
    public class ScriptableObjectListPrinter {
        private readonly SerializedProperty _serializedProp;
        private const float _textareaHeight = 50;
        private readonly Dictionary<string, bool> _textareaFields = new Dictionary<string, bool>();

        private readonly HashSet<string> _variableBlacklist = new HashSet<string> {
            "m_Script",
            "_id",
            "_parent",
        };

        private Action<Rect, float> _onPropertyPrint;

        public ScriptableObjectListPrinter (SerializedProperty serializedProp) {
            _serializedProp = serializedProp;
        }

        public void DrawScriptableObject (Rect rect, int index, bool active, bool focused) {
            var totalHeight = 0f;

            var element = _serializedProp.GetArrayElementAtIndex(index);
            if (element.objectReferenceValue == null) {
                Debug.LogWarning($"Null element detected in sortable list {element.name}");
                return;
            }

            var serializedObject = new SerializedObject(element.objectReferenceValue);
            var propIterator = serializedObject.GetIterator();

            EditorGUI.BeginChangeCheck();
            totalHeight += PrintObject(rect, element.objectReferenceValue);

            var titlePosition = new Rect(rect);
            titlePosition.y += totalHeight;
            totalHeight += PrintTitle(titlePosition, serializedObject);

            while (propIterator.NextVisible(true)) {
                if (_variableBlacklist.Contains(propIterator.name)) continue;

                var position = new Rect(rect);
                position.y += totalHeight;

                if (RegisterTextareaAttributes(propIterator)) {
                    totalHeight = PrintTextArea(position, propIterator, totalHeight);
                } else {
                    EditorGUI.PropertyField(position, propIterator, true);
                    var height = EditorGUI.GetPropertyHeight(propIterator);
                    totalHeight += height;
                }

                totalHeight += EditorGUIUtility.standardVerticalSpacing;
            }
            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
        }

        private static float PrintTitle (Rect rect, SerializedObject serializedObject) {
            var titleHeight = EditorStyles.label.CalcHeight(new GUIContent(serializedObject.targetObject.name), rect.width) +
                              EditorGUIUtility.standardVerticalSpacing;
            var titlePosition = new Rect(rect) { height = titleHeight };
            serializedObject.targetObject.name =
                EditorGUI.TextField(titlePosition, "Title", serializedObject.targetObject.name);

            return titleHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        private static float PrintObject (Rect rect, Object obj) {
            var height = EditorStyles.objectField.CalcHeight(new GUIContent(obj.name), rect.width);
            var position = new Rect(rect) { height = height };
            GUI.enabled = false;
            EditorGUI.ObjectField(position, obj, typeof(ScriptableObject), false);
            GUI.enabled = true;

            return height + EditorGUIUtility.standardVerticalSpacing;
        }

        public float GetHeight (int index) {
            var totalHeight = 0f;

            var element = _serializedProp.GetArrayElementAtIndex(index);
            if (element.objectReferenceValue == null) {
                Debug.LogWarning($"Null element detected in sortable list {element.name}");
                return 0;
            }

            var propIterator = new SerializedObject(element.objectReferenceValue).GetIterator();

            var objHeight = EditorStyles.objectField.CalcHeight(new GUIContent(element.objectReferenceValue.name), Mathf.Infinity);
            totalHeight += objHeight + EditorGUIUtility.standardVerticalSpacing;

            var titleHeight = EditorStyles.label.CalcHeight(new GUIContent(element.objectReferenceValue.name), Mathf.Infinity);
            totalHeight += titleHeight + EditorGUIUtility.standardVerticalSpacing;

            while (propIterator.NextVisible(true)) {
                if (_variableBlacklist.Contains(propIterator.name)) continue;

                if (RegisterTextareaAttributes(propIterator)) {
                    var labelHeight = EditorStyles.label.CalcHeight(new GUIContent(propIterator.displayName), Mathf.Infinity);
                    totalHeight += _textareaHeight + labelHeight + EditorGUIUtility.standardVerticalSpacing;
                } else {
                    totalHeight += EditorGUI.GetPropertyHeight(propIterator);
                }

                totalHeight += EditorGUIUtility.standardVerticalSpacing;
            }

            return totalHeight;
        }

        private bool RegisterTextareaAttributes (SerializedProperty prop) {
            var type = prop.serializedObject.targetObject.GetType();
            var propId = $"{type.FullName}_{prop.name}";
            if (_textareaFields.ContainsKey(propId)) { return _textareaFields[propId]; }

            WriteTextAreaKey(prop, propId, type);
            if (type.BaseType != null) {
                WriteTextAreaKey(prop, propId, type.BaseType);
            }

            return _textareaFields[propId];
        }

        private void WriteTextAreaKey (SerializedProperty prop, string propId, Type type) {
            if (_textareaFields.TryGetValue(propId, out var currentValue) && currentValue) return;
            var field = type.GetField(prop.name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (field == null) {
                _textareaFields[propId] = false;
                return;
            }

            var textArea = field.GetCustomAttribute<TextAreaAttribute>();
            if (textArea == null) {
                _textareaFields[propId] = false;
                return;
            }

            _textareaFields[propId] = true;
        }

        private float PrintTextArea (Rect position, SerializedProperty propIterator, float totalHeight) {
            var labelHeight = EditorStyles.label.CalcHeight(new GUIContent(propIterator.displayName), position.width);
            GUI.Label(position, propIterator.displayName, new GUIStyle(EditorStyles.wordWrappedLabel));
            totalHeight += labelHeight + EditorGUIUtility.standardVerticalSpacing;

            var textareaStyle = new GUIStyle(EditorStyles.textArea) { wordWrap = true };
            var textAreaPosition = new Rect(position) { height = _textareaHeight };
            textAreaPosition.y += labelHeight + + EditorGUIUtility.standardVerticalSpacing;
            propIterator.stringValue = GUI.TextArea(textAreaPosition, propIterator.stringValue, textareaStyle);
            totalHeight += textAreaPosition.height;

            return totalHeight;
        }
    }
}
