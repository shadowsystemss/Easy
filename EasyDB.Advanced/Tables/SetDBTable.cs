using System.Data.Common;
using EasyDB.Abstractions;
using EasyDB.Extensions;

namespace EasyDB.Advanced.Tables;

public class SetDBTable(IDataBase db, string tableName) // Класс для упрощенья создания списков
{
    protected readonly IDataBase _db = db;
    protected readonly string _tableName = tableName;

    public void Create() => _db.Write($"""
        CREATE TABLE IF NOT EXISTS {_tableName} (
            id INTEGER PRIMARY KEY,
            text TEXT UNIQUE NOT NULL
        )
        """);

    public void Drop() => _db.Write($"DROP TABLE IF EXISTS {_tableName}");

    public virtual void Fill() { }

    public void Add(string text, int? id = null)
    {
        using IDBContext context = _db.Write();
        using DbCommand command = context
            .CreateCommand()
            .With("@text", text);
        if (id.HasValue)
        {
            command.CommandText = $"REPLACE INTO {_tableName}(id, text) VALUES(@id, @text)";
            command.AddParameter(text, id.Value);
        }
        else
            _db.Write($"INSERT OR IGNORE INTO {_tableName}(text) VALUES(@text)");
        command.ExecuteNonQuery();
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
        using DbCommand command = context
            .CreateCommand($"REPLACE INTO {_tableName}(id, text) VALUES(@id, @text)");

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
    {
        using IDBContext context = _db.Write();
        using DbCommand command = context
            .CreateCommand($"UPDATE {_tableName} SET text = @text WHERE id = @id")
            .With("@id", id)
            .With("@text", text);
        command.ExecuteNonQuery();
    }

    public void RemoveById(int id)
        => _db.Write($"DELETE FROM {_tableName} WHERE id = {id}");

    public void RemoveByText(string text)
    {
        _db.Write($"DELETE FROM {_tableName} WHERE text = @text");
    }

    public string? GetTextById(int id)
        => (string?)_db.Scalar($"SELECT text FROM {_tableName} WHERE id = {id}");

    public int? GetIdByText(string text)
        => (int?)_db.Scalar($"SELECT id FROM {_tableName} WHERE text = '{text.Replace("'", "''")}'");

    public List<int>? GetIds(int limit = 100, int offset = 0)
        => _db.ReaderWrapper($"SELECT id FROM {_tableName} LIMIT {limit} OFFSET {offset}", reader => reader.GetScalars<int>().ToList());

    public List<string>? GetList(int limit = 100, int offset = 0)
        => _db.ReaderWrapper($"SELECT text FROM {_tableName} LIMIT {limit} OFFSET {offset}", reader => reader.GetScalars<string>().ToList());

    public Dictionary<int, string>? GetDictionary(int limit = 100, int offset = 0)
        => _db.ReaderWrapper($"SELECT * FROM {_tableName} LIMIT {limit} OFFSET {offset}", DBReaderExtensions.GetDictionary<int, string>);

    public int Count()
        => Convert.ToInt32(_db.Scalar("SELECT COUNT(*) FROM " + _tableName) ?? 0);
}
