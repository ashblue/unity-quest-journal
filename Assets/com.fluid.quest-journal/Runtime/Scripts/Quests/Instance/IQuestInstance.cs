using System.Collections.Generic;
using CleverCrow.Fluid.QuestJournals.Tasks;

namespace CleverCrow.Fluid.QuestJournals.Quests {
    public interface IQuestInstance {
        string Title { get; }
        string Description { get; }
        IQuestDefinition Definition { get; }
        QuestStatus Status { get; }

        IReadOnlyList<ITaskInstanceReadOnly> Tasks { get; }
        ITaskInstance ActiveTask { get; }

        void Next ();
        void SetTask (ITaskDefinition task);
        void Complete ();

        string Save ();
        void Load (string save);
    }
}
