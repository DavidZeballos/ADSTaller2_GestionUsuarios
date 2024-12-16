using Microsoft.EntityFrameworkCore;
using src.Domain.Entities;

namespace src.Infrastructure.Persistence
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.SubjectIdsJson)
                .HasColumnType("jsonb"); // PostgreSQL soporta columnas JSONB
        }
    }
}
