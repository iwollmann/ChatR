using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Chat.Events;
using Microsoft.Security.Application;
using SignalR.Hubs;
using SignalR;
using System.Net;
using System.Threading.Tasks;
using System;
using Chat.Models;
using ChatLibrary;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.Reflection;

namespace Chat
{
    public class Room
    {
        public string Name { get; set; }
        public List<IPerson> People { get; set; }

        public Room()
        {
            People = new List<IPerson>();
        }

        public bool CanSpeak(string name)
        {
            return People.Any(x => x.Name == name);
        }
    }

    public class EventChatDecorator : IChat
    {
        IChat Chat;

        public Subject<ApplicationEvent> Events { get; private set; }

        public EventChatDecorator(IChat chat)
        {
            Chat = chat;

            Events = new Subject<ApplicationEvent>();
        }

        #region IChat Members

        public void Join(string roomName)
        {
            Chat.Join(roomName);

            Events.OnNext(new JoinedApplicationEvent() { RoomName = roomName });
        }

        public void GetRooms()
        {
            Chat.GetRooms();
        }

        public void Send(string roomName, string message)
        {
            Chat.Send(roomName, message);
        }

        public void Quit(string roomName)
        {
            Chat.Quit(roomName);
        }

        public void Quit(string roomName, IPerson user)
        {
            Chat.Quit(roomName, user);
        }

        public void ChangeRoom(string roomFromName, string roomTooName)
        {
            Chat.ChangeRoom(roomFromName, roomTooName);
        }

        public List<IPerson> GetUsersInRoom(string roomName)
        {
            throw new NotImplementedException();
        }

        public Dictionary<KeyValuePair<string, IPerson>, List<IPerson>> voting
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }

    public class SafeMessagePipeline : IMessagePipeline
    {
        #region IMessagePipeline Members

        public string Apply(string original)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

    public class EmoticonsMessagePipeline : IMessagePipeline
    {
        #region IMessagePipeline Members

        public string Apply(string original)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

    public abstract class DelayedMessagePipeline : IMessagePipeline
    {
        #region IMessagePipeline Members

        public string Apply(string original)
        {
            var result = Instant(original);

            Task.Factory.StartNew(() =>
            {
                Delayed(original);
            });

            return result;
        }

        #endregion

        protected abstract string Instant(string original);
        protected abstract void Delayed(string original);
    }

    public interface IMessagePipeline
    {
        string Apply(string original);
    }

    public class Chat : Hub, IChat
    {
        [ImportMany]
        IEnumerable<Lazy<IChatCommand, IChatCommandData>> Commands;

        static Dictionary<string, Room> Rooms = new Dictionary<string, Room>();
        static Person LastToSendMessage = new Person();

        public Dictionary<KeyValuePair<string, IPerson>, List<IPerson>> voting { get; set; }
        
        private Person user;
        public Person User
        {
            get
            {
                if (user == null)
                {
                    user = Person.getUserLogged(Context.ConnectionId);
                }
                return user;
            }
        }

        public Chat()
            :base() 
        {
            voting = new Dictionary<KeyValuePair<string, IPerson>, List<IPerson>>();


            
        }

        public void Join(string roomName)
        {
            var safeRoomName = Encoder.HtmlEncode(roomName);

            CreateRoom(safeRoomName, User.Name);

            Groups.Add(Context.ConnectionId, safeRoomName);

            var usersInRoom = GetUsersInRoom(roomName);
            if (!usersInRoom.Any(x => x.Name == User.Name))
            {
                Clients[roomName].joined(safeRoomName, User.Name);
                LastToSendMessage = new Person();
                Rooms[roomName].People.Add(User);
            }

            SetUsers(safeRoomName);
        }

        private void SetUsers(string roomName)
        {
            if (Rooms.ContainsKey(roomName))
            {
                var room = Rooms[roomName];
                Clients[roomName].setUsers(room.People);
            }
        }

        public List<IPerson> GetUsersInRoom(string roomName)
        {
            if (Rooms.ContainsKey(roomName))
            {
                var room = Rooms[roomName];
                return room.People as List<IPerson>;
            }

            return new List<IPerson>();
        }

        public void GetRooms()
        {
            var rooms = Rooms.Select(x => new { Name = x.Key }).ToList();
            this.Clients[Context.ConnectionId].SetRooms(rooms);
        }

