namespace CommandSystem.Abstractions;

public interface ICommand
{
    ICommand? Execute();
}