using Microsoft.EntityFrameworkCore;

namespace TelegramBot.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<EmployeeViewModel> Employees { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => 
        optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=TestDB_ForTGBot;Trusted_Connection=True;MultipleActiveResultSets=true");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeViewModel>()
            .HasKey(e => e.O_WITEM_ID); // Определение первичного ключа

        base.OnModelCreating(modelBuilder);
    }
}
