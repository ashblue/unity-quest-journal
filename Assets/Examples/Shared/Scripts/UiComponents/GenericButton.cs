using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CleverCrow.Fluid.QuestJournals.Examples {
    public class GenericButton : MonoBehaviour {
        [SerializeField]
        private Button _button;
        
        [SerializeField]
        private Text _text;

        public void BindButton (UnityAction clickCallback) {
            _button.onClick.AddListener(clickCallback);
        }

        public void SetText (string text) {
            _text.text = text;
        }
    }
}

