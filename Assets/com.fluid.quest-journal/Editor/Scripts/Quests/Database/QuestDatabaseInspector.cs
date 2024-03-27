using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CleverCrow.Fluid.QuestJournals.Editors.Utilities;
using CleverCrow.Fluid.QuestJournals.Tasks;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CleverCrow.Fluid.QuestJournals.Quests {
    [CustomEditor(typeof(QuestDatabase))]
    public class QuestDatabaseInspector : Editor {
        ReorderableList _reorderableList;

        void OnEnable () {
            _reorderableList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("_definitions"),
                true, true, true, true);

            _reorderableList.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Quest Definitions"); };

            _reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                    element, GUIContent.none);
            };

            _reorderableList.onAddCallback = (list) => {
                var menu = new GenericMenu();
                var types = GetQuestTypes();

                foreach (var line in types.Lines) {
                    menu.AddItem(
                        new GUIContent(line.path),
                        false,
                        () => CreateQuest(line.type));
                }

                menu.ShowAsContext();
            };
        }

        public override void OnInspectorGUI () {
            base.OnInspectorGUI();

            serializedObject.Update();
            _reorderableList.DoLayoutList();

            var database = target as QuestDatabase;
            if (GUILayout.Button("Repair IDs")) {
                RepairDuplicateIds();
            }

            if (GUILayout.Button("Sync")) {
                SyncAll(database);
            }

            serializedObject.ApplyModifiedProperties();
        }

        static void SyncAll (QuestDatabase database) {
            Undo.RecordObject(database, "Sync Quests");

            var assets = AssetDatabase.FindAssets("t:QuestDefinitionBase");

            database._definitions.Clear();
            foreach (var asset in assets) {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                var definition = AssetDatabase.LoadAssetAtPath<QuestDefinitionBase>(path);
                database._definitions.Add(definition);
            }

            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();
        }

        void RepairDuplicateIds () {
            if (!EditorUtility.DisplayDialog(
                    "Confirm Fix IDs",
                    "Are you sure you want to fix duplicate item definition IDs? This will randomize the ID of any duplicates found and print the details to the console. This may affect save data as these IDs are used to restore saves. It is highly recommended you backup your project before proceeding as this cannot be undone.",
                    "Yes",
                    "Cancel"
                )) return;

            var count = 0;
            var ids = new HashSet<string>();

            // Loop over all quests and tasks in the database
            var quests = AssetDatabase.FindAssets("t:QuestDefinitionBase");
            var tasks = AssetDatabase.FindAssets("t:TaskDefinitionBase");

            Debug.Log("BEGIN: Fixing duplicate definition IDs.");
            foreach (var asset in quests.Concat(tasks)) {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                var definition = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

                // If the definition is a quest, check the ID
                if (definition is QuestDefinitionBase quest) {
                    if (ids.Contains(quest.Id)) {
                        var newId = Guid.NewGuid().ToString();
                        Debug.LogWarning($"Duplicate quest ID {quest.Id} in {path}. Randomizing the ID to {newId}.");

                        var so = new SerializedObject(definition);
                        var idField = so.FindProperty("_id");
                        idField.stringValue = newId;

                        so.ApplyModifiedProperties();
                        EditorUtility.SetDirty(definition);

                        count++;
                    } else {
                        ids.Add(quest.Id);
                    }
                }

                // If the definition is a task, check the ID
                if (definition is TaskDefinitionBase task) {
                    if (ids.Contains(task.Id)) {
                        var newId = Guid.NewGuid().ToString();
                        Debug.LogWarning($"Duplicate task ID {task.Id} in {path}. Randomizing the ID to {newId}.");

                        var so = new SerializedObject(definition);
                        var idField = so.FindProperty("_id");
                        idField.stringValue = newId;

                        so.ApplyModifiedProperties();
                        EditorUtility.SetDirty(definition);

                        count++;
                    } else {
                        ids.Add(task.Id);
                    }
                }
            }

            if (count > 0) {
                Debug.Log($"Fixed {count} duplicate definition ID(s).");
            } else {
                Debug.Log("No duplicate definition IDs found. Nothing to fix.");
            }

            Debug.Log("END: Fixing duplicate definition IDs.");
        }

        private void CreateQuest (Type type) {
            var defaultPath = GetActiveProjectPath();

            // Give the user a file save dialog for the new quest
            var path = EditorUtility.SaveFilePanelInProject("Save Quest", "Quest", "asset", "Create your new quest",
                defaultPath);
            if (path.Length != 0) {
                // Generate the new SO
                var pathFileName = path.Substring(path.LastIndexOf("/") + 1);
                pathFileName = pathFileName.Substring(0, pathFileName.LastIndexOf("."));
                var asset = CreateInstance(type) as QuestDefinitionBase;
                asset.SetupEditor(InsertSpaces(pathFileName));

                // Save the SO with an undo operation
                Undo.RecordObject(target, "Add Quest");
                AssetDatabase.CreateAsset(asset, path);

                // Add this item to the reorderable list
                var index = _reorderableList.serializedProperty.arraySize;
                _reorderableList.serializedProperty.arraySize++;
                var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                element.objectReferenceValue = asset;

                _reorderableList.serializedProperty.serializedObject.ApplyModifiedProperties();

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        string GetActiveProjectPath () {
            if (Selection.activeObject == null) {
                var databasePath = AssetDatabase.GetAssetPath(target);
                return databasePath.Substring(0, databasePath.LastIndexOf("/"));
            }

            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (AssetDatabase.IsValidFolder(path)) {
                return path;
            }

            return path.Substring(0, path.LastIndexOf("/"));
        }

        TypesToMenu<QuestDefinitionBase> GetQuestTypes () {
            return new TypesToMenu<QuestDefinitionBase>();
        }

        static string InsertSpaces (string originalString) {
            if (string.IsNullOrEmpty(originalString))
                return string.Empty;

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(originalString[0]);

            for (int i = 1; i < originalString.Length; i++) {
                if (char.IsUpper(originalString[i]))
                    stringBuilder.Append(' ');
                stringBuilder.Append(originalString[i]);
            }

            return stringBuilder.ToString();
        }
    }
}
