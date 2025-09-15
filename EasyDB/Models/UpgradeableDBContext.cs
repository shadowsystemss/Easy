using EasyDB.Abstractions;
using System.Data.Common;

namespace EasyDB.Models;

public class UpgradeableDBContext(IDataBase db, DbConnection connection, Action? dispose) : DBContext(connection, dispose), IUpgradeableDBContext
{
    protected IDataBase _db = db;

    public IDBContext Write() => _db.Write();

    public IDBContext Transaction() => _db.Transaction();
}