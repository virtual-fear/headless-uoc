namespace Client.Cryptography.Impl
{
    using Networking.IO;
    internal sealed class DefaultCrypto : Crypto
    {
        public DefaultCrypto() : base(0) { }
        public override void Encrypt(byte[] buffer, int offset, int length, IConsolidator output) => output.Enqueue(buffer, offset, length);
        public override void Decrypt(byte[] buffer, int offset, int length, IConsolidator output) => output.Enqueue(buffer, offset, length);
        public override int Decode(byte[] src, int srcOffset, byte[] dst, int dstOffset, int length)
        {
            for (int x = 0; x < length; x++)
                dst[dstOffset + x] = src[srcOffset + x];

            return length;
        }
    }
}
