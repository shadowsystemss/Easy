namespace CommandSystem.Abstractions;

public interface ITextCommand : ICommand
{
    ICommand? Execute(string? text);
}
