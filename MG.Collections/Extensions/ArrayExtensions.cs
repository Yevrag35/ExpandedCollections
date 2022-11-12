using System;

namespace MG.Collections.Extensions.Arrays
{
    /// <summary>
    /// 
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Determines if the <see cref="Array"/> is empty (i.e. - count is equal to 0).
        /// </summary>
        /// <param name="array">The array to check.</param>
        /// <returns>
        ///     <see langword="true"/> if the 
        /// </returns>
        public static bool IsEmpty(this Array array)
        {
            return array.Length <= 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this Array? array)
        {
            return null == array || array.Length <= 0;
        }
    }
}

