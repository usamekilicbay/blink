namespace blink
{
    public static class CommandManager
    {
        // If there is no Command.txt file in the directory of exe file, this commands will be returned
        private static readonly List<string> _defaultCommands = new()
        { "10 push-ups", "Close your eyes", "Take a look outside", "Breath deeply" };

        private static List<string> _commands;

        /// <summary>
        /// For using Async, comment the _commands = ReadCommandFile(); line
        /// then uncomment SetCommandsAsync(); line
        /// </summary>
        static CommandManager()
        {
            _defaultCommands = _defaultCommands.ConvertAll(x => x.ToUpper());
            _commands = ReadCommandFile();
            //SetCommandsAsync();
        }

        #region Sync
        public static List<string> ReadCommandFile()
        {
            //string path = "C://Users//usame//Desktop";
            string path = AppDomain.CurrentDomain.BaseDirectory;
            const string fileName = "Commands.txt";
            string file = Path.Combine(path, fileName);

            if (File.Exists(file))
            {
                string command = File.ReadAllText(file);
                List<string> commandList = command.
                    Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).
                    ToList();
                commandList.ForEach(x => x.Trim());

                return commandList;
            }

            return _defaultCommands;
        }

        public static string GetRandomCommand()
        {
            Random random = new();
            int randIndex = random.Next(0, _commands.Count);
            return _commands[randIndex];
        }
        #endregion

        #region Async Reading

        private static async void SetCommandsAsync()
        {
            _commands = await ReadCommandFileAsync();
        }

        public static async Task<List<string>> ReadCommandFileAsync()
        {
            string path = "C://Users//usame//Desktop";
            const string fileName = "Commands.txt";
            string file = Path.Combine(path, fileName);

            if (File.Exists(file))
            {
                string command = await File.ReadAllTextAsync(file);
                List<string> commandList = command.Split("|").ToList();
                commandList.ForEach(x => x.Trim());
                return commandList;
            }

            return _defaultCommands.ToList();
        }

        public static async Task<string> GetRandomCommandAsync()
        {
            Random random = new();
            int randIndex = random.Next(0, _commands.Count);
            return _commands[randIndex];
        }
        #endregion
    }
}
