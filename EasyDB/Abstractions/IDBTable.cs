namespace EasyDB.Abstractions;

public interface IDBTable
{
    void Create();
    void Fill();
    void Drop();
}