using System.Data.Common;
using EasyDB.Abstractions;

namespace EasyDB.Models;

public class DBContext(DbConnection connection, Action? dispose) : IDBContext
{
    protected DbConnection _connection = connection;
    protected Action? _dispose = dispose;

    public DbCommand CreateCommand()
        => _connection.CreateCommand();
    public virtual void Dispose()
    {
        _dispose?.Invoke();
        GC.SuppressFinalize(this);
    }
}
