namespace PersistCommunicator.Abstractions
{
    public interface IChatManager
    {
        void AddGroup(string roomName, string userName, string connectionId);
        Tuple<string?, string> DisconnectUser(string connectionId);
        List<string> GetRooms();
        void RemoveGroup(string roomName);
        string roomCreatedMessage(string UserName, string roomName);
        string roomJoinedMessage(string UserName);
        string userLeftMessage(string UserName);
    }
}