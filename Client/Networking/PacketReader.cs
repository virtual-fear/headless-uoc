namespace Client.Networking;
using System;
using System.Text;

public class PacketReader
{
    private readonly StringBuilder TextBuilder;
    protected readonly byte[] Buffer;
    public string Name { get; }
    public byte Command { get; }
    public int Length { get; }
    public int Index { get; protected set; }
    internal PacketReader(Span<byte> buffer, bool fixedSize, byte cmd, string name)
    {
        if (buffer == null)
            throw new ArgumentNullException(nameof(buffer));

        TextBuilder = new StringBuilder();
        Buffer = buffer.ToArray();
        Length = buffer.Length;
        Index = fixedSize ? 1 : 3;
        Command = cmd;
        Name = name;
    }
    internal static PacketReader Create(
        ref Span<byte> buffer,
        ref PacketHandler handler)
        => new(buffer,
            fixedSize: (handler.Length != -1),
                  cmd: (byte)handler.PacketID,
                  name: handler.Name);

    protected bool IsSafeChar(int c) => c >= 0x20 && c < 0xFFFE;
    public int Seek(int offset, SeekOrigin origin) => Index += origin switch
    {
        SeekOrigin.Begin => offset - Index,
        SeekOrigin.Current => offset,
        SeekOrigin.End => Length - (offset - Index),
        _ => throw new ArgumentOutOfRangeException(nameof(origin))
    };
    public bool ReadBoolean() => Buffer[Index] == 1;
    public sbyte ReadSByte() => (sbyte)Buffer[Index++];
    public byte ReadByte() => Buffer[Index++];
    public byte[] ReadBytes(int length)
    {
        Span<byte> data = stackalloc byte[length];
        for (int i = 0; i < length; i++)
            data[i] = ReadByte();
        return data.ToArray();
    }

    #region Strings

    #region (Unicode)
    public string ReadString()
    {
        StringBuilder sb = new StringBuilder();
        int c;
        while (Index < Length && (c = Buffer[Index++]) != 0)
            sb.Append((char)c);
        return sb.ToString();
    }
    public string ReadString(int fixedLength)
    {
        int bound = Index + fixedLength;
        int end = bound;
        if (bound > Length) bound = Length;
        var sb = TextBuilder;
        int c;
        sb.Clear();
        while (Index < bound && (c = Buffer[Index++]) != 0)
            sb.Append((char)c);
        Index = end;
        return sb.ToString();
    }
    public string ReadStringSafe()
    {
        StringBuilder sb = new StringBuilder();
        int c;
        while (Index < Length && (c = Buffer[Index++]) != 0)
        {
            if (IsSafeChar(c)) sb.Append((char)c);
        }
        return sb.ToString();
    }
    public string ReadStringSafe(int fixedLength)
    {
        int bound = Index + fixedLength;
        int end = bound;
        if (bound > Length) bound = Length;
        StringBuilder sb = new StringBuilder();
        int c;
        while (Index < bound && (c = Buffer[Index++]) != 0)
        {
            if (IsSafeChar(c)) sb.Append((char)c);
        }
        Index = end;
        return sb.ToString();
    }
    public string ReadUnicodeString()
    {
        StringBuilder sb = new StringBuilder();
        int c;
        while (Index < Length)
        {
            c = ReadByte();
            if (c == 0x00) continue;
            sb.Append((char)c);
        }
        return sb.ToString();
    }
    public string ReadUnicodeString(int fixedLength)
    {
        int bound = Index + (fixedLength << 1);
        int end = bound;
        if (bound > Length) bound = Length;
        StringBuilder sb = new StringBuilder();
        int c;
        while ((Index + 1) < bound && (c = ((Buffer[Index++] << 8) | Buffer[Index++])) != 0)
            sb.Append((char)c);
        Index = end;
        return sb.ToString();
    }
    public string ReadUnicodeStringLE()
    {
        StringBuilder sb = new StringBuilder();
        int c;
        while ((Index + 1) < Length && (c = (Buffer[Index++] | (Buffer[Index++] << 8))) != 0)
            sb.Append((char)c);
        return sb.ToString();
    }
    public string ReadUnicodeStringLESafe(int fixedLength)
    {
        int bound = Index + (fixedLength << 1);
        int end = bound;
        if (bound > Length) bound = Length;
        StringBuilder sb = new StringBuilder();
        int c;
        while ((Index + 1) < bound && (c = (Buffer[Index++] | (Buffer[Index++] << 8))) != 0)
        {
            if (IsSafeChar(c)) sb.Append((char)c);
        }
        Index = end;
        return sb.ToString();
    }
    public string ReadUnicodeStringLESafe()
    {
        StringBuilder sb = new StringBuilder();
        int c;
        while ((Index + 1) < Length && (c = (Buffer[Index++] | (Buffer[Index++] << 8))) != 0)
        {
            if (IsSafeChar(c)) sb.Append((char)c);
        }
        return sb.ToString();
    }
    public string ReadUnicodeStringSafe()
    {
        StringBuilder sb = new StringBuilder();
        int c;
        while ((Index + 1) < Length && (c = ((Buffer[Index++] << 8) | Buffer[Index++])) != 0)
        {
            if (IsSafeChar(c)) sb.Append((char)c);
        }
        return sb.ToString();
    }
    public string ReadUnicodeStringSafe(int fixedLength)
    {
        int bound = Index + (fixedLength << 1);
        int end = bound;
        if (bound > Length) bound = Length;
        StringBuilder sb = new StringBuilder();
        int c;
        while ((Index + 1) < bound && (c = ((Buffer[Index++] << 8) | Buffer[Index++])) != 0)
        {
            if (IsSafeChar(c)) sb.Append((char)c);
        }
        Index = end;
        return sb.ToString();
    }
    #endregion

