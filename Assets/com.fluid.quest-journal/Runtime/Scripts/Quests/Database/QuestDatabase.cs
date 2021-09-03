using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CleverCrow.Fluid.QuestJournals.Quests {
    [CreateAssetMenu(menuName = "Fluid/Quest Journal/Database", fileName = "QuestDatabase")]
    public class QuestDatabase : ScriptableObject, IQuestDatabase {
        private Dictionary<string, IQuestDefinition> _idToQuest;

        [HideInInspector]
        public List<QuestDefinitionBase> questDefinitions = new List<QuestDefinitionBase>();

        public void Setup () {
            _idToQuest = questDefinitions.ToDictionary((quest) => quest.Id, (quest) => quest as IQuestDefinition);
        }

        public IQuestDefinition GetQuest (string id) {
            return _idToQuest[id];
        }
    }
}
