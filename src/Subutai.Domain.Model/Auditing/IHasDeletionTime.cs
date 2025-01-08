namespace Subutai.Domain.Model.Auditing;

public interface IHasDeletionTime
{
    DateTimeOffset? DeletedAt { get; set; }
}
