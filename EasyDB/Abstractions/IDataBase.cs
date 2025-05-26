namespace EasyDB.Abstractions;

public interface IDataBase : IDisposable
{
    IDBContext Read();
    IDBContext Write();
    IUpgradeableDBContext UpgradeableRead();
    IDBContext Transaction();
}
