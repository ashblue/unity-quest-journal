using System.Collections.Generic;

namespace CleverCrow.Fluid.QuestJournals.Quests {
    [System.Serializable]
    public class QuestCollectionEntrySave {
        public string questId;
        public string questSave;
    }

    [System.Serializable]
    public class QuestCollectionSave {
        public List<QuestCollectionEntrySave> quests;
    }
}
