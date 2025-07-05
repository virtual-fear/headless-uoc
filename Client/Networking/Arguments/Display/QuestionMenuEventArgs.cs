using Client.Game;

namespace Client.Networking.Arguments;
public sealed class QuestionMenuEventArgs : EventArgs
{
    [PacketHandler(0x7C, length: -1, ingame: true)]
    private static event PacketEventHandler<QuestionMenuEventArgs>? Update;
    public NetState State { get; }
    public int MenuSerial { get; }
    public string Question { get; }
    public string[] Answers { get; }
    internal QuestionMenuEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        // EnsureCapacity( 256 )
        MenuSerial = ip.ReadInt32();
        ip.ReadInt16();  //  0
        byte length;
        string question;
        string[] answers;
        length = ip.ReadByte();
        if (length != 0)
            question = ip.ReadString(length);
        else
            question = string.Empty;

        length = ip.ReadByte();
        if (length > 0)
        {
            answers = new string[length];
            for (int i = 0; i < answers.Length; ++i)
            {
                ip.ReadInt32();
                length = ip.ReadByte();
                answers[i] = ip.ReadString(length);
            }
        }
        else
        {
            answers = new string[0];
        }
        Question = question;
        Answers = answers;
    }
    static QuestionMenuEventArgs() => Update += QuestionMenuEventArgs_Update;
    private static void QuestionMenuEventArgs_Update(QuestionMenuEventArgs e)
         => Display.ShowQuestionMenu(e.State, e.MenuSerial, e.Question, e.Answers);
}