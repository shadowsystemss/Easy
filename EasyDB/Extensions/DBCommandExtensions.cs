using System.Data.Common;

namespace EasyDB.Extensions;

public static class DBCommandExtensions
{
    public static DbParameter AddParameter(this DbCommand command, string name, object? value)
    {
        DbParameter parameter = command.CreateParameter();
        if (name.Length > 0 && name[0] != '@')
            name = '@' + name;
        parameter.ParameterName = name;
        parameter.Value = value;
        command.Parameters.Add(parameter);
        return parameter;
    }

    public static DbCommand With(this DbCommand command, string name, object? value)
    {
        command.AddParameter(name, value);
        return command;
    }

    public static DbCommand With(this DbCommand command, params DbParameter[] parameters)
    {
        command.Parameters.AddRange(parameters);
        return command;
    }
}
