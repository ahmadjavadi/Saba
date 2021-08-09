using System;

namespace Card
{
    public delegate void CardManagerEventHandler(object sender, CardManagerEvent e);
    public class CardManagerEvent : EventArgs
    {
        public string message;        
    }
}
