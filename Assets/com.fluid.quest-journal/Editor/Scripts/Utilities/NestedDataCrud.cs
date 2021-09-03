using System;
using System.Collections.Generic;
using CleverCrow.Fluid.QuestJournals.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CleverCrow.Fluid.QuestJournals.Editors.Utilities {
    public class NestedDataCrud<T> where T : Object, ISetupEditor {
        private readonly ScriptableObject _parent;
        private readonly List<T> _list;
        private readonly TypesToMenu<T> _menuData;
        private Action<T> _onCreateCallback;

        public NestedDataCrud (ScriptableObject parent, List<T> list, TypesToMenu<T> menuData) {
            _menuData = menuData;
            _parent = parent;
            _list = list;
        }

        public void ShowMenu (Rect buttonRect, ReorderableList list) {
            var menu = new GenericMenu();

            foreach (var line in _menuData.Lines) {
                menu.AddItem(
                    new GUIContent(line.path),
                    false,
                    () => CreateItem(line.type));
            }

            menu.ShowAsContext();
        }

        public void BindOnCreate (Action<T> callback) {
            _onCreateCallback = callback;
        }

        private void CreateItem (Type type) {
            var graphPath = AssetDatabase.GetAssetPath(_parent);
            var graph = AssetDatabase.LoadAssetAtPath<ScriptableObject>(graphPath);

            var listItem = ScriptableObject.CreateInstance(type) as T;
            Debug.Assert(listItem != null, $"Failed to create new type {type}");
            listItem.SetupEditor();
            _onCreateCallback?.Invoke(listItem);

            Undo.SetCurrentGroupName("Add type");

            Undo.RecordObject(graph, "Add type");
            Undo.RecordObject(_parent, "Add type");

            _list.Add(listItem);
            AssetDatabase.AddObjectToAsset(listItem, graph);
            Undo.RegisterCreatedObjectUndo(listItem, "Add type");

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
            AssetDatabase.SaveAssets();
        }

        public void DeleteItem (ReorderableList list) {
            var graphPath = AssetDatabase.GetAssetPath(_parent);
            var graph = AssetDatabase.LoadAssetAtPath<ScriptableObject>(graphPath);
            var listItem = _list[list.index];

            Undo.SetCurrentGroupName("Delete type");

            Undo.RecordObject(graph, "Delete type");
            Undo.RecordObject(_parent, "Delete type");

            _list.Remove(listItem);
            Undo.DestroyObjectImmediate(listItem);

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
            AssetDatabase.SaveAssets();
        }
    }
}
