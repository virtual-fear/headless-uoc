using System;
using System.Text;
using System.Xml.Linq;

namespace Client.Networking;
public class PacketReader
{
    public static PacketReader Instance { get; private set; }

    private string m_Name;
    private byte m_Command;
    public byte[] Buffer { get; }
    private int m_Length; 
    private int m_Index;

    internal static PacketReader Initialize(ArraySegment<byte> segment, PacketHandler handler) => Initialize(segment, (handler.Length != -1), (byte)handler.PacketID, handler.Name);
    internal static PacketReader Initialize(ArraySegment<byte> segment, bool fixedSize, byte command, string name) => Initialize(segment.Array, segment.Offset, segment.Count, fixedSize, command, name);
    internal static PacketReader Initialize(byte[]? buffer, int index, int size, bool fixedSize, byte command, string name) => new PacketReader(buffer, index, size, fixedSize, command, name);
    public PacketReader(byte[]? buffer, int index, int size, bool fixedSize, byte cmd, string name)
    {
        if (buffer == null)
            throw new ArgumentNullException(nameof(buffer));

        Buffer = buffer;
        m_Length = buffer.Length;
        m_Index = index + (fixedSize ? 1 : 3); // MAYBE 0 : 2?
        m_Command = cmd;
        m_Name = name;
    }

    public int Index => m_Index;
    public int Length => m_Length;

    private bool IsSafeChar(int c)
    {
        return (c >= 0x20 && c < 0xFFFE);
    }

    public int Seek(int offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                m_Index = offset;
                break;

            case SeekOrigin.Current:
                m_Index += offset;
                break;

            case SeekOrigin.End:
                m_Index = m_Length - offset;
                break;
        }

