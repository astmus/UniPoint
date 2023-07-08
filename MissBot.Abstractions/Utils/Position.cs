using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace MissBot.Abstractions.Utils
{
    public record struct Position
    {
        ushort value;
        public int Current => value;
        public PositionRange Range { get; }
        public int Forward()
            => ++value;

            public int Back()
            => value > 0 ?  --value : value;
    }
    public record struct PositionRange : IEquatable<PositionRange>
    {
        /// <summary>Represent the inclusive start PositionIndex of the Range.</summary>
        public PositionIndex Start { get; }

        /// <summary>Represent the exclusive end PositionIndex of the Range.</summary>
        public PositionIndex End { get; }

        /// <summary>Construct a Range object using the start and end PositionIndexes.</summary>
        /// <param name="start">Represent the inclusive start PositionIndex of the range.</param>
        /// <param name="end">Represent the exclusive end PositionIndex of the range.</param>
        public PositionRange(PositionIndex start, PositionIndex end)
        {
            Start = start;
            End = end;
        }

        /// <summary>Indicates whether the current Range object is equal to another object of the same type.</summary>
        /// <param name="value">An object to compare with this object</param>
        //public override bool Equals([NotNullWhen(true)] object value) =>
        //    value is PositionRange r &&
        //    r.Start.Equals(Start) &&
        //    r.End.Equals(End);

        /// <summary>Indicates whether the current Range object is equal to another Range object.</summary>
        /// <param name="other">An object to compare with this object</param>
        public bool Equals(PositionRange other) => other.Start.Equals(Start) && other.End.Equals(End);

        /// <summary>Returns the hash code for this instance.</summary>
        public override int GetHashCode()
        {
#if (!NETSTANDARD2_0 && !NETFRAMEWORK)
            return HashCode.Combine(Start.GetHashCode(), End.GetHashCode());
#else
            return HashHelpers.Combine(Start.GetHashCode(), End.GetHashCode());
#endif
        }

        /// <summary>Converts the value of the current Range object to its equivalent string representation.</summary>
        public override string ToString()
        {
#if (!NETSTANDARD2_0 && !NETFRAMEWORK)
            Span<char> span = stackalloc char[2 + (2 * 11)]; // 2 for "..", then for each PositionIndex 1 for '^' and 10 for longest possible uint
            int pos = 0;

            if (Start.IsFromEnd)
            {
                span[0] = '^';
                pos = 1;
            }
            bool formatted = Start.Value.TryFormat(span.Slice(pos), out int charsWritten, provider: default);
            Debug.Assert(formatted);
            pos += charsWritten;

            span[pos++] = '.';
            span[pos++] = '.';

            if (End.IsFromEnd)
            {
                span[pos++] = '^';
            }
            formatted = End.Value.TryFormat(span.Slice(pos), out charsWritten, provider:default);
            Debug.Assert(formatted);
            pos += charsWritten;

            return new string(span.Slice(0, pos));
#else
            return Start.ToString() + ".." + End.ToString();
#endif
        }

        /// <summary>Create a Range object starting from start PositionIndex to the end of the collection.</summary>
        public static PositionRange StartAt(PositionIndex start) => new PositionRange(start, PositionIndex.End);

        /// <summary>Create a Range object starting from first element in the collection to the end PositionIndex.</summary>
        public static PositionRange EndAt(PositionIndex end) => new PositionRange(PositionIndex.Start, end);

        /// <summary>Create a PositionRange object starting from first element to the end.</summary>
        public static PositionRange All => new PositionRange(PositionIndex.Start, PositionIndex.End);

        /// <summary>Calculate the start offset and length of PositionRange object using a collection length.</summary>
        /// <param name="length">The length of the collection that the PositionRange will be used with. length has to be a positive value.</param>
        /// <remarks>
        /// For performance reason, we don't validate the input length parameter against negative values.
        /// It is expected PositionRange will be used with collections which always have non negative length/count.
        /// We validate the PositionRange is inside the length scope though.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (int Offset, int Length) GetOffsetAndLength(int length)
        {
            int start;
            PositionIndex startPositionIndex = Start;
            if (startPositionIndex.IsFromEnd)
                start = length - startPositionIndex.Value;
            else
                start = startPositionIndex.Value;

            int end;
            PositionIndex endPositionIndex = End;
            if (endPositionIndex.IsFromEnd)
                end = length - endPositionIndex.Value;
            else
                end = endPositionIndex.Value;

            if ((uint)end > (uint)length || (uint)start > (uint)end)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            return (start, end - start);
        }
    }

    public record struct PositionIndex : IEquatable<PositionIndex>
    {
        private readonly ushort _value;

        /// <summary>Construct an PositionIndex using a value and indicating if the PositionIndex is from the start or from the end.</summary>
        /// <param name="value">The PositionIndex value. it has to be zero or positive number.</param>
        /// <param name="fromEnd">Indicating if the PositionIndex is from the start or from the end.</param>
        /// <remarks>
        /// If the PositionIndex constructed from the end, PositionIndex value 1 means pointing at the last element and PositionIndex value 0 means pointing at beyond last element.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PositionIndex(ushort value, bool fromEnd = false)
        {

            if (fromEnd)
                _value = (ushort)~value;
            else
                _value = value;
        }

        // The following private constructors mainly created for perf reason to avoid the checks
        private PositionIndex(ushort value)
        {
            _value = value;
        }

        /// <summary>Create an PositionIndex pointing at first element.</summary>
        public static PositionIndex Start => new PositionIndex(0);

        /// <summary>Create an PositionIndex pointing at beyond last element.</summary>
        public static PositionIndex End => new PositionIndex(Convert.ToUInt16(~0));

        /// <summary>Create an PositionIndex from the start at the position indicated by the value.</summary>
        /// <param name="value">The PositionIndex value from the start.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PositionIndex FromStart(ushort value)
            => new PositionIndex(value);
        

        /// <summary>Create an PositionIndex from the end at the position indicated by the value.</summary>
        /// <param name="value">The PositionIndex value from the end.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PositionIndex FromEnd(ushort value)
            =>  new PositionIndex(Convert.ToUInt16(~value));
        
        /// <summary>Returns the PositionIndex value.</summary>
        public ushort Value
        {
            get
            {
                if (_value < 0)
                    return (ushort)~_value;
                else
                    return _value;
            }
        }

        /// <summary>Indicates whether the PositionIndex is from the start or the end.</summary>
        public bool IsFromEnd => _value < 0;

        /// <summary>Calculate the offset from the start using the giving collection length.</summary>
        /// <param name="length">The length of the collection that the PositionIndex will be used with. length has to be a positive value</param>
        /// <remarks>
        /// For performance reason, we don't validate the input length parameter and the returned offset value against negative values.
        /// we don't validate either the returned offset is greater than the input length.
        /// It is expected PositionIndex will be used with collections which always have non negative length/count. If the returned offset is negative and
        /// then used to PositionIndex a collection will get out of range exception which will be same affect as the validation.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetOffset(int length)
        {
            int offset = _value;
            if (IsFromEnd)
            {
                // offset = length - (~value)
                // offset = length + (~(~value) + 1)
                // offset = length + value + 1

                offset += length + 1;
            }
            return offset;
        }

        /// <summary>Indicates whether the current PositionIndex object is equal to another object of the same type.</summary>
        /// <param name="obj">An object to compare with this object</param>
        //public override bool Equals([NotNullWhen(true)] object obj) => obj is PositionIndex && _value == ((PositionIndex)obj)._value;

        /// <summary>Indicates whether the current PositionIndex object is equal to another PositionIndex object.</summary>
        /// <param name="other">An object to compare with this object</param>
        public bool Equals(PositionIndex other) => _value == other._value;

        /// <summary>Returns the hash code for this instance.</summary>
        public override int GetHashCode() => _value;

        /// <summary>Converts integer number to an PositionIndex.</summary>
        public static implicit operator PositionIndex(ushort value) => FromStart(value);

        /// <summary>Converts the value of the current PositionIndex object to its equivalent string representation.</summary>
        public override string ToString()
        {
            if (IsFromEnd)
                return ToStringFromEnd();

            return Value.ToString(provider: default);
        }

        private string ToStringFromEnd()
        {
#if (!NETSTANDARD2_0 && !NETFRAMEWORK)
            Span<char> span = stackalloc char[11]; // 1 for ^ and 10 for longest possible uushort value
            Debug.Assert(Value.TryFormat(span[1..], out var charsWritten, provider: default));
            span[0] = '^';
            return new string(span.Slice(0, charsWritten + 1));
#else
            return '^' + Value.ToString();
#endif
        }
    }

}
