using CleverCrow.Fluid.QuestJournals.Quests;
using CleverCrow.Fluid.Utilities;
using UnityEngine;

namespace CleverCrow.Fluid.QuestJournals {
    public class QuestJournalManager : Singleton<QuestJournalManager> {
        public IQuestCollection Quests { get; private set; }
        private QuestJournalSettings Settings => QuestJournalSettings.Current;

        private void Awake () {
            Quests = new QuestCollection(Settings.Database);
            Settings.StartingQuests.ForEach((quest) => Quests.Add(quest));

            if (Application.isEditor) {
                SetupDebugQuests();
            }
        }

        private void SetupDebugQuests () {
            Settings.DebugQuests.ForEach(quest => {
                var instance = Quests.Add(quest.Definition);
                if (quest.MarkComplete) instance.Complete();
            });
            
            Settings.DebugTasks.ForEach(task => Quests.Add(task));
        }

        public string Save () {
            return Quests.Save();
        }

        public void Load (string save) {
            Quests.Load(save);
        }
    }
}
