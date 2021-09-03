using CleverCrow.Fluid.QuestJournals.Quests;

namespace CleverCrow.Fluid.QuestJournals.Tasks {
    public interface ITaskDefinition {
        string Id { get; }
        string Title { get; }
        string Description { get; }
        IQuestDefinition Parent { get; }
    }
}
