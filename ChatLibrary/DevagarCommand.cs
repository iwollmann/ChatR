using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace ChatLibrary
{
    [Export(typeof(IChatCommand))]
    [ExportMetadata("Code", "devagar")]
    public class DevagarCommand : IChatCommand
    {
        public bool Execute(IChat context, string roomName, IPerson user, string parameters)
        {
            var p = parameters.Split(' ');
            //var usersInRoom = context.GetUsersInRoom(roomName);
            var womanStyle = p[1];
            
            context.Send(roomName, "Devagar hein! " + womanStyle + " na área!");

            return false;
        }
    }
}
