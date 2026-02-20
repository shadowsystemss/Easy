using EasyDb.Abstractions;
using EasyDb.Advanced.Models;

namespace EasyDb.Advanced.Services;

public class DbMonopolyContainer(IDataBase db) : IDataBase
{
    private readonly IDataBase _db = db;
    private readonly ReaderWriterLockSlim _locker = new();

    public IDbContext Read()
    {
        _locker.EnterReadLock();
        return new DbContextMonopolyProxy(_db.Read(), _locker.ExitReadLock);
    }

    public IDbContext Write()
    {
        _locker.EnterWriteLock();
        return new DbContextMonopolyProxy(_db.Write(), _locker.ExitWriteLock);
    }

    public IDbContext Transaction()
    {
        _locker.EnterWriteLock();
        return new DbContextMonopolyProxy(_db.Transaction(), _locker.ExitWriteLock);
    }

    public IUpgradeableDbContext UpgradeableRead()
    {
        _locker.EnterUpgradeableReadLock();
        return new UpgradableDbContextMonopolyProxy(
            _db.UpgradeableRead(),
            _locker.ExitUpgradeableReadLock,
            _locker.EnterWriteLock,
            _locker.ExitWriteLock);
    }

    public void Dispose()
    {
        _db.Dispose();
        _locker.Dispose();
        GC.SuppressFinalize(this);
    }
}
