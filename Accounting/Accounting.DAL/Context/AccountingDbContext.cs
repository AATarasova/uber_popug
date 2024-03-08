using Accounting.Domain.Models;
using Microsoft.EntityFrameworkCore;
using DbTask = Accounting.Domain.Models.Task;

namespace Accounting.DAL.Context;

public sealed class AccountingDbContext : DbContext
{
    internal DbSet<DbTask> Tasks { get; set; } = null!;
    internal DbSet<Account> Accounts { get; set; } = null!;
    internal DbSet<Transaction> Transactions { get; set; } = null!;

    public AccountingDbContext()
    {
        Database.EnsureCreated();

    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Database=popug_accounting;Username=postgres;Password=34767;");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbTask>(entity =>
        {
            entity.HasKey(e => e.TaskId);
            entity.Property(e => e.AssignmentPrice).HasDefaultValueSql("random() * 10 + 10");
            entity.Property(e => e.CompletionPrice).HasDefaultValueSql("random() * 20 + 20");
        });
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => e.EmployeeId).IsUnique();
            entity.Property(e => e.Balance).HasDefaultValue(0).IsRequired();
            entity.Property(e => e.EmployeeId).IsRequired();
        });
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.TransactionDate).HasDefaultValueSql("NOW()");
            entity.Property(e => e.TransactionType).IsRequired();
            entity.Property(e => e.TargetAccountId).IsRequired();
        });
    }
}   