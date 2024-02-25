using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Auth.DAL.Context;

public sealed class AuthDbContext : DbContext
{
    internal DbSet<User> Users { get; set; } = null!;

    public AuthDbContext()
    {
        Database.EnsureCreated();

    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Database=popug_auth;Username=postgres;Password=34767;");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).ValueGeneratedOnAdd().UseIdentityAlwaysColumn();
            entity.Property(e => e.PublicId).Metadata.SetValueGeneratorFactory((_, _) => new SequentialGuidValueGenerator());;
            entity.Property(e => e.Email).HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.Password).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.LastName).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.MiddleName).HasMaxLength(20).IsUnicode(false);
        });
    }
}   