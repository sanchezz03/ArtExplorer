using ArtExplorer.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArtExplorer.DAL.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public DbSet<User> Users { get; set; }
    public AppDbContext() { }
    public AppDbContext(DbContextOptions options) : base(options)
    {

    }

    #region Protected methods

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=artexplorer.database;Port=5432;Database=ArtExplorer;Username=admin;Password=YourStrong!Passw0rd;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    #endregion
}
