using EasyDb.Abstractions;
using System.Data.Common;

namespace EasyDb.Models;

public class UpgradeableDbContext(IDataBase db, DbConnection connection, Action? dispose) : DbContext(connection, dispose), IUpgradeableDbContext
{
    protected IDataBase _db = db;

    public IDbContext Write() => _db.Write();

    public IDbContext Transaction() => _db.Transaction();
}