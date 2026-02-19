using System.Data.Common;

namespace EasyDb.Abstractions;

public interface IDbConnector
{
    DbConnection Connect();
}
