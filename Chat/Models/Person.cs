using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatLibrary;

namespace Chat.Models
{
    public class Person : IPerson
    {
        public string Domain { get; set; }
        public string Name { get; set; }
        public string ConnectionId { get; set; }

        public Person() { }

        public Person(string connectionId)
        {
            this.ConnectionId = connectionId;
        }

        public Person(string name, string connectionId)
        {
            this.Name = name;
            this.ConnectionId = connectionId;
        }

        public static Person getUserLogged(string connectionId)
        {
            var login = HttpContext.Current.User.Identity.Name.Split('\\');
            if (login.Length > 1)
            {
                return new Person()
                {
                    Domain = login[0],
                    Name = login[1],
                    ConnectionId = connectionId
                };
            }
            else if (login.Length == 1)
            {
                return new Person(login[0], connectionId);
            }
            else
            {
                return new Person(connectionId);
            }
        }
    }
}