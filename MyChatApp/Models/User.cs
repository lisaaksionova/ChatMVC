using Microsoft.AspNetCore.Identity;

namespace MyChatApp.Models
{
    public class User : IdentityUser
    {
        public User() : base()
        {
            Chats = new List<ChatUser>();
        }
        public ICollection<ChatUser> Chats { get; set; }
    }
}
