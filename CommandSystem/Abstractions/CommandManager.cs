namespace CommandSystem.Abstractions;

public abstract class CommandManager : ITextCommand
{
    protected ICommand? _step;
    protected abstract void Message(string? text);
    protected abstract bool Intercept(string? text);

    public ICommand? Execute(string? text)
    {
        if (Intercept(text) && _step is ITextCommand textCommand)
            _step = textCommand.Execute(text);

        if (_step is IMessageCommand messageCommand)
            Message(messageCommand.Message);

        _step = _step?.Execute();
        return this;
    }

    public abstract ICommand? Execute();
}
