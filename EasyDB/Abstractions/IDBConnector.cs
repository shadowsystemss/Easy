using System.Data.Common;

namespace EasyDb.Abstractions;

public interface IDBConnector
{
    DbConnection Connect();
}
