namespace EasyDb.Abstractions;

public interface IUpgradeableDbContext : IDbContext
{
    IDbContext Write();
    IDbContext Transaction();
}