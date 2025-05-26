using System.Data.Common;
using EasyDB.Abstractions;

namespace EasyDB.Models;

public class DBContext : IDBContext
{
    protected DbConnection _connection;
    protected Action? _dispose;

    public DBContext(DbConnection connection, Action? dispose)
    {
        _connection = connection;
        _dispose = dispose;
    }

    public DbCommand CreateCommand(string sql)
    {
        DbCommand command = _connection.CreateCommand();
        command.CommandText = sql;
        return command;
    }

    public int Execute(string sql)
    {
        using DbCommand command = CreateCommand(sql);
        return command.ExecuteNonQuery();
    }

    public object? Scalar(string sql)
    {
        using DbCommand command = CreateCommand(sql);
        return command.ExecuteScalar();
    }

    public T ReaderWrapper<T>(string sql, Func<DbDataReader, T> action)
    {
        using DbCommand command = CreateCommand(sql);
        using DbDataReader reader = command.ExecuteReader();
        return action(reader);
    }

    public virtual void Dispose()
    {
        _dispose?.Invoke();
        GC.SuppressFinalize(this);
    }
}
