namespace Client.Networking.Incoming;
public sealed class QuestionMenuEventArgs : EventArgs
{
    public NetState State { get; }
    public int MenuSerial { get; }
    public string? Question { get; }
    public string[]? Answers { get; }
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
}