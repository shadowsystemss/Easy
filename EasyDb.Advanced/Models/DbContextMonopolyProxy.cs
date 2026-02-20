using EasyDb.Abstractions;
using System.Data.Common;

namespace EasyDb.Advanced.Models;

public class DbContextMonopolyProxy(IDbContext context, Action? dispose) : IDbContext
{
    private readonly IDbContext _context = context;
    private readonly Action? _dispose = dispose;

    public DbCommand CreateCommand()
        => _context.CreateCommand();

    public virtual void Dispose()
    {
        _context.Dispose();
        _dispose?.Invoke();
        GC.SuppressFinalize(this);
    }
}
