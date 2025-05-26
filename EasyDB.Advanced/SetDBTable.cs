using System.Data.Common;
using EasyDB.Abstractions;
using EasyDB.Extensions;

namespace EasyDB.Advanced;

public class SetDBTable : IDBTable // Класс для упращения создания списков
{
    protected readonly IDataBase _db;
    protected readonly string _tableName;

    public SetDBTable(IDataBase db, string tableName)
    {
        _db = db;
        _tableName = tableName;
    }

    public void Create() => _db.Write($"""
        CREATE TABLE {_tableName} (
            id INTEGER PRIMARY KEY,
            text TEXT UNIQUE NOT NULL
        )
        """);

    public void Drop() => _db.Write(_tableName);

    public virtual void Fill() { }

    public void Add(string text, int? id = null)
    {
        if (id.HasValue)
            _db.Write($"REPLACE INTO {_tableName}(id, text) VALUES({id.Value}, '{text.Replace("'", "''")}')");
        else
            _db.Write($"INSERT OR IGNORE INTO {_tableName}(text) VALUES('{text.Replace("'", "''")}')");
    }

    public void Add(IList<string> data)
    {
        using IDBContext context = _db.Write();
        using DbCommand command = context.CreateCommand($"INSERT INTO {_tableName}(text) VALUES(@text)");

        DbParameter textParameter = command.CreateParameter();
        textParameter.ParameterName = "@text";
        command.Parameters.Add(textParameter);

        foreach (string item in data)
        {
            textParameter.Value = item;
            command.ExecuteNonQuery();
        }
    }

    public void Replace(IList<string> data)
    {
        using IDBContext context = _db.Write();
        using DbCommand command = context.CreateCommand($"REPLACE INTO {_tableName}(id, text) VALUES(@id, @text)");

        DbParameter idParameter = command.CreateParameter();
        idParameter.ParameterName = "@id";
        command.Parameters.Add(idParameter);

        DbParameter textParameter = command.CreateParameter();
        textParameter.ParameterName = "@text";
        command.Parameters.Add(textParameter);

        for (int i = 0; i < data.Count; i++)
        {
            idParameter.Value = i;
            textParameter.Value = data[i];
            command.ExecuteNonQuery();
        }
    }

    public void Update(int id, string text)
        => _db.Write($"UPDATE {_tableName} SET text = '{text.Replace("'", "''")}' WHERE id = {id}");

    public void RemoveById(int id)
        => _db.Write($"DELETE FROM {_tableName} WHERE id = {id}");

    public void RemoveByText(string text)
        => _db.Write($"DELETE FROM {_tableName} WHERE text = '{text.Replace("'", "''")}'");

    public string? GetTextById(int id)
        => (string?)_db.Scalar($"SELECT text FROM {_tableName} WHERE id = {id}");

    public int? GetIdByText(string text)
        => (int?)_db.Scalar($"SELECT id FROM {_tableName} WHERE text = '{text.Replace("'", "''")}'");

    public List<int>? GetIds(int limit = 100, int offset = 0)
        => _db.ReaderWrapper($"SELECT id FROM {_tableName} LIMIT {limit} OFFSET {offset}", reader => reader.GetScalars<int>().ToList());

    public List<string>? GetList(int limit = 100, int offset = 0)
        => _db.ReaderWrapper($"SELECT text FROM {_tableName} LIMIT {limit} OFFSET {offset}", reader=>reader.GetScalars<string>().ToList());

    public Dictionary<int, string>? GetDictionary(int limit = 100, int offset = 0)
        => _db.ReaderWrapper($"SELECT * FROM {_tableName} LIMIT {limit} OFFSET {offset}", DBReaderExtensions.GetDictionary<int,string>);

    public int Count()
        => Convert.ToInt32(_db.Scalar("SELECT COUNT(*) FROM " + _tableName) ?? 0);
}
