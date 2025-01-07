namespace Client;
public class GameException : ArgumentException
{
    public GameException() : base("A problem with the game has occured.") { }
    public GameException(string message) : base(message) { }
    public GameException(string message, string paramName) : base(message, paramName) { }
    public GameException(string message, Exception innerException) : base(message, innerException) { }
}
