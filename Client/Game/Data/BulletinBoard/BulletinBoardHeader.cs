namespace Client.Game.Data.BulletinBoard;

using Client.Game.Context;
public sealed class BulletinBoardHeader
{
    public Item Board { get; }
    public Item Thread { get; }
    public string Poster { get; }
    public string Subject { get; }
    public string Time { get; }
    public BulletinBoardHeader(Item board, Item thread, string poster, string subject, string time)
    {
        Board = board;
        Thread = thread;
        Poster = poster;
        Subject = subject;
        Time = time;
    }
}
