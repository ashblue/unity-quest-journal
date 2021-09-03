using System.Collections.Generic;
using CleverCrow.Fluid.QuestJournals.Quests;
using CleverCrow.Fluid.QuestJournals.Tasks;
using NSubstitute;

namespace CleverCrow.Fluid.QuestJournals.Testing.Builders {
    public class QuestDefinitionBuilder {
        private string _name;
        private string _description;

        private List<ITaskDefinition> _tasks = new List<ITaskDefinition> {
            Substitute.For<ITaskDefinition>(),
        };

        public IQuestDefinition Build () {
            var questData = Substitute.For<IQuestDefinition>();
            questData.Title.Returns(_name);
            questData.Description.Returns(_description);

            _tasks.ForEach(t => t.Parent.Returns(questData));
            questData.Tasks.Returns(_tasks);

            return questData;
        }

        public QuestDefinitionBuilder WithSingleTask (ITaskDefinition task) {
            _tasks = new List<ITaskDefinition> { task };
            return this;
        }

        public QuestDefinitionBuilder WithTaskCount (int count) {
            _tasks.Clear();
            for (var i = 0; i < count; i++) {
                _tasks.Add(Substitute.For<ITaskDefinition>());
            }

            return this;
        }

        public QuestDefinitionBuilder WithName (string name) {
            _name = name;
            return this;
        }

        public QuestDefinitionBuilder WithDescription (string description) {
            _description = description;
            return this;
        }

        public QuestDefinitionBuilder WithTasks (List<ITaskDefinition> tasks) {
            _tasks = tasks;
            return this;
        }
    }
}
