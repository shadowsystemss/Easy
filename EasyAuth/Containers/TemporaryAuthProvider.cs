using EasyAuth.Abstractions;
using EasyAuth.Models;

namespace EasyAuth.Containers;

public class TemporaryAuthProvider(IAuthorizeProvider authProvider, TimeSpan duration, TimeSpan checking, IDateKeeper? lastDateKeeper = null) : IAuthorizeProvider
{
    private readonly IAuthorizeProvider _authProvider = authProvider;
    private readonly TimeSpan _duration = duration;
    private readonly TimeSpan _checking = checking;
    private readonly IDateKeeper _dateKeeper = lastDateKeeper ?? new SimpleDateKeeper();

    public async Task<bool> AuthorizeTestAsync(CancellationToken ct)
    {
        Task<bool>? task = null;
        TimeSpan delta = DateTime.Now - await _dateKeeper.GetDate();

        if (delta > _checking)
            task = Update(ct);

        if (delta < _duration)
            return true;

        if (task is null)
            return false;

        return await task;
    }

    private async Task<bool> Update(CancellationToken ct)
    {
        if (!await _authProvider.AuthorizeTestAsync(ct))
            return false;

        await _dateKeeper.SetDate(DateTime.Now);
        return true;
    }
}
