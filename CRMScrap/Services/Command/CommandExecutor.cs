namespace CRMScrap.Services.Command
{
    public class CommandExecutor
    {
        private readonly List<ICommand> _commands;

        public CommandExecutor()
        {
            _commands = new List<ICommand>();
        }

        public void AddCommand(ICommand command)
        {
            _commands.Add(command);
        }

        public void ExecuteCommands()
        {
            foreach (var command in _commands)
            {
                command.Execute();
            }
        }
    }
}
