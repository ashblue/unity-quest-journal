using System.Collections.Generic;
using Adnc.Utility.Editors;
using CleverCrow.Fluid.QuestJournals.Editors.Utilities;
using CleverCrow.Fluid.QuestJournals.Tasks;
using UnityEditor;

namespace CleverCrow.Fluid.QuestJournals.Quests {
    public class SortableListTasks : SortableListBase {
        private static TypesToMenu<TaskDefinitionBase> _taskTypes;
        private readonly ScriptableObjectListPrinter _soPrinter;
        private readonly NestedDataCrud<TaskDefinitionBase> _taskCrud;

        private static TypesToMenu<TaskDefinitionBase> TaskTypes =>
            _taskTypes ??= new TypesToMenu<TaskDefinitionBase>();

        public SortableListTasks (Editor editor, string property, QuestDefinitionBase parent, List<TaskDefinitionBase> tasks, string title) : base(editor, property, title) {
            _editor = editor;

            _soPrinter = new ScriptableObjectListPrinter(_serializedProp);
            _taskCrud = new NestedDataCrud<TaskDefinitionBase>(parent, tasks, TaskTypes);
            _taskCrud.BindOnCreate((task) => {
                task.SetParent(parent);
            });

            _list.drawElementCallback = _soPrinter.DrawScriptableObject;
            _list.elementHeightCallback = _soPrinter.GetHeight;

            _list.onAddDropdownCallback = _taskCrud.ShowMenu;
            _list.onRemoveCallback = _taskCrud.DeleteItem;
        }
    }
}
