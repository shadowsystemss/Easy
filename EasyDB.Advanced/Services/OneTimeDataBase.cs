using EasyDb.Abstractions;
using EasyDb.Models;
using System.Data.Common;

namespace EasyDb.Advanced.Services;

public class OneTimeDatabase(Func<DbConnection> connectionFactory) : IDataBase
{
    protected Func<DbConnection> _connectionFactory = connectionFactory;

    public OneTimeDatabase(IDbConnector connector)
        : this(connector.Connect) { }

    public void Dispose() => GC.SuppressFinalize(this);

    private DbContext Connect()
    {
        DbConnection connection = _connectionFactory.Invoke();
        connection.Open();
        return new DbContext(connection, () => { connection.Close(); connection.Dispose(); });
    }

    public IDbContext Read() => Connect();

    public IDbContext Write() => Connect();

    public IUpgradeableDbContext UpgradeableRead()
    {
        DbConnection connection = _connectionFactory.Invoke();
        return new UpgradeableDbContext(this, connection, () => { connection.Close(); connection.Dispose(); });
    }

    public IDbContext Transaction()
    {
        DbConnection connection = _connectionFactory.Invoke();
        return new TransactionDbContext(connection, () => { connection.Close(); connection.Dispose(); });
    }
}
