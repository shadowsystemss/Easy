namespace CommandSystem.Abstractions
{
    public interface ICommand
    {
        public ICommand? Execute();
    }
}