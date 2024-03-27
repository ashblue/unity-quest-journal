using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.QuestJournals.Tasks;
using CleverCrow.Fluid.QuestJournals.Utilities;
using UnityEngine;

namespace CleverCrow.Fluid.QuestJournals.Quests {
    public class QuestCollection : IQuestCollection {
        private readonly UnityEventSafe<IQuestInstance> _eventQuestAdd = new();
        private readonly UnityEventSafe<IQuestInstance> _eventQuestComplete = new();
        private readonly UnityEventSafe<IQuestInstance> _eventQuestUpdate = new();
        private readonly UnityEventSafe<IQuestInstance, ITaskInstanceReadOnly> _eventQuestTaskComplete = new();

        private readonly Dictionary<IQuestDefinition, IQuestInstance> _quests = new();
        private readonly IQuestDatabase _questDatabase;

        public QuestCollection (IQuestDatabase questDatabase) {
            questDatabase.Setup();
            _questDatabase = questDatabase;
        }

        public IUnityEventReadOnly<IQuestInstance> EventQuestAdd => _eventQuestAdd;
        public IUnityEventReadOnly<IQuestInstance> EventQuestComplete => _eventQuestComplete;
        public IUnityEventReadOnly<IQuestInstance> EventQuestUpdate => _eventQuestUpdate;
        public IUnityEventReadOnly<IQuestInstance, ITaskInstanceReadOnly> EventQuestTaskComplete => _eventQuestTaskComplete;

        public IQuestInstance Add (IQuestDefinition definition) {
            var instance = AddInternal(definition);

            Bind(instance);
            _eventQuestAdd.Invoke(instance);

            return instance;
        }

        /// <summary>
        /// Primarily a debugging method. Will not trigger events in the way you might expect. Not production recommended (use Add(IQuestDefinition) instead).
        /// </summary>
        public IQuestInstance Add (ITaskDefinition definition) {
            var quest = AddInternal(definition.Parent);
            quest.SetTask(definition);
            Bind(quest);

            _eventQuestAdd.Invoke(quest);

            return quest;
        }

        IQuestInstance AddInternal (IQuestDefinition definition) {
            var existingResult = Get(definition);
            if (existingResult != null) {
                return existingResult;
            }

            var instance = new QuestInstance(definition);
            _quests[definition] = instance;

            return instance;
        }

        void Bind (IQuestInstance instance) {
            instance.EventComplete.AddListener(_eventQuestComplete.Invoke);
            instance.EventUpdate.AddListener(_eventQuestUpdate.Invoke);
            instance.EventTaskComplete.AddListener(_eventQuestTaskComplete.Invoke);
        }

        void Unbind (IQuestInstance instance) {
            instance.EventComplete.RemoveListener(_eventQuestComplete.Invoke);
            instance.EventUpdate.RemoveListener(_eventQuestUpdate.Invoke);
            instance.EventTaskComplete.RemoveListener(_eventQuestTaskComplete.Invoke);
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
            // Unbind all events just in case
            foreach (var quest in _quests.Values) {
                Unbind(quest);
            }

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
