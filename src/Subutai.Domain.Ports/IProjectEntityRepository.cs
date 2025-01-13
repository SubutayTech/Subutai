using Subutai.Domain.Model;

namespace Subutai.Domain.Ports;

public interface IProjectEntityRepository
{
    Task<ProjectEntity> AddAsync(ProjectEntity entity);
    Task<ProjectEntity> UpdateAsync(ProjectEntity entity);
    Task<ProjectEntity> DeleteAsync(ProjectEntity entity);
}
