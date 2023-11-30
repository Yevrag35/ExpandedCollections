using System;
using System.Diagnostics.CodeAnalysis;

namespace MG.Collections.Extensions.Arrays
{
    /// <summary>
    /// An extension class for checking element sizes on <see cref="Array"/> instances.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Determines if the <see cref="Array"/> is empty (i.e. - count is equal to 0).
        /// </summary>
        /// <param name="array">The array to check.</param>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="Array"/> contains 0
        ///     elements; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="array"/> is <see langword="null"/>.
        /// </exception>
        public static bool IsEmpty([NotNullWhen(false)] this Array array)
        {
            if (null == array)
            {
                throw new ArgumentNullException(nameof(array));
            }

            return array.Length <= 0;
        }

        /// <summary>
        /// Determines if the <see cref="Array"/> is <see langword="null"/>
        /// or contains no elements.
        /// </summary>
        /// <param name="array">The <see cref="Array"/> to check.</param>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="Array"/> is <see langword="null"/>
        ///     or contains 0 elements; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsNullOrEmpty([NotNullWhen(false)] this Array? array)
        {
            return null == array || array.Length <= 0;
        }
    }
}

