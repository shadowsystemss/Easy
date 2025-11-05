using System.Data.Common;

namespace EasyDb.Extensions;

public static class DBReaderExtensions
{
    public static IEnumerable<T> GetScalars<T>(this DbDataReader reader, Func<DbDataReader, T> generator)
    {
        while (reader.Read())
            yield return generator(reader);
    }

    public static IEnumerable<T> GetScalars<T>(this DbDataReader reader)
        => GetScalars(reader, x => x.GetFieldValue<T>(0));

    public static List<T> GetList<T>(this DbDataReader reader)
        => [.. GetScalars<T>(reader)];

    public static Dictionary<TKey, TValue>? GetDictionary<TKey, TValue>(this DbDataReader reader) where TKey : notnull
    {
        if (!reader.HasRows)
            return null;

        var result = new Dictionary<TKey, TValue>();
        while (reader.Read())
            result.Add(reader.GetFieldValue<TKey>(0), reader.GetFieldValue<TValue>(1));

        return result;
    }
}
