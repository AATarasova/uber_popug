using Dashboard.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DAL.Context;

public sealed class DashboardDbContext : DbContext
{
    internal DbSet<CompanyAccountHistoryItem> CompanyAccount { get; set; } = null!;
    internal DbSet<EmployeesProductivityHistoryItem> EmployeesProductivity { get; set; } = null!;
    internal DbSet<TasksRatingHistoryItem> TasksRating { get; set; } = null!;

    public DashboardDbContext()
    {
        Database.EnsureCreated();

    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Database=popug_dashboard;Username=postgres;Password=34767;");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompanyAccountHistoryItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Balance).IsRequired();
            entity.Property(e => e.Date).IsRequired();
        });
        modelBuilder.Entity<EmployeesProductivityHistoryItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.EmployeesNumber).IsRequired();
            entity.Property(e => e.UnproductiveEmployeesNumber).IsRequired();
            entity.Property(e => e.Date).IsRequired();
        });
        modelBuilder.Entity<TasksRatingHistoryItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Cost).IsRequired();
            entity.Property(e => e.TaskId).IsRequired();
            entity.Property(e => e.Date).IsRequired();
        });
    }
}   