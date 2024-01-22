namespace estandaresFinal.Data.Entities
{
    public class Capturer : IEntity
    {
        public int Id { get; set; }
        public User? User { get; set; }
    }
}
