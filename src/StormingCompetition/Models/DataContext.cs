using Microsoft.EntityFrameworkCore;

namespace StormingCompetition.Models;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserLog> UsersLog { get; set; }

    public string DbPath { get; }

    public DataContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "StormingCompetition.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u  => u.Id);
        modelBuilder.Entity<UserLog>().HasKey(ul => ul.Id);

        modelBuilder.Entity<User>()
            .HasMany(u => u.UserLogs)
            .WithOne(ul => ul.User)
            .HasForeignKey(ul => ul.UserId);
    }
}
