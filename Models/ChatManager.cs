using PersistCommunicator.Abstractions;
using System.Collections.Generic;

namespace PersistCommunicator.Models
{
    public class ChatManager : IChatManager
    {
         private static Dictionary<string, Tuple<HashSet<Communicator>, string>> rooms { get; set; }

         public ChatManager() {
           rooms = new Dictionary<string, Tuple<HashSet<Communicator>,string>>();
        }

        public string roomCreatedMessage(string UserName, string roomName) => $"Hey {UserName} ! Room {roomName} has been created";
        public string roomJoinedMessage(string UserName) => $"{UserName} Joined the Session";
        public string userLeftMessage(string UserName) => $"{UserName} left the Session";
        public void AddGroup(string roomName, string userName, string connectionId)
        {
            Communicator communicator = new() { CommunicatorConnectionId = connectionId, CommunicatorName = userName };
            if (rooms.TryGetValue(roomName, out Tuple<HashSet<Communicator>, string> value))
            {
                value.Item1.Add(communicator);
            }
            else
                rooms.Add(roomName, Tuple.Create(new HashSet<Communicator> { communicator }, connectionId));
        }
        public void RemoveGroup(string roomName) => rooms.Remove(roomName);
        public List<string> GetRooms()
        {
            return rooms.Count > 0 ? rooms.Keys.Select(x => x.Split('_')[0]).ToList() : new List<string>();
        }
        public (string?, string?) DisconnectUser(string connectionId)
        {
           var userConnections = rooms.Where(w => w.Value.Item1
                                .Any(x => x.CommunicatorConnectionId == connectionId))
                               .Select(y => new    { key = y.Key, communicator = y.Value.Item1
                               .First(z=> z.CommunicatorConnectionId.Equals(connectionId)),
                 }).ToList();

            if (userConnections is null)
            {
                return ("", "");
            }
            var userConnection = userConnections[0];
            userConnections.ForEach(connection => {  rooms[connection.key].Item1.Remove(connection.communicator);  });
            return (userConnection.key,userConnection.communicator.CommunicatorName);
        }

    }
}
