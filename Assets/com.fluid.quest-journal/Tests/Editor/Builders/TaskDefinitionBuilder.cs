using CleverCrow.Fluid.QuestJournals.Tasks;
using NSubstitute;

namespace CleverCrow.Fluid.QuestJournals.Testing.Builders {
    public class TaskDefinitionBuilder {
        private string _id = System.Guid.NewGuid().ToString();
        private string _title = System.Guid.NewGuid().ToString();
        private string _description = System.Guid.NewGuid().ToString();

        public ITaskDefinition Build () {
            var task = Substitute.For<ITaskDefinition>();
            task.Id.Returns(_id);
            task.Title.Returns(_title);
            task.Description.Returns(_description);

            return task;
        }

        public TaskDefinitionBuilder WithTitle (string title) {
            _title = title;
            return this;
        }

        public TaskDefinitionBuilder WithDescription (string description) {
            _description = description;
            return this;
        }
    }
}
