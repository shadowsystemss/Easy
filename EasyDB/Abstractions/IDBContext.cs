using System.Data.Common;

namespace EasyDb.Abstractions;

public interface IDbContext : IDisposable
{
    DbCommand CreateCommand();
}