namespace CommandSystem.Abstractions;

public interface IMessageCommand : ICommand
{
    string? Message { get; }
}
