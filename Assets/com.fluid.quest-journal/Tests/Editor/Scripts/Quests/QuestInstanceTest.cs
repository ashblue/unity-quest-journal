using System.Collections.Generic;
using CleverCrow.Fluid.QuestJournals.Quests;
using CleverCrow.Fluid.QuestJournals.Tasks;
using CleverCrow.Fluid.QuestJournals.Testing.Builders;
using NUnit.Framework;
using UnityEngine;

namespace CleverCrow.Fluid.QuestJournals.Testing.Quests {
    public class QuestInstanceTest {
        private class Options {
            public IQuestDefinition questDefinition = A.QuestDefinition().Build();
        }

        private QuestInstance Setup (Options options = null) {
            options ??= new Options();
            return new QuestInstance(options.questDefinition);
        }

        public class Properties {
            public class Tasks : QuestInstanceTest {
                [Test]
                public void It_should_generate_task_instances_for_each_task () {
                    var taskDefinition = A.TaskDefinition().Build();
                    var questDefinition = A.QuestDefinition().WithSingleTask(taskDefinition).Build();
                    var options = new Options { questDefinition = questDefinition };

                    var quest = Setup(options);

                    Assert.AreEqual(quest.Tasks[0].Definition, taskDefinition);
                }
            }

            public class ActiveTask : QuestInstanceTest {
                [Test]
                public void It_should_set_the_expected_starting_task () {
                    var taskDefinition = A.TaskDefinition().Build();
                    var questDefinition = A.QuestDefinition().WithSingleTask(taskDefinition).Build();
                    var options = new Options { questDefinition = questDefinition };

                    var quest = Setup(options);

                    Assert.AreEqual(quest.ActiveTask.Definition, taskDefinition);
                }

                [Test]
                public void It_should_be_null_if_there_are_no_tasks () {
                    var questDefinition = A.QuestDefinition().WithTaskCount(0).Build();
                    var options = new Options { questDefinition = questDefinition };

                    var quest = Setup(options);

                    Assert.IsNull(quest.ActiveTask);
                }

                [Test]
                public void It_should_point_to_the_next_task_when_calling_Next () {
                    var taskDefinitionA = A.TaskDefinition().Build();
                    var taskDefinitionB = A.TaskDefinition().Build();
                    var questDefinition = A.QuestDefinition()
                        .WithTasks(new List<ITaskDefinition> { taskDefinitionA, taskDefinitionB })
                        .Build();

                    var options = new Options { questDefinition = questDefinition };
                    var quest = Setup(options);
                    quest.Next();

                    Assert.AreEqual(taskDefinitionB, quest.ActiveTask.Definition);
                }

                [Test]
                public void It_should_point_to_the_last_task_when_calling_Next_to_complete_all_tasks () {
                    var questDefinition = A.QuestDefinition().WithTaskCount(1).Build();

                    var options = new Options { questDefinition = questDefinition };
                    var quest = Setup(options);
                    quest.Next();

                    Assert.AreEqual(quest.Tasks[0], quest.ActiveTask);
                }
            }

            public class Status : QuestInstanceTest {
                [Test]
                public void It_should_be_Ongoing_by_default () {
                    var quest = Setup();

                    Assert.AreEqual(QuestStatus.Ongoing, quest.Status);
                }

                [Test]
                public void It_should_change_to_Complete_when_all_tasks_are_resolved () {
                    var quest = Setup();
                    quest.Next();

                    Assert.AreEqual(QuestStatus.Complete, quest.Status);
                }

                [Test]
                public void It_should_be_Ongoing_when_Next_is_called_on_multiple_task () {
                    var questDefinition = A.QuestDefinition().WithTaskCount(2).Build();
                    var options = new Options { questDefinition = questDefinition };

                    var quest = Setup(options);
                    quest.Next();

                    Assert.AreEqual(QuestStatus.Ongoing, quest.Status);
                }
            }

            public class Name : QuestInstanceTest {
                [Test]
                public void It_should_use_the_quest_definition_Name () {
                    var name = "Lorem Ipsum";
                    var questDefinition = A.QuestDefinition().WithName(name).Build();
                    var options = new Options { questDefinition = questDefinition };

                    var quest = Setup(options);

                    Assert.AreEqual(name, quest.Title);
                }
            }

            public class Description : QuestInstanceTest {
                [Test]
                public void It_should_use_the_quest_definition_Description () {
                    var description = "Lorem Ipsum";
                    var questDefinition = A.QuestDefinition().WithDescription(description).Build();
                    var options = new Options { questDefinition = questDefinition };

                    var quest = Setup(options);

                    Assert.AreEqual(description, quest.Description);
                }
            }
        }

        public class Methods {
            public class Next : QuestInstanceTest {
                [Test]
                public void It_should_not_crash_when_calling_next_after_all_tasks_are_complete () {
                    var questDefinition = A.QuestDefinition().Build();
                    var options = new Options { questDefinition = questDefinition };

                    var quest = Setup(options);
                    quest.Next();
                    quest.Next();
                }

