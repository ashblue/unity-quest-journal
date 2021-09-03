using UnityEngine;

namespace CleverCrow.Fluid.QuestJournals.Tasks {
    public class TaskInstance : ITaskInstance {
        public ITaskDefinition Definition { get; }
        public string Id => Definition.Id;
        public string Title => Definition.Title;
        public string Description => Definition.Description;

        public TaskStatus Status { get; private set; } = TaskStatus.None;

        public TaskInstance (ITaskDefinition definition) {
            Definition = definition;
        }

        public void Begin () {
            Status = TaskStatus.Ongoing;
        }

        public void Complete () {
            Status = TaskStatus.Complete;
        }

        public string Save () {
            var data = new TaskInstanceSave {
                status = Status,
            };

            return JsonUtility.ToJson(data);
        }

        public void Load (string save) {
            var data = JsonUtility.FromJson<TaskInstanceSave>(save);
            Status = data.status;
        }

        public void ClearStatus () {
            Status = TaskStatus.None;
        }
    }
}
