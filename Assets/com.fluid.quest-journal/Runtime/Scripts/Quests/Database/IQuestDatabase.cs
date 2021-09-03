namespace CleverCrow.Fluid.QuestJournals.Quests {
    public interface IQuestDatabase {
        void Setup ();
        IQuestDefinition GetQuest (string id);
    }
}
