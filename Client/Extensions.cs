using System.Net;

namespace Client
{
    public static class Extensions
    {
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