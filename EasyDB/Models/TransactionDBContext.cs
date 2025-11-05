using System.Data.Common;

namespace EasyDb.Models;

public class TransactionDBContext(DbConnection connection, Action? dispose) : DBContext(connection, dispose)
{
    public DbTransaction Transaction { get; } = connection.BeginTransaction();

    public override void Dispose()
    {
        Transaction.Commit();
        Transaction.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}
