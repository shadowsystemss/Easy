namespace CommandSystem.Abstractions
{
    public interface ITextCommand : ICommand
    {
        public ICommand? Execute(string? text);
    }
}
