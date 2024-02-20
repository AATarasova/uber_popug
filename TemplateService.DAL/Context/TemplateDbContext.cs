using Microsoft.EntityFrameworkCore;
using TemplateService.Domain.Models;

namespace TemplateService.DAL.Context;

public sealed class TemplateDbContext : DbContext
{
    internal DbSet<Template> Templates { get; set; } = null!;

    public TemplateDbContext()
    {
        Database.EnsureCreated();

    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Database=service_template;Username=postgres;Password=34767;");
    }
}   