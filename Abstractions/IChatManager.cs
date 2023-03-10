using System.Collections;

namespace PersistCommunicator.Abstractions
{
    public interface IChatManager
    {
        string AddOrUpdateRoom(string roomName, string userName, string connectionId);
        (string?, string) DisconnectUser(string connectionId);
        List<string> GetRooms();
        void RemoveGroup(string roomName);
        ArrayList roomCreatedResponseToOthers(string UserName, string roomName,string createdRoomKey);
        ArrayList roomCreatedResponseToOwner(string UserName, string roomName,string createdRoomKey);

        string roomJoinedMessage(string UserName);
        string userLeftMessage(string UserName);
    }
}