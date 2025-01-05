namespace Client
{
    public enum ColorType
    {
        None = ConsoleColor.Gray,
        Info = ConsoleColor.DarkYellow,
        Invalid = ConsoleColor.Red,
        Error = ConsoleColor.DarkRed,
        Warning = ConsoleColor.Yellow,
        Success = ConsoleColor.Green,
        Special = ConsoleColor.Magenta
    }
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

        private static void InternalLog(ColorType type, object? sender, string e)
        {
            if (type != ColorType.None)
            {
                Console.ForegroundColor = (ConsoleColor)type;
                string typeText = string.Empty;
                if (type == ColorType.Info)
                    typeText = "[INFO]";
                else if (type == ColorType.Error)
                    typeText = "[ERROR]";
                else if (type == ColorType.Warning)
                    typeText = "[WARNING]";
                Console.Write(typeText + ' ');
            }
            if (sender is ColorType c)
            {
                var color = (ConsoleColor)c;
                if (color != Console.ForegroundColor)
                    Console.ForegroundColor = color;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            Console.WriteLine(e);
            if (sender != null)
                Console.ResetColor();
        }
        private static void Logger_OnPushWarning(object? sender, string e) => InternalLog(ColorType.Warning, sender, e);
        private static void Logger_OnLogError(object? sender, string e) => InternalLog(ColorType.Error, sender, e);
        private static void Logger_OnLog(object? sender, string e) => InternalLog(ColorType.None, sender, e);
        public static void Log(bool indent, string what, ColorType color) => Log($"{(indent ? new string(' ', 3) : string.Empty)}{what}", color);
        public static void Log(object o, string what, ColorType color) => Log($"{Name(o)}: {what}", color);
        public static void Log(string what, ColorType color) => OnLog?.Invoke(color, what);
        public static void Log(string what = "") => OnLog?.Invoke(null, what);
        public static void Log(object o, string what) => Log($"{Name(o)}: {what}");
        public static void LogError(string what) => OnLogError?.Invoke(null, what);
        public static void PushWarning(string what) => OnPushWarning?.Invoke(null, what);
    }
}
