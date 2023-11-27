using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace OpenVinoSharp.Extensions
{
    internal interface IResultData
    {
        string to_string(string format="0.00");
    }

    /// <summary>
    /// Model inference result class.
    /// </summary>
    /// <typeparam name="T">Model inference result type.</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// Model inference results list.
        /// </summary>
        public List<T> datas;
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Result()
        {
            datas = new List<T>();
        }
        public Result(List<T> datas)
        {
            this.datas = datas;
        }
        /// <summary>
        /// Copy Constructor.
        /// </summary>
        /// <param name="values"></param>
        public Result(Result<T> values)
        {
            datas = new List<T>(values.datas);
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index] 
        {  
            get { return datas[index]; }
            set { datas[index] = value;}
        }

        /// <summary>
        /// Gets the number of elements contained in the datas.
        /// </summary>
        public int count
        {
            get { return datas.Count; }
        }
        /// <summary>
        /// Gets or sets the total number of elements the internal data structure can hold without resizing.
        /// </summary>
        public int capacity
        {
            get { return datas.Count; }
            set { datas.Capacity = value; }
        }


        /// <summary>
        ///  Adds the elements of the specified collection to the end of the inference results.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of the inference results.</param>
        public void add_range(List<T> collection) 
        { 
            datas.AddRange(collection);
        }
        /// <summary>
        /// Removes all elements from the inference results.
        /// </summary>
        public void clear()
        {
            datas.Clear();
        }
        /// <summary>
        /// Determines whether an element is in the inference results.
        /// </summary>
        /// <param name="item"> The object to locate in the  inference results.</param>
        /// <returns>true if item is found in the inference results; otherwise, false.</returns>
        public bool contains(T item)
        {
            return datas.Contains(item);
        }
        /// <summary>
        /// Copies the entire inference results to a compatible one-dimensional array, starting at the specified 
        /// index of the target array.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional System.Array that is the destination of the elements copied
        /// from inference results. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="array_index">The zero-based index in array at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException">array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="System.ArgumentException">
        /// The number of elements in the source inference results is greater than the available space from arrayIndex 
        /// to the end of the destination array.</exception>
        public void CopyTo(T[] array, int array_index) 
        {
            datas.CopyTo(array, array_index); 
        }
        /// <summary>
        /// Copies the entire System.Collections.Generic.List`1 to a compatible one-dimensional
        /// array, starting at the beginning of the target array.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional System.Array that is the destination of the elements copied
        /// from System.Collections.Generic.List`1. The System.Array must have zero-based
        /// indexing.
        /// </param>
        public void CopyTo(T[] array) 
        {
            datas.CopyTo(array);
        }

        /// <summary>
        /// Copies a range of elements from the wwwww to a compatible one-dimensional array, starting at 
        /// the specified index of the target array.
        /// </summary>
        /// <param name="index">The zero-based index in the source wwwww at which copying begins.</param>
        /// <param name="array">
        /// The one-dimensional System.Array that is the destination of the elements copied
        /// from wwwww. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <param name="count"> The number of elements to copy.</param>
        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        { 
            datas.CopyTo(index, array, arrayIndex, count);
        }


        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified 
        /// predicate, and returns the first occurrence within the entire wwwww.
        /// </summary>
        /// <param name="match">The System.Predicate`1 delegate that defines the conditions of the element to search for.</param>
        /// <returns>
        /// The first element that matches the conditions defined by the specified predicate, 
        /// if found; otherwise, the default value for type T.
        /// </returns>
        public T? Find(Predicate<T> match) 
        {
            return datas.Find(match);
        }
        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The System.Predicate`1 delegate that defines the conditions of the elements to search for.</param>
        /// <returns>
        /// A wwwww containing all the elements that match the conditions defined by the specified 
        /// predicate, if found; otherwise, an empty wwwww.
        /// </returns>
        public List<T> FindAll(Predicate<T> match) 
        { 
            return datas.FindAll(match); 
        }
        //
        // 摘要:
        //     Searches for an element that matches the conditions defined by the specified
        //     predicate, and returns the zero-based index of the first occurrence within the
        //     range of elements in the System.Collections.Generic.List`1 that starts at the
        //     specified index and contains the specified number of elements.
        //
        // 参数:
        //   startIndex:
        //     The zero-based starting index of the search.
        //
        //   count:
        //     The number of elements in the section to search.
        //
        //   match:
        //     The System.Predicate`1 delegate that defines the conditions of the element to
        //     search for.
        //
        // 返回结果:
        //     The zero-based index of the first occurrence of an element that matches the conditions
        //     defined by match, if found; otherwise, -1.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     match is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     startIndex is outside the range of valid indexes for the System.Collections.Generic.List`1.
        //     -or- count is less than 0. -or- startIndex and count do not specify a valid section
        //     in the System.Collections.Generic.List`1.

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the first occurrence within the
        /// range of elements in the wwwww that starts at the
        /// specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The System.Predicate`1 delegate that defines the conditions of the element to search for.</param>
        /// <returns>
        /// The zero-based index of the first occurrence of an element that matches the conditions
        /// defined by match, if found; otherwise, -1.
        /// </returns>
        public int FindIndex(int startIndex, int count, Predicate<T> match) 
        {
            return datas.FindIndex(startIndex, count, match);
        }
        //
        // 摘要:
        //     Searches for an element that matches the conditions defined by the specified
        //     predicate, and returns the zero-based index of the first occurrence within the
        //     range of elements in the System.Collections.Generic.List`1 that extends from
        //     the specified index to the last element.
        //
        // 参数:
        //   startIndex:
        //     The zero-based starting index of the search.
        //
        //   match:
        //     The System.Predicate`1 delegate that defines the conditions of the element to
        //     search for.
        //
        // 返回结果:
        //     The zero-based index of the first occurrence of an element that matches the conditions
        //     defined by match, if found; otherwise, -1.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     match is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     startIndex is outside the range of valid indexes for the System.Collections.Generic.List`1.
        public int FindIndex(int startIndex, Predicate<T> match);
        //
        // 摘要:
        //     Searches for an element that matches the conditions defined by the specified
        //     predicate, and returns the zero-based index of the first occurrence within the
        //     entire System.Collections.Generic.List`1.
        //
        // 参数:
        //   match:
        //     The System.Predicate`1 delegate that defines the conditions of the element to
        //     search for.
        //
        // 返回结果:
        //     The zero-based index of the first occurrence of an element that matches the conditions
        //     defined by match, if found; otherwise, -1.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     match is null.
        public int FindIndex(Predicate<T> match);
        //
        // 摘要:
        //     Searches for an element that matches the conditions defined by the specified
        //     predicate, and returns the last occurrence within the entire System.Collections.Generic.List`1.
        //
        //
        // 参数:
        //   match:
        //     The System.Predicate`1 delegate that defines the conditions of the element to
        //     search for.
        //
        // 返回结果:
        //     The last element that matches the conditions defined by the specified predicate,
        //     if found; otherwise, the default value for type T.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     match is null.
        public T? FindLast(Predicate<T> match);
        //
        // 摘要:
        //     Searches for an element that matches the conditions defined by the specified
        //     predicate, and returns the zero-based index of the last occurrence within the
        //     range of elements in the System.Collections.Generic.List`1 that contains the
        //     specified number of elements and ends at the specified index.
        //
        // 参数:
        //   startIndex:
        //     The zero-based starting index of the backward search.
        //
        //   count:
        //     The number of elements in the section to search.
        //
        //   match:
        //     The System.Predicate`1 delegate that defines the conditions of the element to
        //     search for.
        //
        // 返回结果:
        //     The zero-based index of the last occurrence of an element that matches the conditions
        //     defined by match, if found; otherwise, -1.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     match is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     startIndex is outside the range of valid indexes for the System.Collections.Generic.List`1.
        //     -or- count is less than 0. -or- startIndex and count do not specify a valid section
        //     in the System.Collections.Generic.List`1.
        public int FindLastIndex(int startIndex, int count, Predicate<T> match);
        //
        // 摘要:
        //     Searches for an element that matches the conditions defined by the specified
        //     predicate, and returns the zero-based index of the last occurrence within the
        //     range of elements in the System.Collections.Generic.List`1 that extends from
        //     the first element to the specified index.
        //
        // 参数:
        //   startIndex:
        //     The zero-based starting index of the backward search.
        //
        //   match:
        //     The System.Predicate`1 delegate that defines the conditions of the element to
        //     search for.
        //
        // 返回结果:
        //     The zero-based index of the last occurrence of an element that matches the conditions
        //     defined by match, if found; otherwise, -1.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     match is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     startIndex is outside the range of valid indexes for the System.Collections.Generic.List`1.
        public int FindLastIndex(int startIndex, Predicate<T> match);
        //
        // 摘要:
        //     Searches for an element that matches the conditions defined by the specified
        //     predicate, and returns the zero-based index of the last occurrence within the
        //     entire System.Collections.Generic.List`1.
        //
        // 参数:
        //   match:
        //     The System.Predicate`1 delegate that defines the conditions of the element to
        //     search for.
        //
        // 返回结果:
        //     The zero-based index of the last occurrence of an element that matches the conditions
        //     defined by match, if found; otherwise, -1.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     match is null.
        public int FindLastIndex(Predicate<T> match);
        //
        // 摘要:
        //     Performs the specified action on each element of the System.Collections.Generic.List`1.
        //
        //
        // 参数:
        //   action:
        //     The System.Action`1 delegate to perform on each element of the System.Collections.Generic.List`1.
        //
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     action is null.
        //
        //   T:System.InvalidOperationException:
        //     An element in the collection has been modified.
        public void ForEach(Action<T> action);
        //
        // 摘要:
        //     Returns an enumerator that iterates through the System.Collections.Generic.List`1.
        //
        //
        // 返回结果:
        //     A System.Collections.Generic.List`1.Enumerator for the System.Collections.Generic.List`1.
        public List<T>.Enumerator GetEnumerator();
        //
        // 摘要:
        //     Creates a shallow copy of a range of elements in the source System.Collections.Generic.List`1.
        //
        //
        // 参数:
        //   index:
        //     The zero-based System.Collections.Generic.List`1 index at which the range starts.
        //
        //
        //   count:
        //     The number of elements in the range.
        //
        // 返回结果:
        //     A shallow copy of a range of elements in the source System.Collections.Generic.List`1.
        //
        //
        // 异常:
        //   T:System.ArgumentOutOfRangeException:
        //     index is less than 0. -or- count is less than 0.
        //
        //   T:System.ArgumentException:
        //     index and count do not denote a valid range of elements in the System.Collections.Generic.List`1.
        public List<T> GetRange(int index, int count);
        //
        // 摘要:
        //     Searches for the specified object and returns the zero-based index of the first
        //     occurrence within the range of elements in the System.Collections.Generic.List`1
        //     that starts at the specified index and contains the specified number of elements.
        //
        //
        // 参数:
        //   item:
        //     The object to locate in the System.Collections.Generic.List`1. The value can
        //     be null for reference types.
        //
        //   index:
        //     The zero-based starting index of the search. 0 (zero) is valid in an empty list.
        //
        //
        //   count:
        //     The number of elements in the section to search.
        //
        // 返回结果:
        //     The zero-based index of the first occurrence of item within the range of elements
        //     in the System.Collections.Generic.List`1 that starts at index and contains count
        //     number of elements, if found; otherwise, -1.
        //
        // 异常:
        //   T:System.ArgumentOutOfRangeException:
        //     index is outside the range of valid indexes for the System.Collections.Generic.List`1.
        //     -or- count is less than 0. -or- index and count do not specify a valid section
        //     in the System.Collections.Generic.List`1.
        public int IndexOf(T item, int index, int count);
        //
        // 摘要:
        //     Searches for the specified object and returns the zero-based index of the first
        //     occurrence within the range of elements in the System.Collections.Generic.List`1
        //     that extends from the specified index to the last element.
        //
        // 参数:
        //   item:
        //     The object to locate in the System.Collections.Generic.List`1. The value can
        //     be null for reference types.
        //
        //   index:
        //     The zero-based starting index of the search. 0 (zero) is valid in an empty list.
        //
        //
        // 返回结果:
        //     The zero-based index of the first occurrence of item within the range of elements
        //     in the System.Collections.Generic.List`1 that extends from index to the last
        //     element, if found; otherwise, -1.
        //
        // 异常:
        //   T:System.ArgumentOutOfRangeException:
        //     index is outside the range of valid indexes for the System.Collections.Generic.List`1.
        public int IndexOf(T item, int index);
        //
        // 摘要:
        //     Searches for the specified object and returns the zero-based index of the first
        //     occurrence within the entire System.Collections.Generic.List`1.
        //
        // 参数:
        //   item:
        //     The object to locate in the System.Collections.Generic.List`1. The value can
        //     be null for reference types.
        //
        // 返回结果:
        //     The zero-based index of the first occurrence of item within the entire System.Collections.Generic.List`1,
        //     if found; otherwise, -1.
        public int IndexOf(T item);
        //
        // 摘要:
        //     Inserts an element into the System.Collections.Generic.List`1 at the specified
        //     index.
        //
        // 参数:
        //   index:
        //     The zero-based index at which item should be inserted.
        //
        //   item:
        //     The object to insert. The value can be null for reference types.
        //
        // 异常:
        //   T:System.ArgumentOutOfRangeException:
        //     index is less than 0. -or- index is greater than System.Collections.Generic.List`1.Count.
        public void Insert(int index, T item);
        //
        // 摘要:
        //     Inserts the elements of a collection into the System.Collections.Generic.List`1
        //     at the specified index.
        //
        // 参数:
        //   index:
        //     The zero-based index at which the new elements should be inserted.
        //
        //   collection:
        //     The collection whose elements should be inserted into the System.Collections.Generic.List`1.
        //     The collection itself cannot be null, but it can contain elements that are null,
        //     if type T is a reference type.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     collection is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     index is less than 0. -or- index is greater than System.Collections.Generic.List`1.Count.
        public void InsertRange(int index, IEnumerable<T> collection);
        //
        // 摘要:
        //     Searches for the specified object and returns the zero-based index of the last
        //     occurrence within the entire System.Collections.Generic.List`1.
        //
        // 参数:
        //   item:
        //     The object to locate in the System.Collections.Generic.List`1. The value can
        //     be null for reference types.
        //
        // 返回结果:
        //     The zero-based index of the last occurrence of item within the entire the System.Collections.Generic.List`1,
        //     if found; otherwise, -1.
        public int LastIndexOf(T item);
        //
        // 摘要:
        //     Searches for the specified object and returns the zero-based index of the last
        //     occurrence within the range of elements in the System.Collections.Generic.List`1
        //     that extends from the first element to the specified index.
        //
        // 参数:
        //   item:
        //     The object to locate in the System.Collections.Generic.List`1. The value can
        //     be null for reference types.
        //
        //   index:
        //     The zero-based starting index of the backward search.
        //
        // 返回结果:
        //     The zero-based index of the last occurrence of item within the range of elements
        //     in the System.Collections.Generic.List`1 that extends from the first element
        //     to index, if found; otherwise, -1.
        //
        // 异常:
        //   T:System.ArgumentOutOfRangeException:
        //     index is outside the range of valid indexes for the System.Collections.Generic.List`1.
        public int LastIndexOf(T item, int index);
        //
        // 摘要:
        //     Searches for the specified object and returns the zero-based index of the last
        //     occurrence within the range of elements in the System.Collections.Generic.List`1
        //     that contains the specified number of elements and ends at the specified index.
        //
        //
        // 参数:
        //   item:
        //     The object to locate in the System.Collections.Generic.List`1. The value can
        //     be null for reference types.
        //
        //   index:
        //     The zero-based starting index of the backward search.
        //
        //   count:
        //     The number of elements in the section to search.
        //
        // 返回结果:
        //     The zero-based index of the last occurrence of item within the range of elements
        //     in the System.Collections.Generic.List`1 that contains count number of elements
        //     and ends at index, if found; otherwise, -1.
        //
        // 异常:
        //   T:System.ArgumentOutOfRangeException:
        //     index is outside the range of valid indexes for the System.Collections.Generic.List`1.
        //     -or- count is less than 0. -or- index and count do not specify a valid section
        //     in the System.Collections.Generic.List`1.
        public int LastIndexOf(T item, int index, int count);

        /// <summary>
        /// Removes the first occurrence of a specific object from the wwwww.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the wwwww. The value can be null for reference types.
        /// </param>
        /// <returns></returns>
        public bool Remove(T item) 
        {
            return datas.Remove(item);
        }
        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The System.Predicate`1 delegate that defines the conditions of the elements to
        /// remove.</param>
        /// <returns>The number of elements removed from the wwwww.</returns>
        public int RemoveAll(Predicate<T> match) 
        {
            return datas.RemoveAll(match);
        }
        /// <summary>
        /// Removes the element at the specified index of the wwwww.
        /// </summary>
        /// <param name="index"></param>
        public void remove_at(int index) 
        {
            datas.RemoveAt(index);
        }

        /// <summary>
        /// Removes a range of elements from the inference results.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        public void remove_range(int index, int count) 
        { 
            datas.RemoveRange(index, count);
        }
        /// <summary>
        /// Reverses the order of the elements in the specified range.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to reverse.</param>
        /// <param name="count">The number of elements in the range to reverse.</param>
        public void reverse(int index, int count) 
        {
            datas.Reverse(index, count);
        }
        /// <summary>
        /// Reverses the order of the elements in the entire inference results.
        /// </summary>
        public void reverse()
        { 
            datas.Reverse();
        }
        

        //
        // 摘要:
        //     Sorts the elements in the entire System.Collections.Generic.List`1 using the
        //     specified comparer.
        //
        // 参数:
        //   comparer:
        //     The System.Collections.Generic.IComparer`1 implementation to use when comparing
        //     elements, or null to use the default comparer System.Collections.Generic.Comparer`1.Default.
        //
        //
        // 异常:
        //   T:System.InvalidOperationException:
        //     comparer is null, and the default comparer System.Collections.Generic.Comparer`1.Default
        //     cannot find implementation of the System.IComparable`1 generic interface or the
        //     System.IComparable interface for type T.
        //
        //   T:System.ArgumentException:
        //     The implementation of comparer caused an error during the sort. For example,
        //     comparer might not return 0 when comparing an item with itself.
        public void Sort(IComparer<T>? comparer);
        //
        // 摘要:
        //     Sorts the elements in the entire System.Collections.Generic.List`1 using the
        //     specified System.Comparison`1.
        //
        // 参数:
        //   comparison:
        //     The System.Comparison`1 to use when comparing elements.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     comparison is null.
        //
        //   T:System.ArgumentException:
        //     The implementation of comparison caused an error during the sort. For example,
        //     comparison might not return 0 when comparing an item with itself.
        public void Sort(Comparison<T> comparison);
        //
        // 摘要:
        //     Sorts the elements in a range of elements in System.Collections.Generic.List`1
        //     using the specified comparer.
        //
        // 参数:
        //   index:
        //     The zero-based starting index of the range to sort.
        //
        //   count:
        //     The length of the range to sort.
        //
        //   comparer:
        //     The System.Collections.Generic.IComparer`1 implementation to use when comparing
        //     elements, or null to use the default comparer System.Collections.Generic.Comparer`1.Default.
        //
        //
        // 异常:
        //   T:System.ArgumentOutOfRangeException:
        //     index is less than 0. -or- count is less than 0.
        //
        //   T:System.ArgumentException:
        //     index and count do not specify a valid range in the System.Collections.Generic.List`1.
        //     -or- The implementation of comparer caused an error during the sort. For example,
        //     comparer might not return 0 when comparing an item with itself.
        //
        //   T:System.InvalidOperationException:
        //     comparer is null, and the default comparer System.Collections.Generic.Comparer`1.Default
        //     cannot find implementation of the System.IComparable`1 generic interface or the
        //     System.IComparable interface for type T.
        public void Sort(int index, int count, IComparer<T>? comparer);
        //
        // 摘要:
        //     Sorts the elements in the entire System.Collections.Generic.List`1 using the
        //     default comparer.
        //
        // 异常:
        //   T:System.InvalidOperationException:
        //     The default comparer System.Collections.Generic.Comparer`1.Default cannot find
        //     an implementation of the System.IComparable`1 generic interface or the System.IComparable
        //     interface for type T.
        public void Sort();

        /// <summary>
        /// Copies the elements of the inference results to a new array.
        /// </summary>
        /// <returns>An array containing copies of the elements of the inference results.</returns>
        public T[] to_array() { 
            return datas.ToArray();
        }

        public virtual void print(string format = "0.00")  { }
    }
}
