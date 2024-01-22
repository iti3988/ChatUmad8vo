namespace estandaresFinal.Data.Entities
{
    public class Coordinator : IEntity
    {
        public int Id { get; set; }
        public User? User { get; set; }
    }
}
