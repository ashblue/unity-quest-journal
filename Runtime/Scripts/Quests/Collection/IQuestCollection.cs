using System.Collections.Generic;
using CleverCrow.Fluid.QuestJournals.Tasks;
using CleverCrow.Fluid.QuestJournals.Utilities;

namespace CleverCrow.Fluid.QuestJournals.Quests {
    public interface IQuestCollection {
        /// <summary>
        /// Triggers when a quest is added to the collection. Generally useful for UI updates
        /// </summary>
        IUnityEventReadOnly<IQuestInstance> EventQuestAdd { get; }

        /// <summary>
        /// Triggers when a quest is completed due to running out of tasks. Useful for quest completion post processing events
        /// </summary>
        IUnityEventReadOnly<IQuestInstance> EventQuestComplete { get; }

        /// <summary>
        /// Triggered when a quest has a task change. A good place to update your UI if you are displaying quest progress
        /// </summary>
        IUnityEventReadOnly<IQuestInstance> EventQuestUpdate { get; }

        /// <summary>
        /// Triggers whenever a task is completed with the corresponding quest and task instance. Useful to fire post processing events with completed tasks.
        /// </summary>
        IUnityEventReadOnly<IQuestInstance, ITaskInstanceReadOnly> EventQuestTaskComplete { get; }

        IQuestInstance Add (IQuestDefinition quest);
        IQuestInstance Add (ITaskDefinition task);

        IQuestInstance Get (IQuestDefinition definition);
        IQuestInstance Get (ITaskDefinition definition);

        string Save ();
        void Load (string save);
        List<IQuestInstance> GetAll ();
    }
}
