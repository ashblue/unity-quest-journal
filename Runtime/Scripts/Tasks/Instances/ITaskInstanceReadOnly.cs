namespace CleverCrow.Fluid.QuestJournals.Tasks {
    public interface ITaskInstanceReadOnly {
        string Id { get; }
        string Title { get; }
        string Description { get; }
        ITaskDefinition Definition { get; }
        TaskStatus Status { get; }

        string Save ();
    }
}
