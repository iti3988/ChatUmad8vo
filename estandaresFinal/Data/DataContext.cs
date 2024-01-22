using estandaresFinal.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace estandaresFinal.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Capturer> Capturers { get; set; }
        public DbSet<Career> Careers { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMember> ChatMembers { get; set; }
        public DbSet<ChatType> ChatTypes { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var cascadeFKs = builder
                .Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(builder);
            builder.Entity<Career>().HasIndex(c => c.Name).IsUnique();

            builder.Entity<ChatMember>()
                .HasOne(ch => ch.Chat)
                .WithMany(c => c.ChatMembers)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
