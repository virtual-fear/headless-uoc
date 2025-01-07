using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace Client.Networking
{
    public class PacketWriter : IDisposable
    {
        private static byte[] m_Buffer = new byte[4];

        private int m_Capacity;
        private MemoryStream m_Stream;

        public PacketWriter()
            : this(64)
        {
        }

        internal PacketWriter(int capacity)
        {
            m_Stream = new MemoryStream((m_Capacity = capacity));
        }

        public Stream UnderlyingStream => m_Stream;
        public int Capacity => m_Capacity;
        public long Length => m_Stream.Length;

        public byte[] Compile()
        {
            lock (m_Stream)
            {
                m_Stream.Flush();
                return m_Stream.ToArray();
            }
        }

        public void Flush()
        {
            lock (m_Stream)
                m_Stream.Flush();
        }

        protected void Flush(int count)
        {
            Flush(0, count);
        }

        protected void Flush(int offset, int count)
        {
            Write(m_Buffer, offset, count);
        }

        public void Write(byte[] buffer)
        {
            lock (buffer)
            {
                Write((int)buffer.Length);
                m_Stream.Write(buffer, 0, buffer.Length);
            }
        }

        public void Write(bool value)
        {
            Write((byte)(value ? 1 : 0));
        }

        public void Write(byte value)
        {
            m_Stream.WriteByte(value);
        }

        public void Write(short value)
        {
            lock (m_Buffer)
            {
                m_Buffer[0] = (byte)(value >> 8);
                m_Buffer[1] = (byte)(value >> 0);
                Flush(0, 2);
            }
        }

        public void WriteUInt32_BE(uint value)
        {
            lock(m_Stream)
            {
                Span<byte> buffer = stackalloc byte[4];
                BinaryPrimitives.WriteUInt32BigEndian(buffer, value);
                m_Stream.Write(buffer);
            }
        }

        public void WriteInt32_LE(uint value)
        {
            lock (m_Buffer)
            {
                var buffer = BitConverter.GetBytes(value);
                if (!BitConverter.IsLittleEndian)
                    Array.Reverse(buffer);

                m_Buffer[0] = buffer[0];
                m_Buffer[1] = buffer[1];
                m_Buffer[2] = buffer[2];
                m_Buffer[3] = buffer[3];
                Flush();
            }
        }

        public void Write(int value)
        {
            lock (m_Buffer)
            {
                m_Buffer[0] = (byte)(value >> 24);
                m_Buffer[1] = (byte)(value >> 16);
                m_Buffer[2] = (byte)(value >> 08);
                m_Buffer[3] = (byte)(value >> 00);
                Flush(0, 4);
            }
        }

        public void Write(long value)
        {
            lock (m_Buffer)
            {
                m_Buffer[0] = (byte)(value >> 56);
                m_Buffer[1] = (byte)(value >> 48);
                m_Buffer[2] = (byte)(value >> 40);
                m_Buffer[4] = (byte)(value >> 32);
                Flush(0, 4);
                m_Buffer[0] = (byte)(value >> 24);
                m_Buffer[1] = (byte)(value >> 16);
                m_Buffer[2] = (byte)(value >> 08);
                m_Buffer[3] = (byte)(value >> 00);
                Flush(0, 4);
            }
        }

        public void Write(sbyte value)
        {
            Write((byte)value);
        }

        public void Write(ushort value)
        {
            Write((short)value);
        }

        public void Write(uint value)
        {
            Write((int)value);
        }

        public void Write(ulong value)
        {
            Write((long)value);
        }

        public void Write(string value)
        {
            Write(Encoding.ASCII.GetBytes(value));
        }

        public void WriteUnicode(string text)
        {
            Write(Encoding.BigEndianUnicode.GetBytes(text));
        }

        /// <summary>
        /// Writes a fixed-length ASCII-encoded string value to the underlying stream. To fit (size), the string content is either truncated or padded with null characters.
        /// </summary>
        public void WriteAsciiFixed(string text, int size)
        {
            if (text == null)
                text = String.Empty;

            int length = text.Length;

            if (text.Length > size)
                length = size;

            if (length > 0)
            {
                Write(Encoding.ASCII.GetBytes(text.ToCharArray()), 0, length);
            }

            size -= length;

            if (size > 0)
            {
                Fill(size);
            }
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            m_Stream.Write(buffer, offset, count);
        }

        /// <summary>
        /// Fills the stream from the current position up to (capacity) with 0x00's
        /// </summary>
        public void Fill()
        {
            Fill((int)(m_Capacity - m_Stream.Length));
        }

        /// <summary>
        /// Writes a number of 0x00 byte values to the underlying stream.
        /// </summary>
        public void Fill(int count)
        {
            if (m_Stream.Position != m_Stream.Length)
            {
                Write(new byte[count], 0, count);
            }
            else
            {
                m_Stream.SetLength((m_Stream.Length + count));
                m_Stream.Position = m_Stream.Length;
            }
        }

        /// <summary>
        /// Offsets the current position from an origin.
        /// </summary>
        public long Seek(long offset, SeekOrigin loc)
        {
            return m_Stream.Seek(offset, loc);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_Capacity = 0;
            if (m_Stream != null)
            {
                m_Stream.Dispose();
                m_Stream = null;
            }
        }

        #endregion IDisposable Members
    }
}