    #region (UTF8)
    public string ReadUTF8String()
    {
        if (Index >= Length) return String.Empty;
        int count = 0;
        int index = Index;
        while (index < Length && Buffer[index++] != 0) ++count;
        index = 0;
        byte[] buffer = new byte[count];
        int value = 0;
        while (Index < Length && (value = Buffer[Index++]) != 0)
            buffer[index++] = (byte)value;
        return Encoding.UTF8.GetString(buffer);
    }
    public string ReadUTF8StringSafe()
    {
        if (Index >= Length) return String.Empty;
        int count = 0;
        int index = Index;
        while (index < Length && Buffer[index++] != 0) ++count;
        index = 0;
        byte[] buffer = new byte[count];
        int value = 0;
        while (Index < Length && (value = Buffer[Index++]) != 0)
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
    public string ReadUTF8StringSafe(int fixedLength)
    {
        if (Index >= Length)
        {
            Index += fixedLength;
            return String.Empty;
        }
        int bound = Index + fixedLength;
        if (bound > Length) bound = Length;
        int count = 0;
        int index = Index;
        int start = Index;
        while (index < bound && Buffer[index++] != 0) ++count;
        index = 0;
        byte[] buffer = new byte[count];
        int value = 0;
        while (Index < bound && (value = Buffer[Index++]) != 0)
            buffer[index++] = (byte)value;
        string s = Encoding.UTF8.GetString(buffer);
        bool isSafe = true;
        for (int i = 0; isSafe && i < s.Length; ++i)
            isSafe = IsSafeChar((int)s[i]);
        Index = start + fixedLength;
        if (isSafe) return s;
        StringBuilder sb = new StringBuilder(s.Length);
        for (int i = 0; i < s.Length; ++i)
            if (IsSafeChar((int)s[i])) sb.Append(s[i]);
        return sb.ToString();
    }
    #endregion

    #endregion

    #region Signed
    public short ReadInt16() => (short)(Index + 2 > Length ? 0 : ((Buffer[Index++] << 8) | Buffer[Index++]));
    public int ReadInt32() => (Buffer[Index++] << 24) | (Buffer[Index++] << 16) | (Buffer[Index++] << 8) | Buffer[Index++];
    #endregion

    #region Unsigned
    public ushort ReadUInt16() => (ushort)(Buffer[Index++] << 8 | Buffer[Index++]);
    public ushort ReadUInt16LE() => (ushort)(Buffer[Index++] | Buffer[Index++] << 8);
    public uint ReadUInt32() => (uint)(Buffer[Index++] << 24 | Buffer[Index++] << 16 | Buffer[Index++] << 8 | Buffer[Index++]);
    public uint ReadUInt32LE() => (uint)(Buffer[Index++] | Buffer[Index++] << 8 | Buffer[Index++] << 16 | (Buffer[Index++] << 24));
    #endregion

    public void TraceBuffer(byte[] buffer, int size)
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"(Tracing Buffer) {Length} bytes");
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
        Console.WriteLine($"(Tracing Packet) '{Name}' ( {Command} 0x{Command:X} ).. Size:{Length}");
        if (buffer)
            TraceBuffer(Buffer, Length);
        Console.ForegroundColor = color;
        Console.WriteLine();
    }
}