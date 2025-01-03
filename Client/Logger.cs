using System;
using Client.Networking;

namespace Client
{
    internal static class Logger
    {
        public static event EventHandler<string>? OnLog;
        public static event EventHandler<string>? OnLogError;
        public static event EventHandler<string>? OnPushWarning;
        private static string Name(object o) => o is string s ? s : o is Type t ? t.Name : o == null ? "o" : o.GetType().Name;
        static Logger()
        {
            Logger.OnLog += Logger_OnLog;
            Logger.OnLogError += Logger_OnLogError;
            Logger.OnPushWarning += Logger_OnPushWarning;
        }
        private static void Logger_OnPushWarning(object? sender, string e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[WARNING] ");

            if (sender == null)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            else if (sender is ConsoleColor c)
                Console.ForegroundColor = c;

            Console.WriteLine(e);
        }
        internal static void Logger_OnLogError(object? sender, string e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] ");

            if (sender == null)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            else if (sender is ConsoleColor c)
                Console.ForegroundColor = c;

            Console.WriteLine(e);
        }
        private static void Logger_OnLog(object? sender, string e)
        {
            if (sender is ConsoleColor c)
                Console.ForegroundColor = c;

            Console.WriteLine(e);
            if (sender != null) { Console.ResetColor(); }
        }

        public static void Log(bool indent, string what, ConsoleColor color) => Log($"{(indent ? new string(' ', 3) : string.Empty)}{what}", color);
        public static void Log(object o, string what, ConsoleColor color) => Log($"{Name(o)}: {what}", color);
        public static void Log(string what, ConsoleColor color) => OnLog?.Invoke(color, what);
        public static void Log(string what = "") => OnLog?.Invoke(null, what);
        public static void Log(object o, string what) => Log($"{Name(o)}: {what}");
        public static void LogError(string what) => OnLogError?.Invoke(null, what);
        public static void PushWarning(string what) => OnPushWarning?.Invoke(null, what);
    }
}
