using Microsoft.EntityFrameworkCore;
using System.IO;

namespace StormingCompetition.Models;

public class DataContext : DbContext
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public DbSet<User> Users { get; set; }
    public DbSet<UserLog> UsersLog { get; set; }

    public string DbPath { get; }

    public DataContext(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
        //var folder = Environment.SpecialFolder.LocalApplicationData;
        //var path = Environment.GetFolderPath(folder);
        //DbPath = System.IO.Path.Join(path, "StormingCompetition.db");
        var folder = _webHostEnvironment.ContentRootPath;
        var path = $"{folder}/Data";
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
