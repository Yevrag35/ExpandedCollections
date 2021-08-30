using System;
using System.Collections.Generic;

#pragma warning disable IDE0130

namespace MG.Collections
{
    /// <summary>
    /// Represents a collection that can be searched through and exposes advanced searching methods
    /// to accommodate this.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISearchableList<T> : IReadOnlyList<T>
    {
        /// <summary>
        /// Determines whether an element is in the <see cref="ISearchableList{T}"/>.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="ISearchableList{T}"/>.  The value can be null for reference types.
        /// </param>
        bool Contains(T item);
        /// <summary>
        ///     Determines whether the <see cref="ISearchableList{T}"/> contains elements that
        ///     match the conditions defined by the specified condition.
        /// </summary>
        /// <param name="match">
        ///     The <see cref="Func{T, TResult}"/> delegate that defines the conditions of the 
        ///     elements to search for.
        /// </param>
        /// <returns>
        /// <see langword="true"/>:
        ///     if the <see cref="ISearchableList{T}"/> contains one or more elements that
        ///     <paramref name="match"/> defined.
        /// <see langword="false"/>:
        ///     otherwise.
        /// </returns>
        bool Exists(Func<T, bool> match);
        /// <summary>
        ///     Searches for an element that matches the conditions defined by the specified
        ///     condition, and returns the first occurrence within the entire <see cref="ISearchableList{T}"/>.
        /// </summary>
        /// <param name="match">
        ///     The <see cref="Func{T, TResult}"/> delegate that defines the conditions of the
        ///     elements to search for.
        /// </param>
        T Find(Func<T, bool> match);
        /// <summary>
        /// Retrieves all of the elements that match the conditions defined by the specified condition.
        /// </summary>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate the defines the conditions of the elements to search for.</param>
        IList<T> FindAll(Func<T, bool> match);
        /// <summary>
        /// Searches for an element that match the conditions defined by the specified condition, and returns the zero-based
        /// index of the first occurrence within the entire <see cref="ISearchableList{T}"/>.
        /// </summary>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate the defines the conditions of the elements to search for.</param>
        int FindIndex(Func<T, bool> match);
        /// <summary>
        /// Searches for an element that match the conditions defined by the specified condition, and returns the zero-based
        /// index of the first occurrence within the range of elements in the <see cref="ISearchableList{T}"/> that extends from the 
        /// specified index to the last element.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate the defines the conditions of the elements to search for.</param>
        int FindIndex(int startIndex, Func<T, bool> match);
        /// <summary>
        /// Searches for an element that match the conditions defined by the specified condition, and returns the zero-based
        /// index of the first occurrence within the range of elements in the <see cref="ISearchableList{T}"/> that starts at the 
        /// specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate the defines the conditions of the elements to search for.</param>
        int FindIndex(int startIndex, int count, Func<T, bool> match);
        /// <summary>
        /// Searches for an elements that matches the conditions defined by the specified condition, and returns the last occurrence within the
        /// entire <see cref="ISearchableList{T}"/>.
        /// </summary>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate the defines the conditions of the elements to search for.</param>
        T FindLast(Func<T, bool> match);
        /// <summary>
        /// Searches for an elements that matches the conditions defined by the specified condition, and returns the zero-based index of the
        /// lat occurrence within the entire <see cref="ISearchableList{T}"/>.
        /// </summary>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate the defines the conditions of the elements to search for.</param>
        int FindLastIndex(Func<T, bool> match);
        /// <summary>
        /// Searches for an elements that matches the conditions defined by the specified condition, and returns the zero-based index of the
        /// lat occurrence within the range of elements in the <see cref="ISearchableList{T}"/> that extends from the first element to the 
        /// specified index.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the backward search.</param>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate the defines the conditions of the elements to search for.</param>
        int FindLastIndex(int startIndex, Func<T, bool> match);
        /// <summary>
        /// Searches for an elements that matches the conditions defined by the specified condition, and returns the zero-based index of the
        /// lat occurrence within the range of elements in the <see cref="ISearchableList{T}"/> that contains the specified number
        /// of elements and ends at the specified index.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the backward search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate the defines the conditions of the elements to search for.</param>
        int FindLastIndex(int startIndex, int count, Func<T, bool> match);
        /// <summary>
        /// Creates a shallow copy of a range of elements in the source <see cref="ISearchableList{T}"/>.
        /// </summary>
        /// <param name="index">The zero-based index at which the range starts.</param>
        /// <param name="count">The number of elements in the range.</param>
        IList<T> GetRange(int index, int count);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence
        /// within the entire <see cref="ISearchableList{T}"/>.
        /// </summary>
        /// <param name="item">
        ///     The object to locate in the <see cref="ISearchableList{T}"/>.  
        ///     The value can be <see langword="null"/> for reference types.
        /// </param>
        /// /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence
        /// within the range of elements in the <see cref="ISearchableList{T}"/> that extends from the specified index to the last element.
        /// </summary>
        /// <param name="item">
        ///     The object to locate in the <see cref="ISearchableList{T}"/>.  
        ///     The value can be <see langword="null"/> for reference types.
        /// </param>
        /// <param name="index">The zero-based starting index of the search.  0 (zero) is valid in an empty list.</param>
        int IndexOf(T item, int index);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence
        /// within the range of elements in the <see cref="ISearchableList{T}"/> that extends from the specified index to the last element.
        /// </summary>
        /// <param name="item">
        ///     The object to locate in the <see cref="ISearchableList{T}"/>.  
        ///     The value can be <see langword="null"/> for reference types.
        /// </param>
        /// <param name="index">The zero-based starting index of the search.  0 (zero) is valid in an empty list.</param>
        int IndexOf(T item, int index, int count);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence
        /// within the entire <see cref="ISearchableList{T}"/>.
        /// </summary>
        /// <param name="item">
        ///     The object to locate in the <see cref="ISearchableList{T}"/>.  
        ///     The value can be <see langword="null"/> for reference types.
        /// </param>
        int LastIndexOf(T item);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence
        /// within the range of elements in the <see cref="ISearchableList{T}"/> that extends from the first element to the specified index.
        /// </summary>
        /// <param name="item">
        ///     The object to locate in the <see cref="ISearchableList{T}"/>.  
        ///     The value can be <see langword="null"/> for reference types.
        /// </param>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        int LastIndexOf(T item, int index);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence
        /// within the range of elements in the <see cref="ISearchableList{T}"/> that contains the specified number
        /// of elements and ends at the specified index.
        /// </summary>
        /// <param name="item">
        ///     The object to locate in the <see cref="ISearchableList{T}"/>.  
        ///     The value can be <see langword="null"/> for reference types.
        /// </param>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        int LastIndexOf(T item, int index, int count);
        /// <summary>
        /// Determines whether every element in the <see cref="ISearchableList{T}"/> matches the conditions
        /// defined by the specified condition.
        /// </summary>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate that defines the conditions to check against the elements.</param>
        bool TrueForAll(Func<T, bool> match);
    }
}
