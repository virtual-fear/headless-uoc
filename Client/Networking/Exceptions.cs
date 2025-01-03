using System;

namespace Client.Networking
{
    public class NetworkException : ArgumentException
    {
        public NetworkException()
            : base("A problem with the network has occured.")
        {
        }

        public NetworkException(string message)
            : base(message)
        {
        }

        public NetworkException(string message, string paramName)
            : base(message, paramName)
        {
        }

        public NetworkException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
