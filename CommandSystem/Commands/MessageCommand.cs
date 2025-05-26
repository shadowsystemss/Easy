using CommandSystem.Abstractions;

namespace CommandSystem.Commands
{
    public class MessageCommand : IMessageCommand, ITextCommand
    {
        protected ICommand? _step;

        public MessageCommand(string message, ICommand? command = null)
        {
            Message = message;
            _step = command;
        }

        public string Message { get; init; }

        public ICommand? Execute(string? text)
        {
            if (!string.IsNullOrWhiteSpace(text)
                && _step is ITextCommand textCommand)
                return textCommand.Execute(text);

            return this;
        }

        public ICommand? Execute() => _step;
    }
}
