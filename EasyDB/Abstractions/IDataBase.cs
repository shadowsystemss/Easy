namespace EasyDb.Abstractions;

public interface IDataBase : IDisposable
{
    IDbContext Read();
    IDbContext Write();
    IUpgradeableDbContext UpgradeableRead();
    IDbContext Transaction();
}
