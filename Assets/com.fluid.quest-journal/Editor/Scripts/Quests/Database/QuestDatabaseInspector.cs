using UnityEditor;

namespace CleverCrow.Fluid.QuestJournals.Quests {
    [CustomEditor(typeof(QuestDatabase))]
    public class QuestDatabaseInspector : Editor {
        private SortableListQuestDefinitions _itemList;

        private void OnEnable () {
            _itemList = new SortableListQuestDefinitions(this, "questDefinitions", "Definitions");
        }

        public override void OnInspectorGUI () {
            base.OnInspectorGUI();
            _itemList.Update();
        }
    }
}
