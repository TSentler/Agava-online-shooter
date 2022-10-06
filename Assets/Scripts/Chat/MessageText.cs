using TMPro;
using UnityEngine;

namespace Chat
{
    [RequireComponent(typeof(TMP_Text))]
    public class MessageText : MonoBehaviour
    {
        private readonly string _template = $"{{0}}: {{1}}";
        
        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        public void SetMessage(string nick, string message)
        {
            _text.SetText(string.Format(_template, nick, message));
        }
    }
}