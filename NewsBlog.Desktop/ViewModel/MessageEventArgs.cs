using System;

namespace NewsBlog.Desktop.ViewModel
{
    public class MessageEventArgs : EventArgs
    {
        public String Message { get; private set; }

        public MessageEventArgs(String message)
        {
            Message = message;
        }
    }
}
