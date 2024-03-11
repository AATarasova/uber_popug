using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using TaskTracker.Domain.Models;
using DbTask = TaskTracker.Domain.Models.Task;

namespace TaskTracker.DAL.Context;

public sealed class TaskTrackerDbContext : DbContext
{
    internal DbSet<DbTask> Tasks { get; set; } = null!;
    internal DbSet<Employee> Employees { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Database=popug_task_tracker;Username=postgres;Password=34767;");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbTask>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.PublicId).Metadata.SetValueGeneratorFactory((_, _) => new SequentialGuidValueGenerator());;
            entity.Property(e => e.DeveloperId).IsRequired();
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("NOW()");
        });
    }
}   