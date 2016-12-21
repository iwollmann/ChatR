using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatLibrary
{
    public interface IChatCommand
    {
        bool Execute(IChat context,string roomName, IPerson user,  string parameters);
    }

    public interface IChatCommandData
    {
        string Code { get; }
    }
}
