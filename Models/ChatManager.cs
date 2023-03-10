using PersistCommunicator.Abstractions;
using System.Collections;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PersistCommunicator.Models
{
    public class ChatManager : IChatManager
    {
        private static Dictionary<string, Tuple<HashSet<Communicator>, string>> rooms { get; set; }

        public ChatManager()
        {
            rooms = new Dictionary<string, Tuple<HashSet<Communicator>, string>>();
        }

        public ArrayList roomCreatedResponseToOwner(string UserName, string room,string createdRoomKey)
        {
            return new ArrayList()
            {
                {"Your Room Has Been Created Successfully!"},
                { $"<tr><td>{room}</td><td><button onclick='JoinRoom({createdRoomKey})' class='btn btn-info joinBtn'>Join</button><button class='btn btn-danger deleteBtn ml-5' onclick='deleteRoom({createdRoomKey})'>Remove</button></td></tr>" }
            };
        }
        public ArrayList roomCreatedResponseToOthers(string UserName, string room, string createdRoomKey)
        {
            return new ArrayList()
            {
                {$"A New Room Has been Created By {UserName} " },
                { $"<tr><td>{room}</td><td><button data-room='{createdRoomKey}' class='btn btn-info joinBtn'>Join</button></td></tr>"}
            };
        }
        public string roomJoinedMessage(string UserName) => $"{UserName} Joined the Room";
        public string userLeftMessage(string UserName) => $"{UserName} left the Room";
        public string AddOrUpdateRoom(string room, string userName, string connectionId)
        {
            Communicator communicator = new() { CommunicatorConnectionId = connectionId, CommunicatorName = userName };
            if (rooms.TryGetValue(room, out Tuple<HashSet<Communicator>, string> value))
            {
                value.Item1.Add(communicator);
            }
            else
            {
                string newCreatedRoom = room + "_" + Guid.NewGuid().ToString();
                rooms.Add(newCreatedRoom, Tuple.Create(new HashSet<Communicator> { communicator }, connectionId));
                return newCreatedRoom;
            }
            return room;
        }
        public void RemoveGroup(string room) => rooms.Remove(room);
        public List<string> GetRooms()
        {
            return rooms.Count > 0 ? rooms.Keys.ToList() : new List<string>();
        }
        public (string?, string?) DisconnectUser(string connectionId)
        {
            var userConnections = rooms.Where(w => w.Value.Item1
                                 .Any(x => x.CommunicatorConnectionId == connectionId))
                                .Select(y => new
                                {
                                    key = y.Key,
                                    communicator = y.Value.Item1
                                .First(z => z.CommunicatorConnectionId.Equals(connectionId)),
                                }).ToList();

            if (userConnections is null)
            {
                return ("", "");
            }
            var userConnection = userConnections[0];
            userConnections.ForEach(connection => { rooms[connection.key].Item1.Remove(connection.communicator); });
            return (userConnection.key, userConnection.communicator.CommunicatorName);
        }
    }
}
