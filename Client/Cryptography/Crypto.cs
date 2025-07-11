﻿namespace Client.Cryptography;
using Client.Cryptography.Impl;
using Client.IO;
public abstract class Crypto
{
    public uint Seed { get; }
    public Crypto(uint seed) => Seed = seed;
    public static Crypto UseDefault() => new DefaultCrypto();
    public abstract void Encrypt(byte[] buffer, int offset, int length, IConsolidator output);
    public abstract void Decrypt(byte[] buffer, int offset, int length, IConsolidator output);
    public abstract int Decode(byte[] src, int srcOffset, byte[] dst, int dstOffset, int length);
    public override string ToString() => GetType().Name;
}
