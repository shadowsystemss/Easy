using EasyDb.Abstractions;

namespace EasyDb.Advanced.Tables;

public class EnumDbTable<T>(IDataBase db, string tableName = nameof(T)) : SetDbTable(db, tableName) where T : Enum
{
    public override void Fill()
    {
        foreach (T value in Enum.GetValues(typeof(T)))
            Add(Decorator(value), value as int?);
    }

    public virtual string Decorator(T value) => value.ToString();
}