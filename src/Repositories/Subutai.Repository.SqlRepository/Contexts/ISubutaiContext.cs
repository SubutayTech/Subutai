using Microsoft.EntityFrameworkCore;
using Subutai.Domain.Model;

namespace Subutai.Repository.SqlRepository.Contexts;

public interface ISubutaiContext
{
    DbSet<ProjectEntity> Projects { get; set; }
    DbSet<DepartmentEntity> Departments { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
