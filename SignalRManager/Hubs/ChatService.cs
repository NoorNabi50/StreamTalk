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
        public async Task CreateRoom(string userName,String room) {
            string createdRoomKey = await CreateOrJoinRoom(room,userName);
            await Clients.Others.SendAsync("newRoomCreated", chatManager.roomCreatedResponseToOthers(userName, room, createdRoomKey));
            await Clients.Caller.SendAsync("roomCreated", chatManager.roomCreatedResponseToOwner(userName, room, createdRoomKey));
        }  
        public async Task JoinRoom(string userName, string room)
        {
            await CreateOrJoinRoom(room,userName);
            await Clients.OthersInGroup(room).SendAsync("joinedRoom", chatManager.roomJoinedMessage(userName));
        }
        public async Task SendMessage(string userName,string message,string room)
        {
               await Clients.OthersInGroup(room).SendAsync("receiveMessage", message);
        }
        public override async Task OnDisconnectedAsync(Exception? e)
        {
           var disconnetedUserInfo =  chatManager.DisconnectUser(Context.ConnectionId);
           await Groups.RemoveFromGroupAsync(Context.ConnectionId, disconnetedUserInfo.Item1);
           await Clients.OthersInGroup(disconnetedUserInfo.Item1).SendAsync("notifyUsers", chatManager.userLeftMessage(disconnetedUserInfo.Item2));
           await base.OnDisconnectedAsync(e);
        }
        private async Task<string> CreateOrJoinRoom(string room,string userName)
        {
            room =  chatManager.AddOrUpdateRoom(room, userName, Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
            return room;
        }

    }


}
