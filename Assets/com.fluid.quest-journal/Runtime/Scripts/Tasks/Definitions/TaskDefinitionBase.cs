using System;
using UnityEngine;
using Adnc.Utility;
using CleverCrow.Fluid.QuestJournals.Quests;
using CleverCrow.Fluid.QuestJournals.Utilities;

namespace CleverCrow.Fluid.QuestJournals.Tasks {
    public abstract class TaskDefinitionBase :
        ScriptableObject,
#if UNITY_EDITOR
        ITaskDefinition,
        ISetupEditor
#else
        ITaskDefinition
#endif
    {
        [ReadOnly]
        [SerializeField]
        private string _id;

        [ReadOnly]
        [SerializeField]
        private QuestDefinitionBase _parent;

        [TextArea]
        [SerializeField]
        private string _description;

        public string Id => _id;
        public string Title => name;
        public string Description => _description;
        public IQuestDefinition Parent => _parent;

#if UNITY_EDITOR
        public void SetupEditor () {
            name = "Untitled Task";
            _id = Guid.NewGuid().ToString();
        }

        public void SetParent (QuestDefinitionBase parent) {
            _parent = parent;
        }
#endif
    }
}
