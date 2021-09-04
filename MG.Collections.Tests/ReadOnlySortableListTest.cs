using MG.Collections.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using MG.Collections;
using Xunit;

namespace MG.Collections.Tests
{
    public class ReadOnlySortableListTest : TestBase
    {
        public static readonly object[][] Greetings = new object[][] {
            ReorderDataRandomly(new string[7] { "Bonjour", "Hola", "Hello", "Guten Tag", "Konnichiwa", "Salve", "Zdravstvuyte" })
        };
        public static readonly object[][] GreetingsArray = new object[][]
        {
            Greetings
        };

        [Fact]
        public void DefaultComparer()
        {
            var strList = new ReadOnlySortableList<string>(new string[] { "a", "b", "c" });
            Assert.Equal(3, strList.Count);
            Assert.NotNull(strList.DefaultComparer);
            Assert.IsAssignableFrom<StringComparer>(strList.DefaultComparer);

            var intList = new ReadOnlySortableList<int>(new int[] { 1, 2, 3 });
            Assert.NotNull(intList.DefaultComparer);
            Assert.IsAssignableFrom<Comparer<int>>(intList.DefaultComparer);
        }

        [Theory]
        [MemberData(nameof(GreetingsArray))]
        public void TestReverse(string[] testData)
        {
            //string[] scrambled = ReorderDataRandomly(testData);
            var preReversed = testData.Reverse().ToArray();

            var list = new ReadOnlySortableList<string>(testData);

            list.Reverse();
            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(preReversed[i], list[i]);
            }

            // ... and back.
            list.Reverse();
            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(testData[i], list[i]);
            }
        }

        [Theory]
        [MemberData(nameof(GreetingsArray))]
        public void Sort(string[] testData)
        {
            IComparer<string> comparer = ReverseStringSort.Normal;
            IComparer<string> reverse = new ReverseStringSort();

            var preSorted = testData.OrderBy(x => x, comparer).ToArray();
            var preReversed = testData.OrderBy(x => x, reverse).ToArray();
            var list = new ReadOnlySortableList<string>(testData, comparer);

            list.Sort();

            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(preSorted[i], list[i]);
            }

            // and reverse it
            list.Sort(reverse);

            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(preReversed[i], list[i]);
            }
        }

        //[Theory]
        //[MemberData(nameof(GreetingsArray))]
        //public void SortIndex(string[] testData)
        //{
        //    int count = testData.Length;
        //    string[] scrambled = ReorderDataRandomly(testData);

        //    for (int i = 0; i < count; i++)
        //    {
        //        var list = new ReadOnlySortableList<string>(testData, ReverseStringSort.Normal);

        //        list.Sort(i);

        //    }
        //}
    }

    internal class ReverseStringSort : IComparer<string>
    {
        internal static readonly IComparer<string> Normal = StringComparer.CurrentCultureIgnoreCase;

        public int Compare(string x, string y)
        {
            return Normal.Compare(x, y) * -1;
        }
    }
}