        public void Send(string roomName, string message)
        {
            var safeMessage = Encoder.HtmlEncode(message);

            //safeMessage = Regex.Replace(safeMessage, "(https?|ftp|file):\\/\\/[-A-Za-z0-9+&@#\\/%?=~_|!:,.;]*[-A-Za-z0-9+&@#\\/%=~_|]", new MatchEvaluator(x =>
            //    {
            //        var client = new WebClient();
            //        var html = client.DownloadString(x.Value);

            //        var matches = Regex.Matches(html, @"<meta name=""(?<NAME>.*?)"" content=""(?<VALUE>.*?)"" />");

            //        Dictionary<string, string> metas = new Dictionary<string, string>();

            //        foreach (var item in matches.OfType<Match>())
            //        {
            //            var key = item.Groups["NAME"].Value.ToLower();
            //            if (metas.ContainsKey(key) == false)
            //            {
            //                metas.Add(key, item.Groups["CONTENT"].Value);
            //            }
            //        }

            //        var titleMatch = Regex.Match(html, @"<title>(?<TITLE>.*?)</title>");
            //        string title = null;
            //        if (titleMatch != null)
            //        {
            //            title = titleMatch.Groups["TITLE"].Value;
            //        }

            //        if (string.IsNullOrEmpty(title) == false)
            //        {
            //            return string.Format(@"<a href=""{1}"">{0}</a>", title, x.Value);
            //        }
            //        else
            //        {
            //            return x.Value;
            //        }
            //    }));

            safeMessage = Regex.Replace(safeMessage, @"\{\{(?<number>[0-9]+):(?<size>[0-9]+)\}\}", @"<img src='http://www.iconfinder.com/ajax/download/png/?id=${number}&s=${size}' />");


            //var name = HttpContext.Current.User.Identity.Name;
            if (Rooms[roomName].CanSpeak(User.Name) && safeMessage.Trim() != string.Empty)
            {
                if (safeMessage.StartsWith(@"\"))
                {
                    foreach (var command in Commands)
                    {
                        if (safeMessage.Remove(0,1).ToLowerInvariant().StartsWith(command.Metadata.Code)){
                            var parameters = safeMessage.Remove(0, command.Metadata.Code.Count() + 1);
                            command.Value.Execute(this, roomName, user, parameters);
                        }
                    }

                    return;
                }

                bool appendMessage = LastToSendMessage.Name == User.Name;
                var prefixMessage = appendMessage ? string.Empty : "<div class='msgUser'><div class='userName'>" + User.Name + "</div><img src='" + VirtualPathUtility.ToAbsolute("~/Content/img/avatar.png") + "'></div>";
                Clients[roomName].messageReceived(roomName, string.Format("<div class='msg'><div class='msgTime'>{2}</div><div class='msgContent'>{1}</div>{0}</div>", prefixMessage, safeMessage, DateTime.Now.ToShortTimeString()), User.Name, appendMessage);
                LastToSendMessage = User;
                Clients[roomName].notify(UrlHelper.GenerateContentUrl("~/Content/img/chat.png", new HttpContextWrapper(HttpContext.Current)), "New Message", safeMessage, 2000);
            }
        }

        public void Quit(string roomName)
        {
            Quit(roomName, User);
        }

        public void Quit(string roomName, IPerson user) {
            Groups.Remove(user.ConnectionId, roomName);
            Rooms[roomName].People.RemoveAll(x => x.Name == user.Name);
            if (LastToSendMessage.Name == user.Name)
                LastToSendMessage = new Person();

            Clients[roomName].Quited(roomName, user.Name);
        }

        private void BroadcastRooms()
        {
            var rooms = Rooms.Select(x => new { Name = x.Key }).ToList();
            this.Clients.SetRooms(rooms);
        }


        private void CreateRoom(string roomName, string name)
        {
            if (Rooms.ContainsKey(roomName) == false)
            {
                var room = new Room()
                {
                    Name = roomName
                };

                Rooms.Add(roomName, room);
                BroadcastRooms();

            }
        }

        public void ChangeRoom(string roomFromName, string roomTooName)
        {
            //var teste = 1;
        }
    }


    public class MvcApplication : System.Web.HttpApplication
    {
        private CompositionContainer _container;

        protected void Application_Start()
        {
            var catalog = new AggregateCatalog(
                            new AssemblyCatalog(Assembly.GetExecutingAssembly()),
                            new AssemblyCatalog(Assembly.Load("ChatLibrary")));

            _container = new CompositionContainer(catalog);

            try
            {
                _container.ComposeParts(catalog);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }

            RouteTable.Routes.MapHubs(new MefDependencyResolver(_container));

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


        }
    }

    public class MefDependencyResolver : DefaultDependencyResolver {

        private CompositionContainer _container;

        public MefDependencyResolver(CompositionContainer container){
            _container = container;
        }

        public override object GetService(Type serviceType)
        {
            if (serviceType == typeof(Chat))
            {
                var chat = new Chat();
                _container.SatisfyImportsOnce(chat);
                return chat;
            }
            var export = _container.GetExports(serviceType, null, null).SingleOrDefault();

            return null != export ? export.Value : base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            var exports = _container.GetExports(serviceType, null, null);

            return exports.Select(x => x.Value).Union(base.GetServices(serviceType));
        }
    }
}