using EasyDb.Abstractions;
using EasyDb.Advanced.Models;

namespace EasyDb.Advanced.Services;

public class DbMonopolyContainer(IDataBase db) : IDataBase
{
    private readonly IDataBase _db = db;
    private readonly ReaderWriterLockSlim _locker = new();

    public void Dispose()
    {
        _db.Dispose();
        GC.SuppressFinalize(this);
    }

    public IDbContext Read()
    {
        _locker.EnterReadLock();
        var context = _db.Read();
        _locker.ExitReadLock();
        return context;
    }

    public IDbContext Transaction()
    {
        _locker.EnterReadLock();
        var context = _db.Transaction();
        _locker.ExitReadLock();
        return context;
    }

    public IUpgradeableDbContext UpgradeableRead()
    {
        _locker.EnterReadLock();
        var context = _db.UpgradeableRead();
        _locker.ExitReadLock();
        return context;
    }

    public IDbContext Write()
    {
        _locker.EnterReadLock();
        var context = _db.Write();
        _locker.ExitReadLock();
        return context;
    }

    public IDbContext Monopoly()
    {
        _locker.EnterWriteLock();
        return new DbContextProxy(_db.Write(), _locker.ExitWriteLock);
    }
}
