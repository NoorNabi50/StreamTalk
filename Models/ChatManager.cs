using PersistCommunicator.Abstractions;

namespace PersistCommunicator.Models
{
    public class ChatManager : IChatManager
    {
        public static Dictionary<string, Tuple<HashSet<Communicator>, string>> rooms = new();
        public string roomCreatedMessage(string UserName, string roomName) => $"Hey {UserName} ! Room {roomName} has been created";
        public string roomJoinedMessage(string UserName) => $"{UserName} Joined the Session";
        public string userLeftMessage(string UserName) => $"{UserName} left the Session";
        public void AddGroup(string roomName, string userName, string connectionId)
        {
            Communicator communicator = new() { CommunicatorConnectionId = connectionId, CommunicatorName = userName };
            if (rooms.ContainsKey(roomName))
                rooms[roomName].Item1.Add(communicator);
            else
                rooms.Add(roomName, Tuple.Create(new HashSet<Communicator> { communicator }, connectionId));
        }
        public void RemoveGroup(string roomName) => rooms.Remove(roomName);
        public List<string> GetRooms()
        {
            return rooms.Count > 0 ? rooms.Keys.ToList().ConvertAll(x => x.Split('_')[0].ToString()) : new List<string>();
        }
        public Tuple<string?, string> DisconnectUser(string connectionId)
        {
           var userConnection =  rooms.Where(w => w.Value.Item1.Any(x => x.CommunicatorConnectionId == connectionId))
                 .Select(y => new
                  { key = y.Key,
                     communicator = y.Value.Item1.First(z=> z.CommunicatorConnectionId.Equals(connectionId)),
                 }).FirstOrDefault();

            if(userConnection != null)
            {
                return Tuple.Create(userConnection.key, userConnection.communicator.CommunicatorName);   
            }

            return null;
        }

    }
}
