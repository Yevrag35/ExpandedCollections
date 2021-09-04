using MG.Collections.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MG.Collections;
using Xunit;

namespace MG.Collections.Tests
{
    public class ReadOnlyListTest
    {
        public static readonly object[][] Greetings = new object[][] {
            new string[7] { "Bonjour", "Hola", "Hello", "Guten Tag", "Konnichiwa", "Salve", "Zdravstvuyte" }
        };
        public static readonly object[][] GreetingsArray = new object[][]
        {
            Greetings
        };

        [Theory]
        [MemberData(nameof(GreetingsArray))]
        public void TestCasts(string[] testData)
        {
            var GreetingsCol = new ReadOnlyCollection<string>(testData);
            var readOnlyList = (ReadOnlyList<string>)GreetingsCol;

            Assert.NotNull(readOnlyList);
            Assert.Equal(Greetings[0].Length, readOnlyList.Count);

            // And back...
            var readOnlyCol = (ReadOnlyCollection<string>)readOnlyList;
            Assert.NotNull(readOnlyCol);
            Assert.Equal(Greetings[0].Length, readOnlyCol.Count);
        }

        [Theory]
        [MemberData(nameof(GreetingsArray))]
        public void TestIndexers(string[] testData)
        {
            var list = new ReadOnlyList<string>(testData);
            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(testData[i], list[i]);
            }
        }
    }
}

//    public class OrderedListTest
//    {
//        private const string ERROR_MSG_1 = @"
//The original ({0}) must be a lesser negative number than the calculated index ({1}).";

//        private const string ERROR_MSG_2 = @"
//The calculated index ({0}) must be, by definition, 
//greater than or equal to {1} (the sum of: {2} + {3}).";
//        private const string ERROR_MSG_3 = @"
//The calculated index ({0}) must be, by definition, also less than the array's total count ({1}).";

//        public static int ConvertToInt(object o) => (int)o;
//        public class ReverseNumberComparer : IComparer<int>
//        {
//            public int Compare(int x, int y)
//            {
//                return x.CompareTo(y) * -1;
//            }
//        }

//        public static readonly List<object[]> TheList = new List<object[]>(3)
//        {
//            new object[] { new int[] { 1, 2, 3, 6, 7, 8, 10 } },
//            new object[] { new int[] { 10, 8, 6, 2, 5, 7, 680, 4 } },
//            new object[] { new int[] { 1, 2, 34, 2, 2, 2, 7, 8, 1, 34 } }
//        };

//        public static IEnumerable<object[]> GetNumbersByIndex(int indexOfTest)
//        {
//            List<object[]> list = TheList;
//            int positiveIndex = IndexHelper.GetPositiveIndex(indexOfTest, list.Count);

//            if (indexOfTest > -1)
//            {
//                yield return list.ElementAt(indexOfTest);
//            }
//        }

//        [Theory]
//        //[InlineData(1, 4, 6, -3, -6, int.MaxValue, int.MinValue)]
//        [InlineData(1)]
//        [InlineData(4)]
//        [InlineData(6)]
//        [InlineData(0)]
//        [InlineData(-1)]
//        [InlineData(-3)]
//        [InlineData(-6)]
//        [InlineData(int.MaxValue)]
//        [InlineData(int.MinValue)]
//        public void TestPositiveIndex(int index)
//        {
//            int[][] numArrays = new int[][] { (int[])TheList[0][0], (int[])TheList[2][0] };

//            foreach (int[] numbers in numArrays)
//            {
//                int positiveIndex = IndexHelper.GetPositiveIndex(index, numbers.Length);
//                if (index > -1)
//                {
//                    Assert.Equal(index, positiveIndex);
//                    if (index < numbers.Length)
//                        Assert.Equal(numbers[index], numbers[positiveIndex]);
//                }
//                else
//                {
//                    Assert.NotEqual(index, positiveIndex);

//                    Assert.True(index < positiveIndex,
//                        string.Format(ERROR_MSG_1,
//                        index, positiveIndex));

//                    Assert.True(positiveIndex >= int.MinValue + numbers.Length,
//                        string.Format(
//                            ERROR_MSG_2,
//                            positiveIndex,
//                            int.MinValue + numbers.Length,
//                            int.MinValue,
//                            numbers.Length));

//                    Assert.True(positiveIndex < numbers.Length);

//                    //Assert.InRange(positiveIndex, int.MinValue + numbers.Length, numbers.Length - 1);
//                }
//            }
//        }

//        //[Theory]

//        //public void TestInsert(object s1, object s2, object s3)
//        //{
//        //    if (s1 is string)
//        //    {
//        //        var ordered = new OrderedList<string>(3);
//        //        int index1 = ordered.Add((string)s1);
//        //        Assert.Equal(0, index1);
//        //        Assert.Equal(s1, Assert.Single(ordered));

//        //        int index2 = ordered.Add((string)s2);
//        //        Assert.Equal(1, index2);
//        //        Assert.Equal(2, ordered.Count);

//        //        int index3 = ordered.Add((string)s3);
//        //        Assert.Equal(0, index3);
//        //        Assert.Equal(3, ordered.Count);
//        //    }
//        //    else
//        //    {
//        //        var ordered = new OrderedList<Guid>(3);
//        //        int index1 = ordered.Add((Guid)s1);
//        //        Assert.Equal(0, index1);
//        //        Assert.Equal(s1, Assert.Single(ordered));

//        //        int index2 = ordered.Add((Guid)s2);
//        //        Assert.Equal(1, index2);
//        //        Assert.Equal(2, ordered.Count);

//        //        int index3 = ordered.Add((Guid)s3);
//        //        Assert.Equal(0, index3);
//        //        Assert.Equal(3, ordered.Count);
//        //    }
            
//        //}
//    }
//}
