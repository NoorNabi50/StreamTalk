using Microsoft.AspNetCore.SignalR;
using PersistCommunicator.Abstractions;
using PersistCommunicator.Models;

namespace PersistCommunicator.SignalRManager.Hubs
{
    public class ChatHub : Hub
    {
        // First User have to create a group/room to begin communication

        private readonly IChatManager chatManager;

        public ChatHub(IChatManager _chatManager)
        {
            chatManager = _chatManager;
        }
        public async Task createRoom(string userName,String roomName) {
            string room = await createOrJoinRoom(roomName,userName);
            await Clients.All.SendAsync("roomCreated", chatManager.roomCreatedMessage(userName, roomName), roomName);
        }
        public async Task joinRoom(string userName, string roomName)
        {
            await createOrJoinRoom(roomName,userName);
            await Clients.OthersInGroup(roomName).SendAsync("joinedRoom", chatManager.roomJoinedMessage(userName));
        }
        public async Task sendMessage(string userName,string message,string roomName)
        {
               await Clients.OthersInGroup(roomName).SendAsync("receiveMessage", message);
        }
        public override async Task OnDisconnectedAsync(Exception? e)
        {
           var disconnetedUserInfo =  chatManager.DisconnectUser(Context.ConnectionId);
            if (disconnetedUserInfo != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, disconnetedUserInfo.Item1);
                await Clients.OthersInGroup(disconnetedUserInfo.Item1).SendAsync("notifyUsers", chatManager.userLeftMessage(disconnetedUserInfo.Item2));
            }
           await base.OnDisconnectedAsync(e);
        }
        private async Task<string> createOrJoinRoom(string roomName,string userName)
        {
            string room = roomName + "_" + Guid.NewGuid().ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
            chatManager.AddGroup(room, userName, Context.ConnectionId);
            return room;
        }



    }


}
