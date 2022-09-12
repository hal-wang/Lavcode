using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

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
            modelBuilder.Entity<FolderEntity>(entity =>
            {
                entity.HasOne(d => d.Icon)
                .WithOne(p => p.Folder)
                    .HasForeignKey<FolderEntity>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Folder_Icon");
            });
            modelBuilder.Entity<PasswordEntity>(entity =>
            {
                entity.HasOne(d => d.Icon)
                .WithOne(p => p.Password)
                    .HasForeignKey<PasswordEntity>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Password_Icon");
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
