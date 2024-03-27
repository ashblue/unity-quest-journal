using System.Collections.Generic;
using CleverCrow.Fluid.QuestJournals.Quests;
using CleverCrow.Fluid.QuestJournals.Tasks;
using CleverCrow.Fluid.SimpleSettings;
using UnityEngine;

namespace CleverCrow.Fluid.QuestJournals {
    [CreateAssetMenu(fileName = "QuestJournalSettings", menuName = "Fluid/Quest Journal/Settings")]
    public class QuestJournalSettings : SettingsBase<QuestJournalSettings> {
        [Tooltip("Link your database here so the journal assets can be globally accessed")]
        [SerializeField]
        private QuestDatabase _database;

        [Tooltip("Quests that will be automatically injected when the game begins. Overwritten on save and load")]
        [SerializeField]
        private List<QuestDefinitionBase> _startingQuests;

        [Tooltip("Hide the default quest definition. Useful if you have your own custom quest definition class and don't want to see the default one in the inspector")]
        [SerializeField]
        bool _hideDefaultQuestDefinition;

        [Tooltip("Hide the default task definition. Useful if you have your own custom task definition class and don't want to see the default one in the inspector")]
        [SerializeField]
        bool _hideDefaultTaskDefinition;

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
        public bool HideDefaultQuestDefinition => _hideDefaultQuestDefinition;
        public bool HideDefaultTaskDefinition => _hideDefaultTaskDefinition;
    }
}
