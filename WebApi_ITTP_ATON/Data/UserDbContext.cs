using Microsoft.EntityFrameworkCore;
using WebApi_ITTP_ATON.Entities;

public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
}