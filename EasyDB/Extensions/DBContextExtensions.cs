using EasyDb.Abstractions;
using System.Data.Common;

namespace EasyDb.Extensions;

public static class DbContextExtensions
{
    public static DbCommand CreateCommand(this IDbContext context, string sql)
    {
        DbCommand command = context.CreateCommand();
        command.CommandText = sql;
        return command;
    }

    public static int Execute(this IDbContext context, string sql, params ReadOnlySpan<(string name, object? value)> parameters)
    {
        using DbCommand command = context.CreateCommand(sql);
        foreach ((string name, object? value) in parameters)
            command.AddParameter(name, value);
        return command.ExecuteNonQuery();
    }

    public static object? Scalar(this IDbContext context, string sql, params ReadOnlySpan<(string name, object? value)> parameters)
    {
        using DbCommand command = context.CreateCommand(sql);
        foreach ((string name, object? value) in parameters)
            command.AddParameter(name, value);
        return command.ExecuteScalar();
    }

    public static T ReaderWrapper<T>(this IDbContext context, string sql, Func<DbDataReader, T> action, params ReadOnlySpan<(string name, object? value)> parameters)
    {
        using DbCommand command = context.CreateCommand(sql);
        foreach ((string name, object? value) in parameters)
            command.AddParameter(name, value);
        using DbDataReader reader = command.ExecuteReader();
        return action(reader);
    }
}