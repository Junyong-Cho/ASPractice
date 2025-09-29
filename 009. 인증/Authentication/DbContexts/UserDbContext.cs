using Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentication.DbContexts;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("users").HasIndex(u => u.UserId).IsUnique();
    }
}
