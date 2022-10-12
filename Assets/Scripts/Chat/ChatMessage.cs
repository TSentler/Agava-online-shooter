using System;

namespace Chat
{
    [Serializable]
    public class ChatMessage
    {
        public ChatMessage(string nick, string text)
        {
            Nick = nick;
            Text = text;
        }

        public string Nick { get; }
        public string Text { get; }
    }
}