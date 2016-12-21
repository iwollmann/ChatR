using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatLibrary
{
    public interface IChat
    {
        void Join(string roomName);
        void GetRooms();
        void Send(string roomName, string message);
        void Quit(string roomName);
        void Quit(string roomName, IPerson user);
        void ChangeRoom(string roomFromName, string roomTooName);
        List<IPerson> GetUsersInRoom(string roomName);

        Dictionary<KeyValuePair<string,IPerson>, List<IPerson>> voting {get;set;}
    }
}
