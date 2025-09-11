using Microsoft.EntityFrameworkCore;

using DbApi.Models;

namespace DbApi.DbContexts;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<User> users { get; set; }
}
