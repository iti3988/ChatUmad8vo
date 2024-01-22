namespace estandaresFinal.Data.Entities
{
    public class ChatMember : IEntity
    {
        public int Id { get; set; }
        public Chat? Chat { get; set; }
        public User? User { get; set; }
    }
}
