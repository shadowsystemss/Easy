using EasyAuth.Abstractions;

namespace EasyAuth.Containers;

public class TimeoutAuthProvider(IAuthorizeProvider authProvider, TimeSpan timeout) : IAuthorizeProvider
{
    private readonly IAuthorizeProvider _authProvider = authProvider;
    private readonly TimeSpan _timeout = timeout;

    public async Task<bool> AuthorizeTestAsync(CancellationToken ct)
    {
        Task<bool> task = _authProvider.AuthorizeTestAsync(ct);
        await Task.WhenAny(task, Task.Delay(_timeout, ct));

        return task.IsCompletedSuccessfully && task.Result;
    }
}
