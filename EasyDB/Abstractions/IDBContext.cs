using System.Data.Common;

namespace EasyDb.Abstractions;

public interface IDBContext : IDisposable
{
    DbCommand CreateCommand();
}