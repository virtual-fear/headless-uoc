using System;
using System.Linq;

namespace Client
{
    public sealed class CommandArgs
    {
        public static readonly string[] F = new string[] { "0", "off", "no", "false", "disabled" };
        public static readonly string[] T = new string[] { "1", "on", "yes", "true", "enabled" };
        public string[] Arguments { get; }
        public CommandArgs(string[] args) => Arguments = args;
        private string GetArgument(int i) => i >= 0 && i < Arguments.Length ? Arguments[i] : string.Empty;
        public bool GetBoolean(int i)
        {
            string arg = GetArgument(i).ToLower();
            if (string.IsNullOrEmpty(arg))
                return false;

            return T.Contains(arg) ? true : F.Contains(arg) ? false : false;
        }
        public int GetInt32(int i) => int.TryParse(GetArgument(i), out int v) ? v : 0;
        public string GetString(int i) => GetArgument(i);
        public TimeSpan GetTimeSpan(int i) => TimeSpan.TryParse(GetArgument(i), out TimeSpan ts) ? ts : TimeSpan.Zero;
    }

    public delegate void CommandCallback(CommandArgs args);
    public sealed class CommandHandler
    {
        public string Name { get; }
        public CommandCallback Callback { get; }
        public CommandHandler(string name, CommandCallback callback)
        {
            Name = name;
            Callback = callback;
        }
    }

    

}
