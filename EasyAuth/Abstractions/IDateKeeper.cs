namespace EasyAuth.Abstractions;

public interface IDateKeeper
{
    Task<DateTime> GetDate();
    Task SetDate(DateTime date);
}
