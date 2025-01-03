namespace Client
{
    using Client.Networking;
    public partial class Assistant : Network
    {
        [STAThread]
        public static void Main() => _Main().GetAwaiter().GetResult();
    }
}
