using System.Data.Common;
using EasyDB.Abstractions;
using EasyDB.Models;

namespace EasyDB.Services;

public class OneTimeDatabase : IDataBase
{
    protected Func<DbConnection> _connectionFactory;

    public OneTimeDatabase(IDBConnector connector)
        : this(connector.Connect) { }

    public OneTimeDatabase(Func<DbConnection> connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void Dispose() => GC.SuppressFinalize(this);

    private DBContext Connect()
    {
        DbConnection connection = _connectionFactory.Invoke();
        connection.Open();
        return new DBContext(connection, () => { connection.Close(); connection.Dispose(); });
    }

    public IDBContext Read() => Connect();

    public IDBContext Write() => Connect();

    public IUpgradeableDBContext UpgradeableRead()
    {
        DbConnection connection = _connectionFactory.Invoke();
        return new UpgradeableDBContext(this, connection, () => { connection.Close(); connection.Dispose(); });
    }

    public IDBContext Transaction()
    {
        DbConnection connection = _connectionFactory.Invoke();
        return new TransactionDBContext(connection, () => { connection.Close(); connection.Dispose(); });
    }
}
