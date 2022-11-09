using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MG.Collections.Extensions.ForEach.Ref
{
    /// <summary>
    /// An extension class allowing 'foreach' loops on <see cref="Range"/> and <see cref="int"/> structs.
    /// </summary>
    public static class ForEachExtensions
    {
        /// <summary>
        /// Creates a ref struct enumerator allowing for enumeration on <see cref="Range"/>
        /// structs.
        /// </summary>
        /// <param name="range">The range of indexes to enumerator through.</param>
        /// <returns>
        ///     An <see cref="IntRefEnumerator"/> struct.
        /// </returns>
        public static IntRefEnumerator GetEnumerator(this Range range)
        {
            return new IntRefEnumerator(range);
        }

        /// <summary>
        /// Creates a ref struct enumerator allowing for enumeration on <see cref="int"/>
        /// structs.
        /// </summary>
        /// <param name="exclusiveEnd">The exclusive end of the range of indicies to enumerate through.</param>
        /// <remarks>
        ///     The <paramref name="exclusiveEnd"/> can simply be the <see cref="ICollection.Count"/> or <see cref="Array.Length"/> property of a collection or array.
        ///     Also, as <see cref="IntRefEnumerator"/> is a ref struct, it *cannot* be 
        ///     used directly within an <see langword="async"/> method.
        /// </remarks>
        /// <returns>
        ///     An <see cref="IntRefEnumerator"/> struct.
        /// </returns>
        public static IntRefEnumerator GetEnumerator(this int exclusiveEnd)
        {
            return GetEnumerator(new Range(0, exclusiveEnd));
        }
    }

    /// <summary>
    /// An enumerator that supports enumerating through a selected <see cref="Range"/>.
    /// </summary>
    /// <remarks>
    ///     This enumerator was inspired entirely by Nick Chapsas - <see href="https://youtu.be/jmmz1cInNow"/>.
    ///     
    ///     Link to his website - <see href="https://nickchapsas.com"/>.
    /// </remarks>
    public ref struct IntRefEnumerator
    {
        private int _current;
        private readonly int _end;

        /// <summary>
        /// Gets the current index representing the current position of the 
        /// <see cref="IntRefEnumerator"/>.
        /// </summary>
        public int Current => _current;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <exception cref="NotSupportedException"><see cref="Index.IsFromEnd"/> is 
        /// <see langword="true"/>.
        /// </exception>
        public IntRefEnumerator(Range range)
        {
            if (range.End.IsFromEnd)
            {
                throw new NotSupportedException("Range must have a finite end.");
            }

            _current = range.Start.Value - 1;
            _end = range.End.Value;
        }

        /// <summary>
        /// Advances the <see cref="IntRefEnumerator"/> to the next index.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> if the advanced index counter is still less than
        ///     the configured end of the <see cref="Range"/>; otherwise, 
        ///     <see langword="false"/>.
        /// </returns>
        public bool MoveNext()
        {
            _current++;
            return _current < _end;
        }
    }
}