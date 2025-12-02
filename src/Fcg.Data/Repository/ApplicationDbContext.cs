using Fcg.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fcg.Data.Repository;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Game> Games { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("Clientes");
            e.HasKey(u => u.Id);
            e.Property(u => u.Id).HasColumnType("UNIQUEIDENTIFIER");
            e.Property(u => u.CreatedAt).HasColumnType("DATETIME").IsRequired();
            e.Property(u => u.LastUpdatedAt).HasColumnType("DATETIME").IsRequired();
            e.Property(u => u.Name).HasColumnType("VARCHAR(100)").IsRequired();
            e.Property(u => u.Email).HasColumnType("VARCHAR(100)").IsRequired();
            e.Property(u => u.Password).HasColumnType("VARCHAR(300)").IsRequired();
            e.Property(u => u.Role).HasColumnType("VARCHAR(20)").IsRequired().HasDefaultValue("User");

            e.HasMany(u => u.Games)
             .WithMany()
             .UsingEntity(j => j.ToTable("UserGames"));
        });

        modelBuilder.Entity<Game>(e =>
        {
            e.ToTable("Games");
            e.HasKey(u => u.Id);
            e.Property(u => u.Id).HasColumnType("UNIQUEIDENTIFIER");
            e.Property(u => u.CreatedAt).HasColumnType("DATETIME").IsRequired();
            e.Property(u => u.LastUpdatedAt).HasColumnType("DATETIME").IsRequired();
            e.Property(u => u.Name).HasColumnType("VARCHAR(100)").IsRequired();
            e.Property(u => u.Description).HasColumnType("VARCHAR(100)").IsRequired();
            e.Property(u => u.Price).HasColumnType("DECIMAL(18,2)").IsRequired();
        });
    }


}
