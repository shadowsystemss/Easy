namespace EasyDb.Abstractions;

public interface IUpgradeableDBContext : IDBContext
{
    IDBContext Write();
    IDBContext Transaction();
}