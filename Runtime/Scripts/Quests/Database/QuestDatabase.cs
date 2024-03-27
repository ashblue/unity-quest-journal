using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CleverCrow.Fluid.QuestJournals.Quests {
    [CreateAssetMenu(menuName = "Fluid/Quest Journal/Database", fileName = "QuestDatabase")]
    public class QuestDatabase : ScriptableObject, IQuestDatabase {
        private Dictionary<string, IQuestDefinition> _idToQuest;

        [HideInInspector]
        [SerializeField]
        public List<QuestDefinitionBase> _definitions = new();

        public void Setup () {
            _idToQuest = _definitions.ToDictionary((quest) => quest.Id, (quest) => quest as IQuestDefinition);
        }

        public IQuestDefinition GetQuest (string id) {
            return _idToQuest[id];
        }
    }
}
