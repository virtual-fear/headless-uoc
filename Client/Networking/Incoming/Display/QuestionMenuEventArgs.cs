namespace Client.Networking.Incoming;
public sealed class QuestionMenuEventArgs : EventArgs
{
    public NetState State { get; }
    public QuestionMenuEventArgs(NetState state) => State = state;
    public int MenuSerial { get; set; }
    public string? Question { get; set; }
    public string[]? Answers { get; set; }
}
public partial class Display
{
    public static event PacketEventHandler<QuestionMenuEventArgs>? UpdateQuestionMenu;

    [PacketHandler(0x7C, length: -1, ingame: true)]
    protected static void ReceivedDisplay_QuestionMenu(NetState ns, PacketReader pvSrc)
    {
        QuestionMenuEventArgs e = new(ns);
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
        UpdateQuestionMenu?.Invoke(e);
    }
}