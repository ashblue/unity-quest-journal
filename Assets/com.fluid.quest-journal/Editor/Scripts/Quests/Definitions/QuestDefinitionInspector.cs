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
            base.OnInspectorGUI();

            serializedObject.Update();
            _taskList.Update();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
