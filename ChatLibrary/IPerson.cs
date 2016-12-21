using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatLibrary
{
    public interface IPerson
    {
        string Domain { get; set; }
        string Name { get; set; }
        string ConnectionId { get; }
    }
}
