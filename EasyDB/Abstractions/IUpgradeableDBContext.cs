namespace EasyDB.Abstractions;

public interface IUpgradeableDBContext : IDBContext
{
    IDBContext Write();
    IDBContext Transaction();
}