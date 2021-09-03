using UnityEditor;

namespace CleverCrow.Fluid.QuestJournals.Quests {
    [CustomEditor(typeof(QuestDefinitionBase), true)]
    public class QuestDefinitionInspector : Editor {
        private SortableListTasks _taskList;

        private void OnEnable () {
            var quest = target as QuestDefinitionBase;
            _taskList = new SortableListTasks(this, "_tasks", quest, quest._tasks, "Tasks");
        }

        public override void OnInspectorGUI () {
            target.name = EditorGUILayout.TextField("Title", target.name);
            _taskList.Update();

            // Listed last to prevent textarea pointer from bugging out (Unity bug)
            base.OnInspectorGUI();
        }
    }
}
