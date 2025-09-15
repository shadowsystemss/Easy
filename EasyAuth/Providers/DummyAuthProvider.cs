using EasyAuth.Abstractions;

namespace EasyAuth.Providers;

public class DummyAuthProvider : IAuthorizeProvider
{
    public Task<bool> AuthorizeTestAsync(CancellationToken cancel)
        => Task.FromResult(true);
}
