namespace Client.Networking;
using System;
using System.Runtime.ExceptionServices;
using System.Text;

/// <summary>
///     Write methods by default are BigEndian
/// </summary>
public class PacketWriter

{
    protected byte[] Buffer;
    protected readonly int Capacity;
    public byte Command { get; }
    public int Length => Buffer.Length;
    public int Index { get; protected set; }
    public PacketWriter(int capacity = 32)
    {
        Buffer = new byte[capacity];
        Capacity = capacity;
    }
    
    public void EnsureCapacity(int additionalCapacity)
    {
        if (Index + additionalCapacity > Buffer.Length)
        {
            Array.Resize(ref Buffer, Math.Max(Buffer.Length * 2, Index + additionalCapacity));
        }
    }
    protected bool IsSafeChar(int c) => c >= 0x20 && c < 0xFFFE;
    public void FilltoCapacity() => FillwithZeros(count: Capacity - Index);
    public void FillwithZeros(int count)
    {
        EnsureCapacity(count);
        for (int i = 0; i < count; i++)
            Buffer[Index++] = 0x00;
    }
    public void Seek(int offset, SeekOrigin origin)
    {
        Index = origin switch
        {
            SeekOrigin.Begin => offset,
            SeekOrigin.Current => Index + offset,
            SeekOrigin.End => Buffer.Length - offset,
            _ => throw new ArgumentOutOfRangeException(nameof(origin))
        };
    }
    public void Write(bool value) => Write((byte)(value ? 1 : 0));
    public void Write(sbyte value) => Write((byte)value);
    public void Write(byte value)
    {
        EnsureCapacity(1);
        Buffer[Index++] = value;
    }
    public void Write(byte[] buffer, int offset, int length)
    {
        EnsureCapacity(length);
        Array.Copy(buffer, offset, Buffer, Index, length);
        Index += length;
    }
    public void WriteAscii(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Write((byte)0);
            return;
        }

        for(int i = 0; i < value.Length; i++)
            Write((byte)value[i]);
        
        Write((byte)0);
    }
    public void WriteAscii(string value, int fixedLength)
    {
        EnsureCapacity(fixedLength);
        int i;
        for (i = 0; i < fixedLength && i < value.Length; i++)
        {
            Buffer[Index++] = (byte)value[i];
        }
        for (; i < fixedLength; i++)
        {
            Buffer[Index++] = 0;
        }
    }
    public void WriteUnicodeLESafe(string value, int fixedLength)
    {
        EnsureCapacity(fixedLength * 2);
        int i;
        for (i = 0; i < fixedLength && i < value.Length; i++)
        {
            char c = value[i];
            if (IsSafeChar(c))
            {
                Buffer[Index++] = (byte)c;
                Buffer[Index++] = (byte)(c >> 8);
            }
            else
            {
                Buffer[Index++] = (byte)'?';
                Buffer[Index++] = 0x00;
            }
        }
        for (; i < fixedLength; i++)
        {
            Buffer[Index++] = 0x00;
            Buffer[Index++] = 0x00;
        }
    }
    public void WriteUnicode(string value)
    {

        foreach (char c in value)
        {
            Write((byte)(c >> 8));
            Write((byte)c);
        }
        Write((ushort)0x00);
    }
    public void WriteUnicode(string value, int fixedLength)
    {
        EnsureCapacity(fixedLength * 2);
        int i;
        for (i = 0; i < fixedLength && i < value.Length; i++)
        {
            Buffer[Index++] = (byte)(value[i] >> 8);
            Buffer[Index++] = (byte)value[i];
        }
        for (; i < fixedLength; i++)
        {
            Buffer[Index++] = 0;
            Buffer[Index++] = 0;
        }
    }
    public void WriteunicodeLE(string value)
    {
        for(int i = 0; i<  value.Length; i++)
        {
            char c = value[i];
            Write((byte)c);
            Write((byte)(c >> 8));
        }
        Write((ushort)0);
    }
    public void WriteUnicodeLE(string value, int fixedLength)
    {
        EnsureCapacity(fixedLength * 2);
        int i;
        for (i = 0; i < fixedLength && i < value.Length; i++)
        {
            Buffer[Index++] = (byte)value[i];
            Buffer[Index++] = (byte)(value[i] >> 8);
        }
        for (; i < fixedLength; i++)
        {
            Buffer[Index++] = 0;
            Buffer[Index++] = 0;
        }
    }
    public void WriteUTF8(string value)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(value);
        Write(buffer, 0, buffer.Length);
        Write((byte)0);
    }
    public void Write(short value)
    {
        Write((byte)(value >> 8));
        Write((byte)value);
    }
    public void Write(int value)
    {
        Write((byte)(value >> 24));
        Write((byte)(value >> 16));
        Write((byte)(value >> 8));
        Write((byte)value);
    }
    public void Write(ushort value)
    {
        Write((byte)(value >> 8));
        Write((byte)value);
    }
    public void WriteLE(ushort value)
    {
        Write((byte)value);
        Write((byte)(value >> 8));
    }
    public void Write(uint value)
    {
        Write((byte)(value >> 24));
        Write((byte)(value >> 16));
        Write((byte)(value >> 8));
        Write((byte)value);
    }
    public void WriteLE(uint value)
    {
        Write((byte)value);
        Write((byte)(value >> 8));
        Write((byte)(value >> 16));
        Write((byte)(value >> 24));
    }
    public byte[] Compile() => Buffer[..Index];
    public void Trace()
    {
        ConsoleColor color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"(Tracing Packet) ( {Command} 0x{Command:X} ).. Size:{Index}");
        Console.ForegroundColor = color;
        Console.WriteLine();
    }
}

