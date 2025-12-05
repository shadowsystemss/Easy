using CommandSystem.Abstractions;

namespace CommandSystem.Commands;

public class MessageCommand(string message, ICommand? command = null) : IMessageCommand, ITextCommand
{
    protected ICommand? _step = command;

    public string Message { get; } = message;

    public ICommand? Execute(string? text)
    {
        if (!string.IsNullOrWhiteSpace(text)
            && _step is ITextCommand textCommand)
            return textCommand.Execute(text);

        return this;
    }

    public ICommand? Execute() => _step;
}
