using System;
using System.Collections.Generic;
using CleverCrow.Fluid.QuestJournals.Tasks;
using CleverCrow.Fluid.QuestJournals.Utilities;
using UnityEngine;

namespace CleverCrow.Fluid.QuestJournals.Quests {
    public abstract class QuestDefinitionBase :
        ScriptableObject,
#if UNITY_EDITOR
        IQuestDefinition,
        ISetupEditor
#else
        IQuestDefinition
#endif
    {
        [SerializeField]
        private string _displayName = "Untitled Quest";

        [Tooltip("Unique identifier for the quest definition. This is used to reference the quest in the database for save and load operations")]
        [SerializeField]
        private string _id;

        [TextArea]
        [SerializeField]
        private string _description;

        [HideInInspector]
        [SerializeField]
        public List<TaskDefinitionBase> _tasks = new();

        public string Id => _id;
        public string DisplayName => _displayName;
        public string Description => _description;
        public IReadOnlyList<ITaskDefinition> Tasks => _tasks;

#if UNITY_EDITOR
        public void SetupEditor (string displayName) {
            _id = Guid.NewGuid().ToString();
            _displayName = displayName;
        }
#endif
    }
}
