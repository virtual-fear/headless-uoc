using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Client.Networking.Diagnostics
{
    public abstract class PacketProfile : Profile
    {
        private long _totalLength;
        public long TotalLength
        {
            get { return _totalLength; }
        }
        public long AverageLength
        {
            get { return _totalLength / Math.Max(1, Count); }
        }

        public PacketProfile(string name) : base(name)
        {
        }

        public PacketProfile(Type type)
            : base(type)
        {
        }

        public void Finish(int length)
        {
            Stop();

            _totalLength += _totalLength;
        }

        public override void WriteTo(TextWriter op)
        {
            base.WriteTo(op);

            op.Write("\t{0,12:F2} {1,-12:N0}", AverageLength, TotalLength);
        }
    }

    public class PacketSendProfile : PacketProfile
    {
        public PacketSendProfile(Type type)
            : base(type.FullName)
        {
        }
    }

    public class PacketReceiveProfile : PacketProfile
    {
        public PacketReceiveProfile(int packetID)
            : base(string.Format("0x{0:X2}", packetID))
        {
        }
    }
}
