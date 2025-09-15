using EasyDB.Abstractions;
using EasyDB.Advanced.Models;

namespace EasyDB.Advanced.Services;

public class DBMonopolyContainer(IDataBase db) : IDataBase
{
    private readonly IDataBase _db = db;
    private readonly ReaderWriterLockSlim _locker = new();

    public void Dispose()
    {
        _db.Dispose();
        GC.SuppressFinalize(this);
    }

    public IDBContext Read()
    {
        _locker.EnterReadLock();
        var context = _db.Read();
        _locker.ExitReadLock();
        return context;
    }

    public IDBContext Transaction()
    {
        _locker.EnterReadLock();
        var context = _db.Transaction();
        _locker.ExitReadLock();
        return context;
    }

    public IUpgradeableDBContext UpgradeableRead()
    {
        _locker.EnterReadLock();
        var context = _db.UpgradeableRead();
        _locker.ExitReadLock();
        return context;
    }

    public IDBContext Write()
    {
        _locker.EnterReadLock();
        var context = _db.Write();
        _locker.ExitReadLock();
        return context;
    }

    public IDBContext Monopoly()
    {
        _locker.EnterWriteLock();
        return new DBContextProxy(_db.Write(), _locker.ExitWriteLock);
    }
}
