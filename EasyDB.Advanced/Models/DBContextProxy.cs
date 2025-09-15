using EasyDB.Abstractions;
using System.Data.Common;

namespace EasyDB.Advanced.Models;

public class DBContextProxy(IDBContext context, Action? dispose) : IDBContext
{
    private readonly IDBContext _context = context;
    protected Action? _dispose = dispose;

    public DbCommand CreateCommand()
        => _context.CreateCommand();
    public virtual void Dispose()
    {
        _context.Dispose();
        _dispose?.Invoke();
        GC.SuppressFinalize(this);
    }
}
