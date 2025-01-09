using Subutai.Domain.Model.Auditing;

namespace Subutai.Domain.Model;

public class ProjectEntity : IHasCreationTime, IHasModificationTime, IHasDeletionTime
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public int? DepartmentId { get; set; }
    public DateTimeOffset DateStarted { get; set; }
    public DateTimeOffset? DateCompleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public DepartmentEntity? Department { get; set; }
}
