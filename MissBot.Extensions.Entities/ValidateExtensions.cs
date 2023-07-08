using System.Runtime.CompilerServices;

namespace MissBot.Extensions
{
    /// <summary>
    /// Extension Methods
    /// </summary>
    public static class ValidateExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIfNull<T>(this T value, string parameterName) =>
            value ?? throw new ArgumentNullException(parameterName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIfTypeIs<T>(this T value, string parameterName, params Type[] types) =>
            types.Contains(typeof(T)) ? throw new ArgumentException(parameterName) : value;
    }
}

