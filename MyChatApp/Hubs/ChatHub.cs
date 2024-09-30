using Microsoft.AspNetCore.SignalR;

namespace MyChatApp.Hubs
{
    public class ChatHub : Hub
    {
        public string GetConnectionId()=>
            Context.ConnectionId;
    }
}
