using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using LinqToDB.Mapping;
using MissBot.Abstractions;
using Newtonsoft.Json.Linq;

namespace MissCore.Internal
{
	public static class Thrower
	{
		public static void ThrowArgumentOutOfRangeException(object? argument)
			=> Throw<ArgumentOutOfRangeException>(argument);

		/// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
		/// <param name="argument">The reference type argument to validate as non-null.</param>
		/// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
		internal static void ThrowIfNull(object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
		{
			if (argument is null)
			{
				Throw(paramName);
			}
		}
#if NETCOREAPP3_0_OR_GREATER
		[DoesNotReturn]
#endif
		internal static void Throw(object? paramName)
			=> throw new ArgumentNullException(paramName.ToString());

		internal static void Throw<TException>(object? paramName) where TException : Exception
			=> throw Activator.CreateInstance(typeof(TException), paramName) as TException;
	}
}
