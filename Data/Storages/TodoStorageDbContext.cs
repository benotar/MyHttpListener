using HttpLearning.Entities;
using Microsoft.EntityFrameworkCore;

namespace HttpLearning.Data.Storages;

public class TodoStorageDbContext : DbContext
{
    public TodoStorageDbContext()
    {
        Database.EnsureCreated();
    }
    
    public DbSet<Todo> Todoes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = TodoStorage.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>().HasData(
            new Todo { Title = "First Test Todo From EF Core" }, 
            new Todo { Title = "Second Test Todo From EF Core" });
    }
}