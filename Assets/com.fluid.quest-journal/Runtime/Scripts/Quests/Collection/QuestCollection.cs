using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.QuestJournals.Tasks;
using UnityEngine;

namespace CleverCrow.Fluid.QuestJournals.Quests {
    public class QuestCollection : IQuestCollection {
        private readonly Dictionary<IQuestDefinition, IQuestInstance> _quests = new Dictionary<IQuestDefinition, IQuestInstance>();
        private readonly IQuestDatabase _questDatabase;

        public QuestCollection (IQuestDatabase questDatabase) {
            questDatabase.Setup();
            _questDatabase = questDatabase;
        }

        public IQuestInstance Add (IQuestDefinition definition) {
            var existingResult = Get(definition);
            if (existingResult != null) {
                return existingResult;
            }

            var instance = new QuestInstance(definition);
            _quests[definition] = instance;

            return instance;
        }

        public IQuestInstance Add (ITaskDefinition definition) {
            var quest = Add(definition.Parent);
            quest.SetTask(definition);

            return quest;
        }

        public IQuestInstance Get (IQuestDefinition definition) {
            _quests.TryGetValue(definition, out var result);
            return result;
        }

        public IQuestInstance Get (ITaskDefinition definition) {
            return Get(definition.Parent);
        }

        public List<IQuestInstance> GetAll () {
            return _quests.Values.ToList();
        }

        public string Save () {
            var data = new QuestCollectionSave {
                quests = _quests.ToList()
                    .Select(q => new QuestCollectionEntrySave {
                        questId = q.Value.Definition.Id,
                        questSave = q.Value.Save(),
                    })
                    .ToList(),
            };

            return JsonUtility.ToJson(data);
        }

        public void Load (string save) {
            _quests.Clear();

            var data = JsonUtility.FromJson<QuestCollectionSave>(save);
            data.quests.ForEach(quest => {
                var definition = _questDatabase.GetQuest(quest.questId);
                var instance = Add(definition);
                instance.Load(quest.questSave);
            });
        }
    }
}
