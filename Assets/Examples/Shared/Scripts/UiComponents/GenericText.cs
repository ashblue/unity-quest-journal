using UnityEngine;
using UnityEngine.UI;

namespace CleverCrow.Fluid.QuestJournals.Examples {
    public class GenericText : MonoBehaviour {
        [SerializeField]
        private Text _text;

        [SerializeField]
        private string _prefix;

        public void SetText (string text) {
            _text.text = $"{_prefix} {text}".Trim();
        }
    }
}
