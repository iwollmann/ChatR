using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Events
{
    public class JoinedApplicationEvent : ApplicationEvent
    {
        public string RoomName { get; set; }
    }
}
