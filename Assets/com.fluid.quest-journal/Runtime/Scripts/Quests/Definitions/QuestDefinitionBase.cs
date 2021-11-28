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
        [HideInInspector]
        [SerializeField]
        private string _id;

        [TextArea]
        [SerializeField]
        private string _description;

        [HideInInspector]
        [SerializeField]
        public List<TaskDefinitionBase> _tasks = new List<TaskDefinitionBase>();

        public string Id => _id;
        public string Title => name;
        public string Description => _description;
        public IReadOnlyList<ITaskDefinition> Tasks => _tasks;

#if UNITY_EDITOR
        public void SetupEditor () {
            name = "Untitled Quest";
            _id = Guid.NewGuid().ToString();
        }
#endif
    }
}
