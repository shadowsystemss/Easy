using EasyDB.Abstractions;

namespace EasyDB.Advanced;

public class EnumDBTable<T> : SetDBTable where T : Enum
{
    public EnumDBTable(IDataBase db, string tableName = nameof(T)) : base(db, tableName) { }

    public override void Fill()
    {
        foreach (T value in Enum.GetValues(typeof(T)))
            Add(Decorator(value), value as int?);
    }

    public virtual string Decorator(T value) => value.ToString();
}