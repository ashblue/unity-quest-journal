using System.Collections.Generic;
using CleverCrow.Fluid.QuestJournals.Tasks;
using CleverCrow.Fluid.QuestJournals.Utilities;

namespace CleverCrow.Fluid.QuestJournals.Quests {
    public interface IQuestInstance {
        string Title { get; }
        string Description { get; }
        IQuestDefinition Definition { get; }
        QuestStatus Status { get; }

        IReadOnlyList<ITaskInstanceReadOnly> Tasks { get; }
        ITaskInstanceReadOnly ActiveTask { get; }

        IUnityEventReadOnly<IQuestInstance> EventComplete { get; }
        IUnityEventReadOnly<IQuestInstance> EventUpdate { get; }
        IUnityEventReadOnly<IQuestInstance, ITaskInstanceReadOnly> EventTaskComplete { get; }

        void Next ();
        void SetTask (ITaskDefinition task);
        void Complete ();

        string Save ();
        void Load (string save);
    }
}
