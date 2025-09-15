using EasyAuth.Abstractions;

namespace EasyAuth.Providers;

public class DemoAuthProvider(DateTime term) : IAuthorizeProvider
{
    private readonly DateTime _term = term;

    public Task<bool> AuthorizeTestAsync(CancellationToken cancel)
        => Task.FromResult(DateTime.Now < _term);
}
