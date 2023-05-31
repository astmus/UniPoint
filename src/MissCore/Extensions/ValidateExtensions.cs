using System.Runtime.CompilerServices;

namespace MissCore.Extensions
{
    /// <summary>
    /// Extension Methods
    /// </summary>
    public static class ValidateExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIfNull<T>(this T value, string parameterName) =>
            value ?? throw new ArgumentNullException(parameterName);
    }
}

