namespace Subutai.Domain.Model.Auditing;

public interface IHasCreationTime
{
    DateTimeOffset CreatedAt { get; set; }
}
