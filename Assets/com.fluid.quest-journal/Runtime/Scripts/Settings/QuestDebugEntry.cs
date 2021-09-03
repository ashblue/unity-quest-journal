using CleverCrow.Fluid.QuestJournals.Quests;
using UnityEngine;

namespace CleverCrow.Fluid.QuestJournals {
    [System.Serializable]
    public class QuestDebugEntry {
        [SerializeField]
        private QuestDefinitionBase definition;

        [SerializeField]
        private bool _markComplete;

        public IQuestDefinition Definition => definition;
        public bool MarkComplete => _markComplete;
    }
}
