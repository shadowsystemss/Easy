using System.Data.Common;

namespace EasyDB.Abstractions;

public interface IDBConnector
{
    DbConnection Connect();
}
