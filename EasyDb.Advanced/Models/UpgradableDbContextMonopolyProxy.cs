using EasyDb.Abstractions;
using System.Data.Common;

namespace EasyDb.Advanced.Models;

public class UpgradableDbContextMonopolyProxy(IUpgradeableDbContext context, Action? dispose, Action? write, Action? writeDispose) : IUpgradeableDbContext
{
    private readonly IUpgradeableDbContext _context = context;
    private readonly Action? _dispose = dispose;
    private readonly Action? _write = write;
    private readonly Action? _writeDispose = writeDispose;

    public DbCommand CreateCommand()
        => _context.CreateCommand();

    public IDbContext Transaction()
    {
        _write?.Invoke();
        return new DbContextMonopolyProxy(_context.Transaction(), _writeDispose);
    }

    public IDbContext Write()
    {
        _write?.Invoke();
        return new DbContextMonopolyProxy(_context.Write(), _writeDispose);
    }

    public virtual void Dispose()
    {
        _context.Dispose();
        _dispose?.Invoke();
        GC.SuppressFinalize(this);
    }
}
