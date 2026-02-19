using EasyDb.Abstractions;
using System.Data.Common;

namespace EasyDb.Extensions;

public static class DbExtensions
{
    public static int Write(this IDataBase db, string sql, params ReadOnlySpan<(string name, object? value)> parameters)
    {
        using IDbContext ctx = db.Write();
        return ctx.Execute(sql, parameters);
    }

    public static object? Scalar(this IDataBase db, string sql, params ReadOnlySpan<(string name, object? value)> parameters)
    {
        using IDbContext ctx = db.Read();
        return ctx.Scalar(sql, parameters);
    }

    public static T Read<T>(this IDataBase db, string sql, Func<DbDataReader, T> action, params ReadOnlySpan<(string name, object? value)> parameters)
    {
        using IDbContext ctx = db.Read();
        return ctx.ReaderWrapper(sql, action, parameters);
    }
}
