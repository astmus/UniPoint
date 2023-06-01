using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MissBot.Common.Extensions
{
    internal static class StringExtensions
    {
        public static LineSplitEnumerator SplitLines(this string str)
            => new LineSplitEnumerator(str.AsSpan());

        public static SplitEnumerator SplitParameters(this string str)
            => new SplitEnumerator(str.AsSpan(), ';');

        public ref struct SplitEnumerator
        {
            private ReadOnlySpan<char> _str;
            private ReadOnlySpan<char> _separators;


            public SplitEnumerator(ReadOnlySpan<char> splitTarget, params char[] separators)
            {
                _str = splitTarget;
                _separators = separators.AsSpan();
                Current = default;
            }

            public SplitEnumerator GetEnumerator()
                => this;

            public bool MoveNext()
            {
                var span = _str;
                if (span.Length == 0)
                    return false;

                var index = span.IndexOfAny(_separators);
                if (index == -1)
                {
                    _str = ReadOnlySpan<char>.Empty;
                    Current = new SplitEntry(span, ReadOnlySpan<char>.Empty);
                    return true;
                }

                if (index < span.Length - 1)
                    if (span[index + 1] is char next && !_separators.Contains(next))
                    {
                        var start = span[..index];                                            
                        Current = new SplitEntry(start, span.Slice(index,1));
                        _str = span[(index + 1)..];
                        return true;
                    }

                Current = new SplitEntry(span[..index], span.Slice(index, 1));
                _str = span[(index + 1)..];
                return true;
            }

            public SplitEntry Current { get; private set; }
        }
        public ref struct LineSplitEnumerator
        {
            private ReadOnlySpan<char> _str;

            public LineSplitEnumerator(ReadOnlySpan<char> splitTarget)
            {
                _str = splitTarget;
                Current = default;
            }

            public LineSplitEnumerator GetEnumerator() => this;

            public bool MoveNext()
            {
                var span = _str;
                if (span.Length == 0)
                    return false;

                var index = span.IndexOfAny('\r', '\n');
                if (index == -1)
                {
                    _str = ReadOnlySpan<char>.Empty;
                    Current = new SplitEntry(span, ReadOnlySpan<char>.Empty);
                    return true;
                }

                if (index < span.Length - 1 && span[index] == '\r')
                {
                    // Try to consume the '\n' associated to the '\r'
                    var next = span[index + 1];
                    if (next == '\n')
                    {
                        Current = new SplitEntry(span[..index], span.Slice(index, 2));
                        _str = span[(index + 2)..];
                        return true;
                    }
                }

                Current = new SplitEntry(span[..index], span.Slice(index, 1));
                _str = span[(index + 1)..];
                return true;
            }

            public SplitEntry Current { get; private set; }
        }

    }
    internal readonly ref struct SplitEntry
    {
        public SplitEntry(ReadOnlySpan<char> entry, ReadOnlySpan<char> delimiter)
        {
            Segment = entry;
            Delimiter = delimiter;
        }

        public ReadOnlySpan<char> Segment { get; }
        public ReadOnlySpan<char> Delimiter { get; }

        // This method allow to deconstruct the type, so you can write any of the following code
        // foreach (var entry in str.SplitLines()) { _ = entry.Line; }
        // foreach (var (line, endOfLine) in str.SplitLines()) { _ = line; }
        public void Deconstruct(out ReadOnlySpan<char> entry, out ReadOnlySpan<char> delimiter)
        {
            entry = Segment;
            delimiter = Delimiter;
        }

        // This method allow to implicitly cast the type into a ReadOnlySpan<char>, so you can write the following code
        // foreach (ReadOnlySpan<char> entry in str.SplitLines())
        public static implicit operator ReadOnlySpan<char>(SplitEntry entry)
            => entry.Segment;

        public static implicit operator string(SplitEntry entry)
            => entry.Segment.ToString();
    }
}
