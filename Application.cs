using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Client
{
    public static class Application
    {
        public static Version Version => new Version(0, 0, 1, 0); // Initial alpha release
        public static Assembly Assembly { get; } = typeof(Application).Assembly;
        public static Process Process { get; } = Process.GetCurrentProcess();
        public static string ProcessName { get; private set; } = nameof(Client);
        public static Thread? Thread { get; private set; }
        public static SynchronizationContext? LoopContext { get; private set; }
        static Application() => Configure(Process);
        private static void Configure(Process process)
        {
            ProcessName = process.ProcessName;
            Thread = Thread.CurrentThread;
            Thread.Name = "Game Thread";
            LoopContext = new EventLoopContext();
            SynchronizationContext.SetSynchronizationContext(LoopContext);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            
            Console.OutputEncoding = Encoding.UTF8;

            Logger.Log($"[https://github.com/godot-this/uo-assistant]", ColorType.Success);
            string version = $"Version: {Version.Major}.{Version.Minor}.{Version.Build}.{Version.Revision}";
            if (Version.Major > 0)
                version += " (release)";
            else if (Version.Build == 1)
                version += " (alpha)";
            else if (Version.Build >= 2)
                version += " (alpha + new features)";
            Logger.Log(version, ColorType.Info);
            Logger.Log($"Running on {RuntimeInformation.FrameworkDescription}", ColorType.Info);

            // Setup the assistant
            Assistant.Configure();

            // Run the assistant
            Assistant.AsyncConnect();
        }

        private static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        [STAThread]
        public static void Main() => WaitIndefinitely().GetAwaiter().GetResult();

        /// <summary>
        ///     The main entry point for the client application. 
        ///     <para>This method initiates the asynchronous processing of the application and waits indefinitely for tasks to complete.</para>
        /// </summary>
        /// <returns>A <see cref="Task"</see>> representing the asynchronous operation.</returns>
        static async Task WaitIndefinitely() => await Task.Delay(-1);
    }
}
