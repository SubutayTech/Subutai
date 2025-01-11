using Microsoft.EntityFrameworkCore;
using Subutai.Domain.Model;
using Subutai.Domain.Ports;
using Subutai.Repository.SqlRepository.Contexts;

namespace Subutai.Repository.SqlRepository.Repositories;

public class ProjectEntityRepository : IProjectEntityRepository
{
    private readonly ISubutaiContext _context;

    public ProjectEntityRepository(ISubutaiContext context)
    {
        _context = context;
    }
    public async Task<ProjectEntity> AddAsync(ProjectEntity entity)
    {
        await _context.Projects.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public async Task<ProjectEntity> UpdateAsync(ProjectEntity entity)
    {
        var singleEntity = await _context.Projects.FirstOrDefaultAsync(e => e.Id == entity.Id);

        if(singleEntity == null)
       {
        throw new ArgumentNullException(nameof(entity));

       } 
        _context.Projects.Update(singleEntity);
        singleEntity.UpdatedAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();
        return singleEntity;
    }
}
