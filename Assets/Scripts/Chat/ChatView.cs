using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Chat
{
    public class ChatView : MonoBehaviour
    {
        [SerializeField] private Transform _rootMessage;
        [SerializeField] private GameObject _message;
        [SerializeField] private TMP_InputField _input;
        [SerializeField] private Button _button;
        
        public bool IsSendReady => _input.text != string.Empty;

        public event UnityAction<string> OnSubmit;

        private void OnValidate()
        {
            if (_message == null 
                || _message.TryGetComponent(out MessageText _text) == false)
            {
                Debug.Log(nameof(MessageText) + 
                          " was not found", this);
                _message = null;
            }
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(SendHandler);
            _input.onSubmit.AddListener(SendHandler);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(SendHandler);
            _input.onSubmit.RemoveListener(SendHandler);
        }

        private void SendHandler(string value)
        {
            SendHandler();
        }
        
        private void SendHandler()
        {
            OnSubmit?.Invoke(_input.text);
        }

        public void AddMessage(string nick, string text)
        {
            var message = Instantiate(_message, _rootMessage);
            message.GetComponent<MessageText>().SetMessage(
                nick, text);
        }
    }
}