                [Test]
                public void It_should_mark_the_new_task_as_ongoing () {
                    var questDefinition = A.QuestDefinition().WithTaskCount(2).Build();
                    var options = new Options { questDefinition = questDefinition };

                    var quest = Setup(options);
                    quest.Next();

                    Assert.AreEqual(TaskStatus.Ongoing, quest.ActiveTask.Status);
                }

                [Test]
                public void It_should_mark_completed_task_as_complete () {
                    var questDefinition = A.QuestDefinition().WithTaskCount(1).Build();
                    var options = new Options { questDefinition = questDefinition };

                    var quest = Setup(options);
                    quest.Next();

                    Assert.AreEqual(TaskStatus.Complete, quest.ActiveTask.Status);
                }
            }

            public class SetTask : QuestInstanceTest {
                [Test]
                public void It_should_set_all_previous_tasks_to_complete () {
                    var taskDefinitionA = A.TaskDefinition().Build();
                    var taskDefinitionB = A.TaskDefinition().Build();
                    var questDefinition = A.QuestDefinition()
                        .WithTasks(new List<ITaskDefinition> { taskDefinitionA, taskDefinitionB })
                        .Build();

                    var options = new Options { questDefinition = questDefinition };
                    var quest = Setup(options);
                    quest.SetTask(taskDefinitionB);

                    Assert.AreEqual(TaskStatus.Complete, quest.Tasks[0].Status);
                }

                [Test]
                public void It_should_set_the_targeted_task_as_ongoing () {
                    var taskDefinitionA = A.TaskDefinition().Build();
                    var taskDefinitionB = A.TaskDefinition().Build();
                    var questDefinition = A.QuestDefinition()
                        .WithTasks(new List<ITaskDefinition> { taskDefinitionA, taskDefinitionB })
                        .Build();

                    var options = new Options { questDefinition = questDefinition };
                    var quest = Setup(options);
                    quest.SetTask(taskDefinitionB);

                    Assert.AreEqual(TaskStatus.Ongoing, quest.ActiveTask.Status);
                }

                [Test]
                public void It_should_set_all_future_tasks_as_none () {
                    var taskDefinitionA = A.TaskDefinition().Build();
                    var taskDefinitionB = A.TaskDefinition().Build();
                    var questDefinition = A.QuestDefinition()
                        .WithTasks(new List<ITaskDefinition> { taskDefinitionA, taskDefinitionB })
                        .Build();

                    var options = new Options { questDefinition = questDefinition };
                    var quest = Setup(options);
                    quest.SetTask(taskDefinitionB);
                    quest.SetTask(taskDefinitionA);

                    Assert.AreEqual(TaskStatus.None, quest.Tasks[1].Status);
                }
            }

            public class Complete : QuestInstanceTest {
                [Test]
                public void It_should_mark_the_quest_as_complete () {
                    var questDefinition = A.QuestDefinition().Build();
                    var options = new Options { questDefinition = questDefinition };

                    var quest = Setup(options);
                    quest.Complete();

                    Assert.AreEqual(QuestStatus.Complete, quest.Status);
                }

                [Test]
                public void It_should_mark_all_tasks_as_complete () {
                    var questDefinition = A.QuestDefinition().Build();
                    var options = new Options { questDefinition = questDefinition };

                    var quest = Setup(options);
                    quest.Complete();

                    foreach (var task in quest.Tasks) {
                        Assert.AreEqual(TaskStatus.Complete, task.Status);
                    }
                }
            }

            public class Save : QuestInstanceTest {
                [Test]
                public void It_should_save_the_quest_data () {
                    var questDefinition = A.QuestDefinition().Build();
                    var options = new Options { questDefinition = questDefinition };

                    var quest = Setup(options);
                    var taskInstance = quest.Tasks[0];
                    var expectedSaveRaw = new QuestInstanceSave {
                        taskIndex = 0,
                        tasks = new List<QuestInstanceTaskSave> {
                            new QuestInstanceTaskSave {
                                id = taskInstance.Id,
                                save = taskInstance.Save(),
                            },
                        },
                    };
                    var expectedSave = JsonUtility.ToJson(expectedSaveRaw);

                    var saveRaw = quest.Save();

                    Assert.AreEqual(expectedSave, saveRaw);
                }
            }

            public class Load : QuestInstanceTest {
                [Test]
                public void It_should_load_the_expected_index () {
                    var questDefinition = A.QuestDefinition().WithTaskCount(2).Build();
                    var options = new Options { questDefinition = questDefinition };

                    var questSample = Setup(options);
                    questSample.Next();
                    var save = questSample.Save();

                    var quest = Setup(options);
                    quest.Load(save);

                    Assert.AreEqual(quest.ActiveTask.Definition, questSample.ActiveTask.Definition);
                }

                [Test]
                public void It_should_restore_the_task_as_expected () {
                    var questDefinition = A.QuestDefinition().WithTaskCount(2).Build();
                    var options = new Options { questDefinition = questDefinition };

                    var questSample = Setup(options);
                    questSample.Next();
                    var save = questSample.Save();

                    var quest = Setup(options);
                    quest.Load(save);

                    Assert.AreEqual(TaskStatus.Complete, questSample.Tasks[0].Status);
                }
            }
        }
    }
}
