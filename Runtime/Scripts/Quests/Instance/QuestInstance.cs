using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.QuestJournals.Tasks;
using CleverCrow.Fluid.QuestJournals.Utilities;
using UnityEngine;

namespace CleverCrow.Fluid.QuestJournals.Quests {
    public class QuestInstance : IQuestInstance {
        private readonly List<ITaskInstance> _tasks = new();

        readonly IUnityEventSafe<IQuestInstance> _eventComplete = new UnityEventSafe<IQuestInstance>();
        readonly IUnityEventSafe<IQuestInstance> _eventUpdate = new UnityEventSafe<IQuestInstance>();
        readonly IUnityEventSafe<IQuestInstance, ITaskInstanceReadOnly> _eventTaskComplete = new UnityEventSafe<IQuestInstance, ITaskInstanceReadOnly>();

        private int _taskIndex;

        public IQuestDefinition Definition { get; }
        public string Title => Definition.DisplayName;
        public string Description => Definition.Description;
        public IReadOnlyList<ITaskInstanceReadOnly> Tasks => _tasks;
        public QuestStatus Status => _taskIndex >= _tasks.Count ? QuestStatus.Complete : QuestStatus.Ongoing;

        public IUnityEventReadOnly<IQuestInstance> EventComplete => _eventComplete;
        public IUnityEventReadOnly<IQuestInstance> EventUpdate => _eventUpdate;
        public IUnityEventReadOnly<IQuestInstance, ITaskInstanceReadOnly> EventTaskComplete => _eventTaskComplete;

        public ITaskInstanceReadOnly ActiveTask {
            get {
                if (_tasks.Count == 0) return null;
                return Status == QuestStatus.Complete ? _tasks[_tasks.Count - 1] : _tasks[_taskIndex];
            }
        }

        ITaskInstance ActiveTaskInternal => ActiveTask as ITaskInstance;

        public QuestInstance (IQuestDefinition definition) {
            Definition = definition;
            PopulateTasks(definition.Tasks);
        }

        /// <summary>
        /// Primarily a debugging method. Not recommended in production. Call Next() instead
        /// </summary>
        public void SetTask (ITaskDefinition task) {
            _taskIndex = _tasks.FindIndex((t) => t.Definition == task);

            for (var i = 0; i < Tasks.Count; i++) {
                if (_taskIndex == i) {
                    ActiveTaskInternal.Begin();
                    continue;
                }

                if (i < _taskIndex) {
                    _tasks[i].Complete();
                    continue;
                }

                _tasks[i].ClearStatus();
            }

            _eventUpdate.Invoke(this);
        }

        public void Next () {
            if (Status == QuestStatus.Complete) return;

            var prev = ActiveTask;
            ActiveTaskInternal.Complete();
            _taskIndex += 1;

            if (prev != ActiveTask) {
                ActiveTaskInternal?.Begin();
            }

            _eventTaskComplete.Invoke(this, prev);
            _eventUpdate.Invoke(this);

            if (Status == QuestStatus.Complete) {
                _eventComplete.Invoke(this);
            }
        }

        public string Save () {
            var data = new QuestInstanceSave {
                taskIndex = _taskIndex,
                tasks = _tasks.Select(t => new QuestInstanceTaskSave {
                    id = t.Id,
                    save = t.Save(),
                }).ToList(),
            };

            return JsonUtility.ToJson(data);
        }

        public void Load (string save) {
            var data = JsonUtility.FromJson<QuestInstanceSave>(save);
            _taskIndex = data.taskIndex;
            data.tasks.ForEach(t => {
                var instance = _tasks.Find(i => i.Id == t.id);
                instance.Load(t.save);
            });
        }

        /// <summary>
        /// Primarily a debugging method. Call Next() instead
        /// </summary>
        public void Complete () {
            if (Status == QuestStatus.Complete) return;

            while (Status != QuestStatus.Complete) {
                Next();
            }
        }

        private void PopulateTasks (IReadOnlyCollection<ITaskDefinition> tasks) {
            if (tasks.Count == 0) return;

            foreach (var task in tasks) {
                _tasks.Add(new TaskInstance(task));
            }

            _tasks[0].Begin();
        }
    }
}
