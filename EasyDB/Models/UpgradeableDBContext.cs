using System.Data.Common;
using EasyDB.Abstractions;

namespace EasyDB.Models;

public class UpgradeableDBContext : DBContext, IUpgradeableDBContext
{
    protected IDataBase _db;
    public UpgradeableDBContext(IDataBase db, DbConnection connection, Action? dispose) : base(connection, dispose)
    {
        _db = db;
    }

    public IDBContext Write() => _db.Write();

    public IDBContext Transaction() => _db.Transaction();
}