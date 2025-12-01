using EasyAuth.Abstractions;

namespace EasyAuth.Providers;

public class DummyAuthProvider : IAuthorizeProvider
{
    public Task<bool> AuthorizeTestAsync(CancellationToken ct)
        => Task.FromResult(true);
}
