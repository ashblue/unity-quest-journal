using CleverCrow.Fluid.QuestJournals.Tasks;
using CleverCrow.Fluid.QuestJournals.Testing.Builders;
using NUnit.Framework;

namespace CleverCrow.Fluid.QuestJournals.Testing.Tasks {
    public class TaskInstanceTest {
        private class Options {
            public ITaskDefinition taskDefinition = A.TaskDefinition().Build();
        }

        private TaskInstance Setup (Options options = null) {
            options ??= new Options();
            return new TaskInstance(options.taskDefinition);
        }

        public class Constructor : TaskInstanceTest{
            [Test]
            public void It_should_initialize () {
                var instance = Setup();
                Assert.NotNull(instance);
            }
        }

        public class Title_Property : TaskInstanceTest {
            [Test]
            public void It_should_be_the_same_as_the_definition_title() {
                var title = System.Guid.NewGuid().ToString();
                var options = new Options {
                    taskDefinition = A.TaskDefinition().WithTitle(title).Build()
                };

                var instance = Setup(options);

                Assert.AreEqual(title, instance.Title);
            }
        }

        public class Description_Property : TaskInstanceTest {
            [Test]
            public void It_should_be_the_same_as_the_definition_title() {
                var description = System.Guid.NewGuid().ToString();
                var options = new Options {
                    taskDefinition = A.TaskDefinition().WithDescription(description).Build()
                };

                var instance = Setup(options);

                Assert.AreEqual(description, instance.Description);
            }
        }
    }
}
