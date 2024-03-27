using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CleverCrow.Fluid.QuestJournals.Quests {
    public abstract class SortableListBase {
        protected ReorderableList _list;
        protected SerializedProperty _serializedProp;

        public SortableListBase (Editor editor, string property, string title) {
            if (editor == null) {
                Debug.LogError("Editor cannot be null");
                return;
            }

            _serializedProp = editor.serializedObject.FindProperty(property);

            if (_serializedProp == null) {
                Debug.LogErrorFormat("Could not find property {0}", property);
                return;
            }

            _list = new ReorderableList(
                editor.serializedObject,
                _serializedProp,
                true, true, true, true);

            _list.drawHeaderCallback = rect => { EditorGUI.LabelField(rect, title); };
        }

        public void Update () {
            if (_list != null) {
                _list.DoLayoutList();
            }
        }
    }
}
