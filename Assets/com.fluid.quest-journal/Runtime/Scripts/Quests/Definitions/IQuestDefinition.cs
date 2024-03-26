using System.Collections.Generic;
using CleverCrow.Fluid.QuestJournals.Tasks;

namespace CleverCrow.Fluid.QuestJournals.Quests {
    public interface IQuestDefinition {
        string Id { get; }
        string DisplayName { get; }
        string Description { get; }
        IReadOnlyList<ITaskDefinition> Tasks { get; }
    }
}
