using Microsoft.EntityFrameworkCore;

using DbConnection.Models;

namespace DbConnection.DbContexts;

public class UserDbContext : DbContext  // Microsoft.EntityFrameworkCore 라이브러리를 using해 주어야 한다.
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<User> users { get; set; }
    // 데이터베이스 아래로 users라는 릴레이션이 생성될 것이다.
}
