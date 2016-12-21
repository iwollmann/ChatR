using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ChatLibrary
{
    [Export(typeof(IChatCommand))]
    [ExportMetadata("Code", "votekick")]
    public class VoteKickCommand : IChatCommand
    {
        static object sync = new object();
        static public ConcurrentDictionary<IPerson, Subject<KeyValuePair<string, IPerson>>> Votes = new ConcurrentDictionary<IPerson, Subject<KeyValuePair<string, IPerson>>>();

        static VoteKickCommand()
        {
        }

        public bool Execute(IChat context, string roomName, IPerson user, string parameters)
        {
            var p = parameters.Split(' ');
            var usersInRoom = context.GetUsersInRoom(roomName);
            var users = usersInRoom.Where(x => p.Contains(x.Name));

            if (users.Count() <= 0)
                return false;

            var userToBeKicked = users.Single();

            EnsuresObservable(context, users.Single(), roomName);
            Votes[userToBeKicked].OnNext(new KeyValuePair<string, IPerson>("votekick", user));

            return false;
        }

        private static void EnsuresObservable(IChat chat, IPerson toBeKicked, string roomName)
        {
            if (Votes.ContainsKey(toBeKicked) == false)
            {
                lock (sync)
                {
                    if (Votes.ContainsKey(toBeKicked) == false)
                    {
                        var votes = Votes.AddOrUpdate(toBeKicked, new Subject<KeyValuePair<string, IPerson>>(), (a, b) => null);
                        IDisposable subscribe = null;
                        subscribe = votes.Buffer(TimeSpan.FromSeconds(30))
                            .Subscribe(x =>
                            {
                                try
                                {
                                    if (x.Count >= (chat.GetUsersInRoom(roomName).Count / 2))
                                    {
                                        chat.Quit(roomName, toBeKicked);
                                        //chat.Send(roomName, "SE FUFU!");
                                    }
                                }
                                finally
                                {
                                    Subject<KeyValuePair<string, IPerson>> key = null;
                                    subscribe.Dispose();
                                    Votes.TryRemove(toBeKicked, out key);
                                }
                            });
                    }
                }
            }
        }
    }
}
