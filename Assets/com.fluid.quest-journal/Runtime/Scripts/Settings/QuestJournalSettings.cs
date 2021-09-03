using System.Collections.Generic;
using CleverCrow.Fluid.QuestJournals.Quests;
using CleverCrow.Fluid.QuestJournals.Tasks;
using CleverCrow.Fluid.SimpleSettings;
using UnityEngine;

namespace CleverCrow.Fluid.QuestJournals {
    [CreateAssetMenu(fileName = "QuestJournalSettings", menuName = "Fluid/Quest Journal/Settings")]
    public class QuestJournalSettings : SettingsBase<QuestJournalSettings> {
        [SerializeField]
        private QuestDatabase _database;

        [SerializeField]
        private List<QuestDefinitionBase> _startingQuests;

        [Header("Debug")]

        [Tooltip("Quests started automatically while using the Unity editor mode (excluded from runtime)")]
        [SerializeField]
        private List<QuestDebugEntry> _debugQuests;

        [Tooltip("Automatically starts the corresponding quest and sets the task position while using the editor")]
        [SerializeField]
        private List<TaskDefinition> _debugTasks;

        public IQuestDatabase Database => _database;
        public List<QuestDefinitionBase> StartingQuests => _startingQuests;
        public List<QuestDebugEntry> DebugQuests => _debugQuests;
        public List<TaskDefinition> DebugTasks => _debugTasks;
    }
}
