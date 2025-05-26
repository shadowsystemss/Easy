using System.Data.Common;

namespace EasyDB.Abstractions;

public interface IDBContext : IDisposable
{
    DbCommand CreateCommand(string sql);

    int Execute(string sql);

    object? Scalar(string sql);

    T ReaderWrapper<T>(string sql, Func<DbDataReader, T> action);
}