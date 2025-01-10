using System.Runtime.CompilerServices;
namespace Client.Game.Data;
public readonly struct Serial : IComparable<Serial>, IComparable<uint>,
    IEquatable<Serial>, ISpanFormattable
{
    public static readonly Serial MinusOne = new(0xFFFFFFFF);
    public static readonly Serial Zero = new(0);

    private Serial(uint serial) => Value = serial;

    public uint Value { get; }

    public bool IsMobile
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value is > 0 and < World.ItemOffset;
    }

    public bool IsItem
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value is >= World.ItemOffset and < World.MaxItemSerial;
    }

    public bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value > 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => Value.GetHashCode();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(Serial other) => Value.CompareTo(other.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(uint other) => Value.CompareTo(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object obj) =>
        obj switch
        {
            Serial serial => this == serial,
            uint u => Value == u,
            _ => false
        };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Serial l, Serial r) => l.Value == r.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Serial l, uint r) => l.Value == r;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Serial l, Serial r) => l.Value != r.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Serial l, uint r) => l.Value != r;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(Serial l, Serial r) => l.Value > r.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(Serial l, uint r) => l.Value > r;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(Serial l, Serial r) => l.Value < r.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(Serial l, uint r) => l.Value < r;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(Serial l, Serial r) => l.Value >= r.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(Serial l, uint r) => l.Value >= r;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(Serial l, Serial r) => l.Value <= r.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(Serial l, uint r) => l.Value <= r;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Serial operator +(Serial l, Serial r) => (Serial)(l.Value + r.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Serial operator +(Serial l, uint r) => (Serial)(l.Value + r);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Serial operator ++(Serial l) => (Serial)(l.Value + 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Serial operator -(Serial l, Serial r) => (Serial)(l.Value - r.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Serial operator -(Serial l, uint r) => (Serial)(l.Value - r);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Serial operator --(Serial l) => (Serial)(l.Value - 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        // Maximum number of characters that are needed to represent this:
        // 2 characters for 0x
        // Up to 8 characters to represent the value in hex
        Span<char> span = stackalloc char[10];
        TryFormat(span, out var charsWritten, null, null);
        return span[..charsWritten].ToString();
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        // format and formatProvider are not doing anything right now, so use the
        // default ToString implementation.
        return ToString();
    }

    public bool TryFormat(
        Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider
    ) => format != ReadOnlySpan<char>.Empty
        ? Value.TryFormat(destination, out charsWritten, format, provider)
        : destination.TryWrite(provider, $"0x{Value:X8}", out charsWritten);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator uint(Serial a) => a.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Serial(uint a) => new(a);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Serial other) => Value == other.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ToInt32() => (int)Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Serial Parse(string s) => Parse(s, null);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Serial Parse(string s, IFormatProvider provider) => Parse(s.AsSpan(), provider);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse(string s, IFormatProvider provider, out Serial result) =>
        TryParse(s.AsSpan(), provider, out result);

    public static Serial Parse(ReadOnlySpan<char> s, IFormatProvider provider) => new(uint.Parse(s));
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out Serial result)
    {
        if (uint.TryParse(s, out var value))
        {
            result = new Serial(value);
            return true;
        }
        result = default;
        return false;
    }
}
