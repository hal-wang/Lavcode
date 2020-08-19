using System;

namespace Hubery.Lavcode.Uwp.Controls.Message
{
    internal class MessageItem : IMessage
    {
        public MessageItem(string text, MessageType type, int duration)
        {
            Text = text;
            MessageType = type;
            Duration = new TimeSpan(0, 0, 0, 0, duration);
            AutoHide = duration != default;
        }

        public string Text { get; set; }
        public MessageType MessageType { get; set; }
        public TimeSpan Duration { get; set; }
        public bool AutoHide { get; set; }
    }
}
