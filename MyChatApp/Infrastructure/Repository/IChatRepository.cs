using System.Collections.Generic;
using System.Threading.Tasks;
using MyChatApp.Models;


namespace MyChatApp.Infrastructure.Repository
{
    public interface IChatRepository
    {
        Chat GetChat(int id);
        Task CreateRoom(string name, string userId);
        Task JoinRoom(int chatId, string userId);
        IEnumerable<Chat> GetChats(string userId);
        Task<int> CreatePrivateRoom(string rootId, string targetId);
        IEnumerable<Chat> GetPrivateChats(string userId);

        Task<Message> CreateMessage(int chatId, string message, string userId);
    }
}
