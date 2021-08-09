
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Card.Classes
{ 
    internal enum MessageEventsType
    {
        MeterError,
        CardError
    }

    internal delegate void dataReceived(object sender, MessageEventArgs arg);
    internal class MessageEventArgs : EventArgs
    {
        public string Message { get;  private set; }
        public MessageEventsType MessageType { get; private set; }

        public MessageEventArgs(string message, MessageEventsType type)
        {
            Message = message;
            MessageType = type;
        }
    }  
}