        return m_Index;
    }

    #region Read Methods

    public int ReadInt32()
    {
        return (Buffer[m_Index++] << 24) | (Buffer[m_Index++] << 16) | (Buffer[m_Index++] << 8) | Buffer[m_Index++];
    }

    public short ReadInt16()
    {
        if ((m_Index + 2) > m_Length) return 0;
        return (short)((Buffer[m_Index++] << 8) | Buffer[m_Index++]);
    }

    public byte ReadByte()
    {
        return Buffer[m_Index++];
    }

    public byte[] ReadBytes(int length)
    {
        byte[] data = new byte[length];
        for (int i = 0; i < length; i++)
            data[i] = ReadByte();
        return data;
    }

    public uint ReadUInt32()
    {
        if ((m_Index + 4) > m_Length) return 0;
        return (uint)((Buffer[m_Index++] << 24) | (Buffer[m_Index++] << 16) | (Buffer[m_Index++] << 8) | Buffer[m_Index++]);
    }

    public uint ReadUInt32LE()
    {
        if ((m_Index + 4) > m_Length) return 0;
        return (uint)(Buffer[m_Index++] | (Buffer[m_Index++] << 8) | (Buffer[m_Index++] << 16) | (Buffer[m_Index++] << 24));
    }

    public ushort ReadUInt16()
    {
        if ((m_Index + 2) > m_Length) return 0;
        return (ushort)((Buffer[m_Index++] << 8) | Buffer[m_Index++]);
    }

    public sbyte ReadSByte()
    {
        return (sbyte)Buffer[m_Index++];
    }

    public bool ReadBoolean()
    {
        return (Buffer[m_Index++] != 0);
    }

    public string ReadUnicodeStringLE()
    {
        StringBuilder sb = new StringBuilder();
        int c;
        while ((m_Index + 1) < m_Length && (c = (Buffer[m_Index++] | (Buffer[m_Index++] << 8))) != 0)
            sb.Append((char)c);
        return sb.ToString();
    }

    public string ReadUnicodeStringLESafe(int fixedLength)
    {
        int bound = m_Index + (fixedLength << 1);
        int end = bound;
        if (bound > m_Length) bound = m_Length;
        StringBuilder sb = new StringBuilder();
        int c;
        while ((m_Index + 1) < bound && (c = (Buffer[m_Index++] | (Buffer[m_Index++] << 8))) != 0)
        {
            if (IsSafeChar(c)) sb.Append((char)c);
        }
        m_Index = end;
        return sb.ToString();
    }

    public string ReadUnicodeStringLESafe()
    {
        StringBuilder sb = new StringBuilder();
        int c;
        while ((m_Index + 1) < m_Length && (c = (Buffer[m_Index++] | (Buffer[m_Index++] << 8))) != 0)
        {
            if (IsSafeChar(c)) sb.Append((char)c);
        }
        return sb.ToString();
    }

    public string ReadUnicodeStringSafe()
    {
        StringBuilder sb = new StringBuilder();
        int c;
        while ((m_Index + 1) < m_Length && (c = ((Buffer[m_Index++] << 8) | Buffer[m_Index++])) != 0)
        {
            if (IsSafeChar(c)) sb.Append((char)c);
        }
        return sb.ToString();
    }

    public string ReadUnicodeString()
    {
        StringBuilder sb = new StringBuilder();
        int c;
        while (m_Index < m_Length)
        {
            c = ReadByte();
            if (c == 0x00) continue;
            sb.Append((char)c);
        }
        return sb.ToString();
    }

    public string ReadUTF8StringSafe(int fixedLength)
    {
        if (m_Index >= m_Length)
        {
            m_Index += fixedLength;
            return String.Empty;
        }
        int bound = m_Index + fixedLength;
        if (bound > m_Length) bound = m_Length;
        int count = 0;
        int index = m_Index;
        int start = m_Index;
        while (index < bound && Buffer[index++] != 0) ++count;
        index = 0;
        byte[] buffer = new byte[count];
        int value = 0;
        while (m_Index < bound && (value = Buffer[m_Index++]) != 0)
            buffer[index++] = (byte)value;
        string s = Encoding.UTF8.GetString(buffer);
        bool isSafe = true;
        for (int i = 0; isSafe && i < s.Length; ++i)
            isSafe = IsSafeChar((int)s[i]);
        m_Index = start + fixedLength;
        if (isSafe) return s;
        StringBuilder sb = new StringBuilder(s.Length);
        for (int i = 0; i < s.Length; ++i)
            if (IsSafeChar((int)s[i])) sb.Append(s[i]);
        return sb.ToString();
    }

    public string ReadUTF8StringSafe()
    {
        if (m_Index >= m_Length) return String.Empty;
        int count = 0;
        int index = m_Index;
        while (index < m_Length && Buffer[index++] != 0) ++count;
        index = 0;
        byte[] buffer = new byte[count];
        int value = 0;
        while (m_Index < m_Length && (value = Buffer[m_Index++]) != 0)
            buffer[index++] = (byte)value;
        string s = Encoding.UTF8.GetString(buffer);
        bool isSafe = true;
        for (int i = 0; isSafe && i < s.Length; ++i)
            isSafe = IsSafeChar((int)s[i]);
        if (isSafe) return s;
        StringBuilder sb = new StringBuilder(s.Length);
        for (int i = 0; i < s.Length; ++i)
        {
            if (IsSafeChar((int)s[i])) sb.Append(s[i]);
        }
        return sb.ToString();
    }

    public string ReadUTF8String()
    {
        if (m_Index >= m_Length) return String.Empty;
        int count = 0;
        int index = m_Index;
        while (index < m_Length && Buffer[index++] != 0) ++count;
        index = 0;
        byte[] buffer = new byte[count];
        int value = 0;
        while (m_Index < m_Length && (value = Buffer[m_Index++]) != 0)
            buffer[index++] = (byte)value;
        return Encoding.UTF8.GetString(buffer);
    }

    public string ReadString()
    {
        StringBuilder sb = new StringBuilder();
        int c;
        while (m_Index < m_Length && (c = Buffer[m_Index++]) != 0)
            sb.Append((char)c);
        return sb.ToString();
    }

    public string ReadStringSafe()
    {
        StringBuilder sb = new StringBuilder();
        int c;
        while (m_Index < m_Length && (c = Buffer[m_Index++]) != 0)
        {
            if (IsSafeChar(c)) sb.Append((char)c);
        }
        return sb.ToString();
    }

    public string ReadUnicodeStringSafe(int fixedLength)
    {
        int bound = m_Index + (fixedLength << 1);
        int end = bound;
        if (bound > m_Length) bound = m_Length;
        StringBuilder sb = new StringBuilder();
        int c;
        while ((m_Index + 1) < bound && (c = ((Buffer[m_Index++] << 8) | Buffer[m_Index++])) != 0)
        {
            if (IsSafeChar(c)) sb.Append((char)c);
        }
        m_Index = end;
        return sb.ToString();
    }

    public string ReadUnicodeString(int fixedLength)
    {
        int bound = m_Index + (fixedLength << 1);
        int end = bound;
        if (bound > m_Length) bound = m_Length;
        StringBuilder sb = new StringBuilder();
        int c;
        while ((m_Index + 1) < bound && (c = ((Buffer[m_Index++] << 8) | Buffer[m_Index++])) != 0)
            sb.Append((char)c);
        m_Index = end;
        return sb.ToString();
    }

    public string ReadStringSafe(int fixedLength)
    {
        int bound = m_Index + fixedLength;
        int end = bound;
        if (bound > m_Length) bound = m_Length;
        StringBuilder sb = new StringBuilder();
        int c;
        while (m_Index < bound && (c = Buffer[m_Index++]) != 0)
        {
            if (IsSafeChar(c)) sb.Append((char)c);
        }
        m_Index = end;
        return sb.ToString();
    }

    public string ReadString(int fixedLength)
    {
        int bound = m_Index + fixedLength;
        int end = bound;
        if (bound > m_Length) bound = m_Length;
        StringBuilder sb = new StringBuilder();
        int c;
        while (m_Index < bound && (c = Buffer[m_Index++]) != 0)
            sb.Append((char)c);
        m_Index = end;
        return sb.ToString();
    }

    #endregion

    public void TraceBuffer(byte[] buffer, int size)
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"(Tracing Buffer) {m_Length} bytes");
        for (int i = 0; i < size; i += 16)
        {
            StringBuilder line = new StringBuilder();
            StringBuilder chars = new StringBuilder();
            line.AppendFormat("{0:X4}: ", i);
            for (int j = 0; j < 16; j++)
            {
                if (i + j < size)
                {
                    byte b = buffer[i + j];
                    line.AppendFormat("{0:X2} ", b);
                    chars.Append(b >= 0x20 && b < 0x7F ? (char)b : '.');
                }
                else
                {
                    line.Append("   ");
                    chars.Append(' ');
                }

                if (j == 7)
                    line.Append("- ");
            }
            Console.WriteLine($"{line} {chars}");
        }
        Console.ResetColor();
        Console.WriteLine();
    }

    public void Trace(bool buffer = false)
    {
        ConsoleColor color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"(Tracing Packet) '{m_Name}' ( {m_Command} 0x{m_Command:X} ).. Size:{m_Length}");
        if (buffer)
            TraceBuffer(Buffer, m_Length);
        Console.ForegroundColor = color;
        Console.WriteLine();
    }
}