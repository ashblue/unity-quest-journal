using System.Linq;
using CleverCrow.Fluid.QuestJournals.Quests;
using CleverCrow.Fluid.QuestJournals.Tasks;
using UnityEngine;

namespace CleverCrow.Fluid.QuestJournals.Examples {
    public class PrintQuestDetails : MonoBehaviour {
        private IQuestInstance _quest;

        [Header("Quest")]

        [SerializeField]
        private GenericText _questTitle;

        [SerializeField]
        private GenericText _questId;

        [SerializeField]
        private GenericText _questStatus;

        [SerializeField]
        private GenericText _questDescription;

        [Header("Active Task")]

        [SerializeField]
        private GenericText _taskTitle;

        [SerializeField]
        private GenericText _taskId;

        [SerializeField]
        private GenericText _taskStatus;

        [SerializeField]
        private GenericText _taskDescription;

        [Header("Task List")]

        [SerializeField]
        private GenericText _listItemPrefab;

        [SerializeField]
        private RectTransform _taskListOutput;

        [SerializeField]
        private bool _hideEmptyTasks = true;

        [SerializeField]
        private bool _printTaskDetails;

        public void SetQuest (IQuestInstance quest) {
            _quest = quest;
            RefreshDisplay(quest);
        }

        public void NextTask () {
            _quest.Next();
            RefreshDisplay(_quest);
        }

        private void RefreshDisplay (IQuestInstance quest) {
            UpdateQuest(quest);
            UpdateTask(quest);
            UpdateTaskList(quest);
        }

        private void UpdateTaskList (IQuestInstance quest) {
            foreach (Transform t in _taskListOutput) {
                Destroy(t.gameObject);
            }

            quest.Tasks.ToList().ForEach(task => {
                if (_hideEmptyTasks && task.Status == TaskStatus.None) return;
                var listItem = Instantiate(_listItemPrefab, _taskListOutput);

                var title = task.Title;
                if (_printTaskDetails) {
                    title += $" {task.Status.ToString()}";
                    if (quest.ActiveTask == task) title = $"-> {title}";
                }

                listItem.SetText(title);
            });
        }

        private void UpdateTask (IQuestInstance quest) {
            _taskTitle?.SetText(quest.ActiveTask.Title);
            _taskId?.SetText(quest.ActiveTask.Definition.Id);
            _taskStatus?.SetText(quest.ActiveTask.Status.ToString());
            _taskDescription?.SetText(quest.ActiveTask.Description);
        }

        private void UpdateQuest (IQuestInstance quest) {
            _questTitle?.SetText(quest.Title);
            _questId?.SetText(quest.Definition.Id);
            _questStatus?.SetText(quest.Status.ToString());
            _questDescription?.SetText(quest.Description);
        }
    }
}
