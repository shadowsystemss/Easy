using System.Data.Common;

namespace EasyDb.Models;

public class TransactionDbContext(DbConnection connection, Action? dispose) : DbContext(connection, dispose)
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
