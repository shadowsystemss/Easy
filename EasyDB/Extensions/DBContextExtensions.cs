using EasyDB.Abstractions;
using System.Data.Common;

namespace EasyDB.Extensions;

public static class DBContextExtensions
{
    public static DbCommand CreateCommand(this IDBContext context, string sql)
    {
        DbCommand command = context.CreateCommand();
        command.CommandText = sql;
        return command;
    }

    public static int Execute(this IDBContext context, string sql)
    {
        using DbCommand command = context.CreateCommand(sql);
        return command.ExecuteNonQuery();
    }

    public static object? Scalar(this IDBContext context, string sql)
    {
        using DbCommand command = context.CreateCommand(sql);
        return command.ExecuteScalar();
    }

    public static T ReaderWrapper<T>(this IDBContext context, string sql, Func<DbDataReader, T> action)
    {
        using DbCommand command = context.CreateCommand(sql);
        using DbDataReader reader = command.ExecuteReader();
        return action(reader);
    }

    public static T ReaderWrapper<T>(this IDBContext context, string sql, Func<DbDataReader, IDBContext, T> action)
        => context.ReaderWrapper(sql, reader => action(reader, context));
}
