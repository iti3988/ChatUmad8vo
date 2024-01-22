namespace estandaresFinal.Data.Entities
{
    public class Teacher : IEntity
    {
        public int Id { get; set; }
        public User? User { get; set; }
    }
}
