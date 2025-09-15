namespace EasyAuth.Abstractions;

public interface IGuidProvider
{
    Task<Guid> GetGuidAsync(CancellationToken cancellationToken);
}
