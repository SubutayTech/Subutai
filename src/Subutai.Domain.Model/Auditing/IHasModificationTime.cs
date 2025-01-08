namespace Subutai.Domain.Model.Auditing;

public interface IHasModificationTime
{
    DateTimeOffset? UpdatedAt { get; set; }
}
