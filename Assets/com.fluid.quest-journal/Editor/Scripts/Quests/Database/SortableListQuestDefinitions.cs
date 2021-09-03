using System;
using System.Linq;
using Adnc.Utility.Editors;
using CleverCrow.Fluid.QuestJournals.Editors.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CleverCrow.Fluid.QuestJournals.Quests {
    public class SortableListQuestDefinitions : SortableListBase {
        private static TypesToMenu<QuestDefinitionBase> _questTypes;
        private static TypesToMenu<QuestDefinitionBase> QuestTypes =>
            _questTypes ??= new TypesToMenu<QuestDefinitionBase>();

        public SortableListQuestDefinitions (Editor editor, string property, string title) : base(editor, property, title) {
            _editor = editor;

            _list.drawElementCallback = (rect, index, active, focused) => {
                var element = _serializedProp.GetArrayElementAtIndex(index);

                GUI.enabled = false;
                EditorGUI.ObjectField(rect, element, GUIContent.none);
                GUI.enabled = true;
            };

            _list.onAddDropdownCallback = ShowMenu;
            _list.onRemoveCallback = DeleteQuest;
        }

        private void ShowMenu (Rect buttonRect, ReorderableList list) {
            var menu = new GenericMenu();

            foreach (var line in QuestTypes.Lines) {
                menu.AddItem(
                    new GUIContent(line.path),
                    false,
                    () => CreateQuest(line.type));
            }

            menu.ShowAsContext();
        }

        private void CreateQuest (Type type) {
            var database = _editor.target as QuestDatabase;
            var databasePath = AssetDatabase.GetAssetPath(database);
            var scriptableObjectParent = AssetDatabase.LoadAssetAtPath<ScriptableObject>(databasePath);

            var listItem = ScriptableObject.CreateInstance(type) as QuestDefinitionBase;
            Debug.Assert(listItem != null, $"Failed to create new QuestDefinition");
            listItem.SetupEditor();

            Undo.SetCurrentGroupName("Add Quest");

            Undo.RecordObject(scriptableObjectParent, "Add Quest");
            Undo.RecordObject(database, "Add Quest");

            database.questDefinitions.Add(listItem);
            AssetDatabase.AddObjectToAsset(listItem, scriptableObjectParent);
            Undo.RegisterCreatedObjectUndo(listItem, "Add Quest");

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
            AssetDatabase.SaveAssets();
        }

        private void DeleteQuest (ReorderableList list) {
            var target = _editor.target as QuestDatabase;
            var targetPath = AssetDatabase.GetAssetPath(target);
            var database = AssetDatabase.LoadAssetAtPath<QuestDatabase>(targetPath);
            var quest = database.questDefinitions[list.index];
            var tasks = quest._tasks.ToList();

            Undo.SetCurrentGroupName("Delete Quest");

            tasks.ForEach(t => Undo.RecordObject(t, "Delete Quest"));
            Undo.RecordObject(quest, "Delete Quest");
            Undo.RecordObject(database, "Delete Quest");

            database.questDefinitions.Remove(quest);
            Undo.DestroyObjectImmediate(quest);
            foreach (var t in tasks) {
                Undo.DestroyObjectImmediate(t);
            }

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
            AssetDatabase.SaveAssets();
        }
    }
}
