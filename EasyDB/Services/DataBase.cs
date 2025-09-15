using System.Data.Common;
using EasyDB.Abstractions;
using EasyDB.Models;

namespace EasyDB.Services;

public class DataBase : IDataBase
{
    protected DbConnection _connection;
    protected ReaderWriterLockSlim _locker;

    public DataBase(IDBConnector connector)
        : this(connector.Connect(), null) { }

    public DataBase(IDBConnector connector, ReaderWriterLockSlim locker)
        : this(connector.Connect(), locker) { }

    public DataBase(DbConnection connection, ReaderWriterLockSlim? locker)
    {
        _locker = locker ?? new ReaderWriterLockSlim();
        _connection = connection;
        _connection.Open();
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
        _locker.Dispose();
        GC.SuppressFinalize(this);
    }

    public IDBContext Read()
    {
        _locker.EnterReadLock();
        return new DBContext(_connection, _locker.ExitReadLock);
    }

    public IDBContext Write()
    {
        _locker.EnterWriteLock();
        return new DBContext(_connection, _locker.ExitWriteLock);
    }

    public IUpgradeableDBContext UpgradeableRead()
    {
        _locker.EnterUpgradeableReadLock();
        return new UpgradeableDBContext(this, _connection, _locker.ExitUpgradeableReadLock);
    }

    public IDBContext Transaction()
    {
        _locker.EnterWriteLock();
        return new TransactionDBContext(_connection, _locker.ExitWriteLock);
    }
}