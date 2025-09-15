using EasyAuth.Abstractions;

namespace EasyAuth.Containers;

public class TemporaryAuthProvider(IAuthorizeProvider authProvider, TimeSpan duration, TimeSpan checking, DateTime lastAuth = default) : IAuthorizeProvider
{
    private readonly IAuthorizeProvider _authProvider = authProvider;
    private readonly TimeSpan _duration = duration;
    private readonly TimeSpan _checking = checking;

    public DateTime LastAuth = lastAuth;

    public Task<bool> AuthorizeTestAsync(CancellationToken cancel)
    {
        Task<bool>? task = null;
        TimeSpan delta = DateTime.Now - LastAuth;

        if (delta > _checking)
            task = Update(cancel);

        if (delta < _duration)
            return Task.FromResult(true);

        return task is not null && task.IsCompletedSuccessfully ? task : Task.FromResult(false);
    }

    private async Task<bool> Update(CancellationToken cancel)
    {
        if (!await _authProvider.AuthorizeTestAsync(cancel))
            return false;

        LastAuth = DateTime.Now;
        return true;
    }
}
