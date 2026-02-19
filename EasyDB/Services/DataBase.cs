using EasyDb.Abstractions;
using EasyDb.Models;
using System.Data.Common;

namespace EasyDb.Services;

public class DataBase : IDataBase
{
    protected DbConnection _connection;
    protected ReaderWriterLockSlim _locker;

    public DataBase(IDbConnector connector)
        : this(connector.Connect(), null) { }

    public DataBase(IDbConnector connector, ReaderWriterLockSlim locker)
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

    public IDbContext Read()
    {
        _locker.EnterReadLock();
        return new DbContext(_connection, _locker.ExitReadLock);
    }

    public IDbContext Write()
    {
        _locker.EnterWriteLock();
        return new DbContext(_connection, _locker.ExitWriteLock);
    }

    public IUpgradeableDbContext UpgradeableRead()
    {
        _locker.EnterUpgradeableReadLock();
        return new UpgradeableDbContext(this, _connection, _locker.ExitUpgradeableReadLock);
    }

    public IDbContext Transaction()
    {
        _locker.EnterWriteLock();
        return new TransactionDbContext(_connection, _locker.ExitWriteLock);
    }
}