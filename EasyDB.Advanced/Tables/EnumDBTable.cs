using EasyDB.Abstractions;

namespace EasyDB.Advanced.Tables;

public class EnumDBTable<T>(IDataBase db, string tableName = nameof(T)) : SetDBTable(db, tableName) where T : Enum
{
    public override void Fill()
    {
        foreach (T value in Enum.GetValues(typeof(T)))
            Add(Decorator(value), value as int?);
    }

    public virtual string Decorator(T value) => value.ToString();
}