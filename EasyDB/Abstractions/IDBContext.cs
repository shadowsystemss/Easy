using System.Data.Common;

namespace EasyDB.Abstractions;

public interface IDBContext : IDisposable
{
    DbCommand CreateCommand();
}