using System.Data.Common;

namespace EasyDB.Abstractions;

public interface IDBConnector
{
    public DbConnection Connect();
}
