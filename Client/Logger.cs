using System.Net;

namespace Client
{
    public enum LogColor
    {
        None = ConsoleColor.Gray,
        Info = ConsoleColor.DarkYellow,
        Invalid = ConsoleColor.Red,
        Error = ConsoleColor.DarkRed,
        Warning = ConsoleColor.Yellow,
        Success = ConsoleColor.Green,
        Magenta = ConsoleColor.Magenta,
        DarkMagenta = ConsoleColor.DarkMagenta,
    }

    public static class Logger
    {
        public static event EventHandler<string>? OnLog;
        public static event EventHandler<string>? OnLogError;
        public static event EventHandler<string>? OnPushWarning;
        private static string Name(object o) // => o is string s ? s : o is Type t ? t.Name : o == null ? "o" : o.GetType().Name;
            => o switch
            {
                string s => s,
                IPAddress addr => addr.ToString(),
                Type t => t.Name,
                _ => o.GetType().Name
            };

            static Logger()
        {
            // When using the Godot instance we don't want to write to the console.
            if (Application.Instance == null)
            {
                Logger.OnLog += Logger_OnLog;
                Logger.OnLogError += Logger_OnLogError;
                Logger.OnPushWarning += Logger_OnPushWarning;
            }
        }
        private static void InternalLog(LogColor type, object? sender, string text)
        {
            // Just to be sure for when using Godot we don't write to the console.
            if (Application.Instance != null)
                return;

            var typeColor = (ConsoleColor)type;
            switch(type)
            {
                case LogColor.Info: typeColor.Write("[INFO] "); break;
                case LogColor.Error: typeColor.Write("[ERROR] "); break;
                case LogColor.Warning: typeColor.Write("[WARNING] "); break;
                case LogColor.None:
                default: break;
            }

            sender ??= ConsoleColor.Gray;
            if (sender is ConsoleColor || sender is LogColor)
                ((ConsoleColor)sender).WriteLine(text);
        }
        private static void Logger_OnPushWarning(object? sender, string e) => InternalLog(LogColor.Warning, sender, e);
        private static void Logger_OnLogError(object? sender, string e) => InternalLog(LogColor.Error, sender, e);
        private static void Logger_OnLog(object? sender, string e) => InternalLog(LogColor.None, sender, e);
        public static void Log(bool indent, string what, LogColor color) => Log($"{(indent ? new string(' ', 3) : string.Empty)}{what}", color);
        public static void Log(object o, string what, LogColor color) => Log($"{Name(o)}: {what}", color);
        public static void Log(string what, LogColor color) => OnLog?.Invoke(color, what);
        public static void Log(string what = "") => OnLog?.Invoke(null, what);
        public static void Log(object o, string what) => Log($"[{Name(o)}] {what}");
        public static void LogError(object o, string what) => LogError($"{Name(o)}: {what}");
        public static void LogError(string what) => OnLogError?.Invoke(null, what);
        public static void PushWarning(string what) => OnPushWarning?.Invoke(null, what);
    }
}
