using CleverCrow.Fluid.QuestJournals;
using UnityEngine;

namespace Examples.DebugQuestPrint.Scripts {
    public class SaveLoadActions : MonoBehaviour {
        private static string _save;

        [SerializeField]
        private GameObject _menuPrefab;

        private void Start () {
            _save = QuestJournalManager.Instance.Save();
        }

        public void Save () {
            _save = QuestJournalManager.Instance.Save();
        }

        public void Load () {
            QuestJournalManager.Instance.Load(_save);
            Instantiate(_menuPrefab);
            Destroy(gameObject);
        }
    }
}
