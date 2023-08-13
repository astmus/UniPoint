using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using LinqToDB.Mapping;
using MissBot.Abstractions.Actions;
using MissBot.Entities.Abstractions;
using MissCore.DataAccess;
using MissCore.Internal;

namespace MissCore.Bot
{
	/// <summary>Construct a Range object using the start and end indexes.</summary>
	/// <param name="start">Represent the inclusive start index of the range.</param>
	/// <param name="end">Represent the exclusive end index of the range.</param>
	[Table("##BotUnits")]
	public record Paging : IEquatable<Paging>, IBotEntity
	{
		Index Start
			=> Skip;

		Index End
			=> Skip + PageSize;

		#region Default implementation
		/// <summary>Indicates whether the current Range object is equal to another Range object.</summary>
		/// <param name="other">An object to compare with this object</param>
		public bool Equals(Range other)
			=> other.Start.Equals(Start) && other.End.Equals(End);

		public virtual bool Equals(Paging other)
			=> other.Start.Equals(Start) && other.End.Equals(End);

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
		//		public override string ToString()
		//		{
		//#if (!NETSTANDARD2_0 && !NETFRAMEWORK)
		//			Span<char> span = stackalloc char[2 + (2 * 11)]; // 2 for "..", then for each index 1 for '^' and 10 for longest possible uint
		//			int pos = 0;

		//			if (Start.IsFromEnd)
		//			{
		//				span[0] = '^';
		//				pos = 1;
		//			}
		//			bool formatted = ((uint)Start.Value).TryFormat(span.Slice(pos), out int charsWritten);
		//			Debug.Assert(formatted);
		//			pos += charsWritten;

		//			span[pos++] = '.';
		//			span[pos++] = '.';

		//			if (End.IsFromEnd)			
		//				span[pos++] = '^';

		//			formatted = ((uint)End.Value).TryFormat(span.Slice(pos), out charsWritten);
		//			Debug.Assert(formatted);
		//			pos += charsWritten;

		//			return new string(span.Slice(0, pos));
		//#else
		//            return Start.ToString() + ".." + End.ToString();
		//#endif
		//		}

		/// <summary>Create a Range object starting from start index to the end of the collection.</summary>
		public static Range StartAt(Index start) => new Range(start, Index.End);

		/// <summary>Create a Range object starting from first element in the collection to the end Index.</summary>
		public static Range EndAt(Index end) => new Range(Index.Start, end);

		/// <summary>Create a Range object starting from first element to the end.</summary>
		public static Range All => new Range(Index.Start, Index.End);

		/// <summary>Calculate the start offset and length of range object using a collection length.</summary>
		/// <param name="length">The length of the collection that the range will be used with. length has to be a positive value.</param>
		/// <remarks>
		/// For performance reason, we don't validate the input length parameter against negative values.
		/// It is expected Range will be used with collections which always have non negative length/count.
		/// We validate the range is inside the length scope though.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public (int Offset, int Length) GetOffsetAndLength(int length)
		{
			int start;
			Index startIndex = Start;
			if (startIndex.IsFromEnd)
				start = length - startIndex.Value;
			else
				start = startIndex.Value;

			int end;
			Index endIndex = End;
			if (endIndex.IsFromEnd)
				end = length - endIndex.Value;
			else
				end = endIndex.Value;

			if ((uint)end > (uint)length || (uint)start > (uint)end)
				Thrower.ThrowArgumentOutOfRangeException(length);

			return (start, end - start);
		}
		#endregion

		[Column]
		public string Entity
			=> nameof(Paging);

		[Column]
		public virtual string Template { get; set; }

		public int Skip
			=> Page * PageSize;
		public int Page { get; set; } = 0;
		public int PageSize { get; set; } = 32;

		public override string ToString()
			=> string.Format(Template, Skip, PageSize);
	}

	//public record Paging : BotUnit
	//{
	//	public uint Skip
	//		=> Page * PageSize;
	//	public uint Page { get; set; } = 0;
	//	public uint PageSize { get; set; } = 32;
	//	public override string ToString()
	//		=> string.Format(Template, Skip, PageSize);
	//}

	public record Search : UnitRequest
	{
		public string Query { get; set; }
		public Paging Pager { get; set; }
	}

	public record Search<TUnit> : Search, ISearchUnitRequest<TUnit>
	{
		public Search()
		{
			Identifier = base.Identifier;
		}
		public override object Identifier { get; init; }
		public override string Get(params object[] args)
		{
			return $"{string.Format(Template, Query)} {Pager} {string.Join(", ", args)}";
		}

		public void SetEtalone(TUnit unit)
		{
			throw new NotImplementedException();
		}
	}
}
