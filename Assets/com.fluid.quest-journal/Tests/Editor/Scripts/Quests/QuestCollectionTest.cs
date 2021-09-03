using System.Collections.Generic;
using CleverCrow.Fluid.QuestJournals.Quests;
using CleverCrow.Fluid.QuestJournals.Testing.Builders;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace CleverCrow.Fluid.QuestJournals.Testing.Quests {
    public class QuestCollectionTest {
        private class Options {
            public IQuestDatabase QuestDatabase = Substitute.For<IQuestDatabase>();
        }

        private QuestCollection Setup (Options options = null) {
            options ??= new Options();
            return new QuestCollection(options.QuestDatabase);
        }

        public class Add_Method {
            public class AddingByQuest : QuestCollectionTest {
                [Test]
                public void It_should_create_a_quest_instance_with_the_definition () {
                    var questData = A.QuestDefinition().Build();

                    var col = Setup();
                    var questInstance = col.Add(questData);

                    Assert.AreEqual(questInstance.Definition, questData);
                }

                [Test]
                public void It_should_return_the_same_quest_instance_with_multiple_adds () {
                    var questData = A.QuestDefinition().Build();

                    var col = Setup();
                    var questInstanceA = col.Add(questData);
                    var questInstanceB = col.Add(questData);

                    Assert.AreEqual(questInstanceA, questInstanceB);
                }
            }

            public class AddingByTask : QuestCollectionTest {
                [Test]
                public void It_should_return_the_parent_quest_instance () {
                    var questData = A.QuestDefinition().Build();
                    var taskData = questData.Tasks[0];

                    var col = Setup();
                    var questInstance = col.Add(taskData);

                    Assert.AreEqual(questInstance.Definition, questData);
                }

                [Test]
                public void It_should_set_the_active_task_on_the_instance_to_the_task () {
                    var questData = A.QuestDefinition().WithTaskCount(2).Build();
                    var taskData = questData.Tasks[1];

                    var col = Setup();
                    var questInstance = col.Add(taskData);

                    Assert.AreEqual(questInstance.ActiveTask.Definition, taskData);
                }
            }
        }

        public class Get_Method {
            public class ByQuest : QuestCollectionTest {
                [Test]
                public void It_should_return_the_quest_instance_by_IQuestData () {
                    var definition = A.QuestDefinition().Build();

                    var col = Setup();
                    var questInstance = col.Add(definition);
                    var getValue = col.Get(definition);

                    Assert.AreEqual(getValue, questInstance);
                }

                [Test]
                public void It_should_return_null_if_there_is_no_quest_instance () {
                    var definition = Substitute.For<IQuestDefinition>();

                    var col = Setup();
                    var getValue = col.Get(definition);

                    Assert.IsNull(getValue);
                }
            }

            public class ByTask : QuestCollectionTest {
                [Test]
                public void It_should_return_the_quest_instance_by_ITaskData () {
                    var questData = A.QuestDefinition().WithTaskCount(2).Build();
                    var taskData = questData.Tasks[1];

                    var col = Setup();
                    var questInstance = col.Add(taskData);
                    var getValue = col.Get(taskData);

                    Assert.AreEqual(questInstance, getValue);
                }
            }
        }

        public class GetAll_Method : QuestCollectionTest {
            [Test]
            public void It_should_get_all_the_quests () {
                var questData = A.QuestDefinition().Build();

                var col = Setup();
                var questInstance = col.Add(questData);
                var quests = col.GetAll();

                Assert.AreEqual(quests[0], questInstance);
            }
        }

        public class Save_Method : QuestCollectionTest {
            [Test]
            public void It_should_save_all_quest_instances () {
                var questData = A.QuestDefinition().Build();

                var col = Setup();
                var questInstance =  col.Add(questData);

                var saveExample = new QuestCollectionSave {
                    quests = new List<QuestCollectionEntrySave> {
                        new QuestCollectionEntrySave {
                            questId = questData.Id,
                            questSave = questInstance.Save(),
                        },
                    },
                };
                var saveExpected = JsonUtility.ToJson(saveExample);

                var save = col.Save();

                Assert.AreEqual(saveExpected, save);
            }
        }

        public class Load_Method : QuestCollectionTest {
            [Test]
            public void It_should_clear_out_old_quest_data () {
                var questData = A.QuestDefinition().Build();

                var col = Setup();
                var emptySave = col.Save();
                col.Add(questData);
                col.Load(emptySave);

                var instance = col.Get(questData);

                Assert.IsNull(instance);
            }

            [Test]
            public void It_should_restore_quest_instances () {
                var questData = A.QuestDefinition().Build();
                var questLibrary = Substitute.For<IQuestDatabase>();
                questLibrary.GetQuest(questData.Id).Returns(questData);
                var options = new Options { QuestDatabase = questLibrary };

                var col = Setup(options);
                col.Add(questData);
                var save = col.Save();
                col.Load(save);
                var questInstance =  col.Get(questData);

                Assert.AreEqual(questData, questInstance.Definition);
            }
        }
    }
}
