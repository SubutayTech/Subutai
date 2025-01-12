using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Subutai.Domain.Model;

namespace Subutai.Repository.SqlRepository.Contexts;

public class SubutaiContext : DbContext, ISubutaiContext
{
    public DbSet<ProjectEntity> Projects { get; set; } = null!;
    public DbSet<DepartmentEntity> Departments { get; set; } = null!;

    public SubutaiContext(DbContextOptions<SubutaiContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
