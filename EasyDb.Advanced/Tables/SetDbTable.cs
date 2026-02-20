using EasyDb.Abstractions;
using EasyDb.Extensions;
using System.Data;
using System.Data.Common;

namespace EasyDb.Advanced.Tables;

public class SetDbTable(IDataBase db, string tableName)
{
    protected readonly IDataBase _db = db;
    protected readonly string _tableName = tableName;

    public int Create() => _db.Write($"""
        CREATE TABLE IF NOT EXISTS {_tableName} (
            id INTEGER PRIMARY KEY,
            text TEXT UNIQUE NOT NULL
        )
        """);

    public int Drop() => _db.Write($"DROP TABLE IF EXISTS {_tableName}");

    public virtual void Fill() { }

    public int Add(string text, int? id = null)
    {
        using IDbContext context = _db.Write();
        using DbCommand command = context
            .CreateCommand()
            .With("@text", text);

        if (id.HasValue)
        {
            command.CommandText = $"""
                REPLACE INTO
                {_tableName} (id, text)
                VALUES (@id, @text)
                """;
            command.AddParameter("@id", id.Value);
        }
        else
            command.CommandText = $"""
                INSERT OR IGNORE INTO
                {_tableName} (text)
                VALUES (@text)
                """;

        return command.ExecuteNonQuery();
    }

    public void Add(IEnumerable<string> items)
    {
        using IDbContext context = _db.Write();
        using DbCommand command = context.CreateCommand($"""
            INSERT INTO
            {_tableName} (text)
            VALUES (@text)
            """);

        DbParameter textParameter = command.AddParameter("@text", DbType.String);
        foreach (string item in items)
        {
            textParameter.Value = item;
            command.ExecuteNonQuery();
        }
    }

    public void Replace(IEnumerable<string> items)
    {
        using IDbContext context = _db.Write();
        using DbCommand command = context.CreateCommand($"""
            REPLACE INTO
            {_tableName} (id, text)
            VALUES (@id, @text)
            """);

        int i = 0;
        DbParameter idParameter = command.AddParameter("@id", DbType.Int32);
        DbParameter textParameter = command.AddParameter("@text", DbType.String);

        foreach (string item in items)
        {
            idParameter.Value = i++;
            textParameter.Value = item;
            command.ExecuteNonQuery();
        }
    }

    public int Update(int id, string text) => _db.Write($"""
            UPDATE {_tableName}
            SET text = @text
            WHERE id = @id
            """, ("@id", id), ("@text", text));

    public int RemoveById(int id) => _db.Write($"""
        DELETE FROM {_tableName}
        WHERE id = @id
        """, ("@id", id));

    public int RemoveByText(string text) => _db.Write($"""
        DELETE FROM {_tableName} WHERE text = @text
        """, ("@text", text));

    public string? GetTextById(int id) => (string?)_db.Scalar($"""
        SELECT text
        FROM {_tableName}
        WHERE id = @id
        """, ("@id", id));

    public int? GetIdByText(string text) => (int?)_db.Scalar($"""
        SELECT id
        FROM {_tableName}
        WHERE text = @text
        """, ("@text", text));

    public List<int>? GetIds(int limit = 100, int offset = 0) => _db.Read($"""
            SELECT id
            FROM {_tableName}
            LIMIT @limit
            OFFSET @offset
            """, reader => reader.GetScalars<int>().ToList(),
            ("@limit", limit),
            ("@offset", offset));

    public List<string>? GetList(int limit = 100, int offset = 0) => _db.Read($"""
            SELECT text
            FROM {_tableName}
            LIMIT @limit
            OFFSET @offset
            """, reader => reader.GetScalars<string>().ToList(),
            ("@limit", limit),
            ("@offset", offset));

    public Dictionary<int, string>? GetDictionary(int limit = 100, int offset = 0) => _db.Read($"""
        SELECT *
        FROM {_tableName}
        LIMIT @limit
        OFFSET @offset
        """, DbReaderExtensions.GetDictionary<int, string>,
        ("@limit", limit),
        ("@offset", offset));

    public int Count() => (int?)_db.Scalar($"""
        SELECT COUNT(*)
        FROM {_tableName}
        """) ?? 0;
}
