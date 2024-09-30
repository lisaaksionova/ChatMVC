namespace MyChatApp.Models
{
    public class Chat
    {

        public int Id { get; set; }
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<ChatUser> Users { get; set; } = new List<ChatUser>();
        public ChatType Type { get; set; }
        public string Name { get; internal set; }
    }
}
