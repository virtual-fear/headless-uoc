using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Client
{
    public static class Application
    {
        public static readonly bool Is64Bit = Environment.Is64BitProcess;
        public static readonly Version ClientVersion = new Version(7, 0, 106, 21);
        public static Version Version => new Version(0, 0, 1, 0); // Initial alpha release
        public static Assembly Assembly { get; } = typeof(Application).Assembly;
        public static Process Process { get; } = Process.GetCurrentProcess();
        public static string Name { get; private set; } = nameof(Client);
        public static Thread? Thread { get; private set; }
        public static Object? Instance { get; set; }

        // Intended for debugging purposes
        static Application() => Configure();
        private static void Configure()
        {
            Name = Application.Process.ProcessName.Contains("Godot") ? "Godot" : nameof(Client);
            Thread = Thread.CurrentThread;
            Thread.Name = "Game Thread";

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            Console.OutputEncoding = Encoding.UTF8;
            Logger.Log($"[https://github.com/godot-this/uo-assistant]", LogColor.Success);

            string majorType = Version.Major switch
            {
                > 0 => "release",
                0 when Version.Build == 1 => "alpha",
                0 when Version.Build >= 2 => "alpha + new features",
                _ => "dev build"
            };

            string version = $"Version: {Version.Major}.{Version.Minor}.{Version.Build}.{Version.Revision} ({majorType})";
            Logger.Log(version, LogColor.Info);
            Logger.Log($"Running on {RuntimeInformation.FrameworkDescription}", LogColor.Info);

            // Check to see if we are running in Godot
            bool runningInGodot = Application.Name.Contains("Godot");
            if (!runningInGodot)
            {
                Assistant.Configure(runningInGodot: false);

                // Immediately connect to the server
                Task.Run(Assistant.AsyncConnect);
            } else
            {
                Assistant.Configure(runningInGodot: true);
            }
        }
        private static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        }

        [STAThread]
        public static void Main() => WaitIndefinitely().GetAwaiter().GetResult();

        /// <summary>
        ///     The main entry point for the client application. 
        ///     <para>This method initiates the asynchronous processing of the application and waits indefinitely for tasks to complete.</para>
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        static async Task WaitIndefinitely() => await Task.Delay(-1);
    }
}
