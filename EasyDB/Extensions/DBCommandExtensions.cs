using System.Data;
using System.Data.Common;

namespace EasyDb.Extensions;

public static class DBCommandExtensions
{
    public static DbParameter CreateParameter(this DbCommand command, string name)
    {
        DbParameter parameter = command.CreateParameter();
        if (name.Length > 0 && name[0] != '@')
            name = '@' + name;
        parameter.ParameterName = name;
        command.Parameters.Add(parameter);
        return parameter;
    }
    public static DbParameter AddParameter(this DbCommand command, string name, object? value)
    {
        DbParameter parameter = command.CreateParameter(name);
        parameter.Value = value ?? DBNull.Value;
        return parameter;
    }

    public static DbParameter AddParameter(this DbCommand command, string name, DbType type)
    {
        DbParameter parameter = command.CreateParameter(name);
        parameter.DbType = type;
        return parameter;
    }

    public static DbCommand With(this DbCommand command, string name, object? value)
    {
        command.AddParameter(name, value);
        return command;
    }

    public static DbCommand With(this DbCommand command, string name, DbType type)
    {
        command.AddParameter(name, type);
        return command;
    }

    public static DbCommand With(this DbCommand command, params DbParameter[] parameters)
    {
        command.Parameters.AddRange(parameters);
        return command;
    }
}
