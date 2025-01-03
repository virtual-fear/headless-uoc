using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Client.Networking
{
    public interface IDiagnostic
    {
        void Close();
        void Open();

        void Received(PacketHandler handler, byte[] buffer, int offset, int length);
        void Sent(Packet packet, byte[] buffer, int offset, int length);
    }

    public static class Diagnostics
    {
        private static List<IDiagnostic> m_Queue = new List<IDiagnostic>();
        public static IEnumerable<IDiagnostic> Queue { get { return m_Queue; } }

        public static void Register(IDiagnostic[] range)
        {
            if ((range != null) && (range.Length > 0))
                for (int i = 0; i < range.Length; ++i)
                {
                    IDiagnostic d = range[i];
                    if (d == null || m_Queue.Contains(d))
                    {
                        continue;
                    }
                    m_Queue.Add(d);
                }
        }

        static Diagnostics()
        {
        }

        public static void Close()
        {
            foreach (IDiagnostic d in Diagnostics.Queue)
                d.Close();
        }

        public static void Open()
        {
            foreach (IDiagnostic d in Diagnostics.Queue)
                d.Open();
        }

        public static void Received(PacketHandler handler, byte[] buffer, int offset, int length)
        {
            foreach (IDiagnostic d in Diagnostics.Queue)
                d.Received(handler, buffer, offset, length);
        }

        public static void Sent(Packet packet, byte[] buffer, int offset, int length)
        {
            foreach (IDiagnostic d in Diagnostics.Queue)
                d.Sent(packet, buffer, offset, length);
        }
    }


    public sealed class Diagnostic : IDiagnostic
    {
        private TextWriter m_Output;

        public static IDiagnostic Instantiate<T>()
        {
            return new Diagnostic(new StreamWriter((typeof(T).Name.ToLower() + "-diagnostics.log"), true));
        }

        public Diagnostic(TextWriter output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            m_Output = output;
        }

        private static string Format(string title)
        {
            return Format("{0}...({1})", title, DateTime.Now);
        }

        private static string Format( string format, params object[] args )
        {
            string m = string.Format(format, args);
            return string.Format("###\t{0}\t###",m);
        }

        public void Close()
        {
            m_Output.WriteLine(Format("Closed"));
            m_Output.Flush();
        }

        public void Open()
        {
            m_Output.WriteLine(Format("Opened"));
            m_Output.Flush();
        }

        public void Received(PacketHandler handler, byte[] buffer, int offset, int length)
        {
            DateTime now = DateTime.Now;

            m_Output.WriteLine("(Received) '{0}' ( {1:X0} bytes )", handler.Name, length);
            m_Output.WriteLine("@ {0} {1}", now.ToShortDateString(), now.TimeOfDay);

            Trace(m_Output, buffer, offset, length);

            m_Output.WriteLine();
        }

        public void Sent(Packet packet, byte[] buffer, int offset, int length)
        {
            DateTime now = DateTime.Now;

            m_Output.WriteLine("(Sent) '{0}' ( {1:N0} bytes )", packet.GetType().Name, length);
            m_Output.WriteLine("@ {0} {1}", now.ToShortDateString(), now.TimeOfDay);

            Trace(m_Output, buffer, offset, length);

            m_Output.WriteLine();
        }

        private static void Trace(TextWriter output, byte[] buffer, int offset, int length)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if ((offset < 0) || (offset >= buffer.Length))
            {
                throw new ArgumentOutOfRangeException("offset", offset, "Offset must be greater than or equal to zero and less than the size of the buffer.");
            }
            if ((length < 0) || (length > buffer.Length))
            {
                throw new ArgumentOutOfRangeException("length", length, "Length cannot be less than zero or greater than the size of the buffer.");
            }
            if ((buffer.Length - offset) < length)
            {
                throw new ArgumentException("Offset and length do not point to a valid segment within the buffer.");
            }
            output.WriteLine("        0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F");
            output.WriteLine("       -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- --");
            int x = 0;
            while (x < length)
            {
                output.Write(x.ToString("X4"));
                output.Write("   ");
                for (int i = 0; i < 0x10; i++)
                {
                    if ((x + i) < length)
                    {
                        output.Write(buffer[(offset + x) + i].ToString("X2"));
                        if (i == 7)
                        {
                            output.Write("  ");
                        }
                        else
                        {
                            output.Write(' ');
                        }
                    }
                    else if (i == 7)
                    {
                        output.Write("    ");
                    }
                    else
                    {
                        output.Write("   ");
                    }
                }
                output.Write("  ");
                int y = 0;
                while ((y < 0x10) && (x < length))
                {
                    byte num4 = buffer[offset + x];
                    if ((num4 >= 0x20) && (num4 < 0x80))
                    {
                        output.Write((char)num4);
                    }
                    else
                    {
                        output.Write('.');
                    }
                    y++;
                    x++;
                }
                output.WriteLine();
            }
            output.Flush();
        }
    }
}
