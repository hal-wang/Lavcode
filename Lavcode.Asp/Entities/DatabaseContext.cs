using Microsoft.EntityFrameworkCore;

namespace Lavcode.Asp.Entities
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<FolderEntity> Folders { get; set; } = null!;
        public DbSet<PasswordEntity> Passwords { get; set; } = null!;
        public DbSet<IconEntity> Icons { get; set; } = null!;
        public DbSet<KeyValuePairEntity> KeyValuePairs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
