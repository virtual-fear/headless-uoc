namespace Client.Networking.Incoming.Display;
public partial class PacketHandlers
{
    public static event PacketEventHandler<DisplayQuestionMenuEventArgs>? DisplayQuestionMenu;
    public sealed class DisplayQuestionMenuEventArgs : EventArgs
    {
        public NetState State { get; }
        public DisplayQuestionMenuEventArgs(NetState state) => State = state;
        public int MenuSerial { get; set; }
        public string Question { get; set; }
        public string[] Answers { get; set; }
    }

    protected static class QuestionMenu
    {
        [PacketHandler(0x7C, length: -1, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            DisplayQuestionMenuEventArgs e = new(ns);
            // EnsureCapacity( 256 )
            e.MenuSerial = pvSrc.ReadInt32();
            pvSrc.ReadInt16();  //  0
            byte length;
            string question;
            string[] answers;
            length = pvSrc.ReadByte();
            if (length != 0)
                question = pvSrc.ReadString(length);
            else
                question = string.Empty;

            length = pvSrc.ReadByte();
            if (length > 0)
            {
                answers = new string[length];
                for (int i = 0; i < answers.Length; ++i)
                {
                    pvSrc.ReadInt32();
                    length = pvSrc.ReadByte();
                    answers[i] = pvSrc.ReadString(length);
                }
            }
            else
            {
                answers = new string[0];
            }
            e.Question = question;
            e.Answers = answers;
            DisplayQuestionMenu?.Invoke(e);
        }
    }
}