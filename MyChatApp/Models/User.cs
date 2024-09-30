using Microsoft.AspNetCore.Identity;

namespace MyChatApp.Models
{
    public class User: IdentityUser
    {
        public ICollection<ChatUser> Chats { get; set; }
    }
}
