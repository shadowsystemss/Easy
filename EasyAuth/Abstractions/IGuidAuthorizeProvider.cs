namespace EasyAuth.Abstractions;

public interface IGuidAuthorizeProvider
{
    Task<bool> AuthorizeTestAsync(Guid guid, CancellationToken cancel);
}
