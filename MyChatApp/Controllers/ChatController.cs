using Azure.Core.Pipeline;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MyChatApp.Database;
using MyChatApp.Hubs;
using MyChatApp.Models;

namespace MyChatApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private IHubContext<ChatHub> _chat;
        public ChatController(
            IHubContext<ChatHub> chat
        )
        {
            _chat = chat;
        }
        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomName)
        {
            await _chat.Groups.AddToGroupAsync(connectionId, roomName);
            return Ok();
        }
        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> LeaveRoom(string connectionId, string roomName)
        {
            await _chat.Groups.RemoveFromGroupAsync(connectionId, roomName);
            return Ok();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(
            string message,
            string roomName,
            int chatId,
            [FromServices] AppDbContext ctx)
        {
            var Message = new Message
            {
                ChatId = chatId,
                Text = message,
                Timestamp = DateTime.Now,
                UserName = User.Identity.Name
            };
            ctx.Messages.Add(Message);
            await ctx.SaveChangesAsync();
            await _chat.Clients.Group(roomName)
                .SendAsync("ReceiveMessage", new
                {
                    Text = Message.Text,
                    Name = Message.UserName,
                    TimeStamp = Message.Timestamp.ToString("dd/MM/yyyy hh:mm:ss")
                });
            return Ok();
        }
    }
}
