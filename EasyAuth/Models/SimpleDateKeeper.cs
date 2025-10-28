using EasyAuth.Abstractions;

namespace EasyAuth.Models;

public class SimpleDateKeeper : IDateKeeper
{
    private DateTime _lastDate;

    public SimpleDateKeeper() { }

    public SimpleDateKeeper(DateTime lastDate) => _lastDate = lastDate;

    public Task<DateTime> GetDate() => Task.FromResult(_lastDate);

    public Task SetDate(DateTime date)
    {
        _lastDate = date;
        return Task.CompletedTask;
    }
}
