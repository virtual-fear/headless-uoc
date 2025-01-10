namespace Client.Game.Data.BulletinBoard;

using Client.Game.Context;
public sealed class BulletinBoardHeader
{
    public ItemContext Board { get; }
    public ItemContext Thread { get; }
    public string Poster { get; }
    public string Subject { get; }
    public string Time { get; }
    public BulletinBoardHeader(ItemContext board, ItemContext thread, string poster, string subject, string time)
    {
        Board = board;
        Thread = thread;
        Poster = poster;
        Subject = subject;
        Time = time;
    }
}
