using System.Data.Common;
using EasyDB.Abstractions;
using EasyDB.Models;

namespace EasyDB.Advanced.Services;

public class OneTimeDatabase(Func<DbConnection> connectionFactory) : IDataBase
{
    protected Func<DbConnection> _connectionFactory = connectionFactory;

    public OneTimeDatabase(IDBConnector connector)
        : this(connector.Connect) { }

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
