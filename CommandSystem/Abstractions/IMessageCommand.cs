namespace CommandSystem.Abstractions
{
    public interface IMessageCommand : ICommand
    {
        public string? Message { get; }
    }
}
