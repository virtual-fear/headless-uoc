using System.Net;

namespace Client
{
    public static class Extensions
    {
        #region Extending Colors to the Console
        public static void Write(this ConsoleColor foregroundColor, string value)
        {

            var lastForeground = Console.ForegroundColor;
            if (lastForeground == foregroundColor)
            {
                Console.Write(value);
            } else { 
                Console.ForegroundColor = foregroundColor;
                Console.Write(value);
                Console.ForegroundColor = lastForeground;
            }
        }

        public static void Write(this ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
        {
            var lastBackground = Console.BackgroundColor;
            if (lastBackground == backgroundColor)
            {
                foregroundColor.Write(value);
            } else
            {
                Console.BackgroundColor = backgroundColor;
                foregroundColor.Write(value);
                Console.BackgroundColor = lastBackground;
            }
        }
        public static void WriteLine(this ConsoleColor foregroundColor, string value)
        {
            var lastForeground = Console.ForegroundColor;
            if (lastForeground == foregroundColor)
            {
                Console.WriteLine(value);
            }
            else
            {
                Console.ForegroundColor = foregroundColor;
                Console.WriteLine(value);
                Console.ForegroundColor = lastForeground;
            }
        }

        public static void WriteLine(this ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
        {
            var lastBackground = Console.BackgroundColor;
            if (lastBackground == backgroundColor)
            {
                foregroundColor.WriteLine(value);
            }
            else
            {
                Console.BackgroundColor = backgroundColor;
                foregroundColor.WriteLine(value);
                Console.BackgroundColor = lastBackground;
            }
        }
        #endregion

        public static uint ToUInt32(this IPAddress ipAddress)
        {
            byte[] addressBytes = ipAddress.MapToIPv4().GetAddressBytes();

            if (addressBytes != null && addressBytes.Length != 0)
            {
                return (uint)(addressBytes[0] | addressBytes[1] << 8 | addressBytes[2] << 16 | addressBytes[3] << 24);
            }

            return 0x100007f;
        }
    }
}