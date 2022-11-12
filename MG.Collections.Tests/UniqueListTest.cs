using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MG.Collections.Tests
{
    public class UniqueListTest
    {
        public static IEnumerable<object[]> GetGuids(int howMany = 1)
        {
            var guids = new object[1][]
            {
                new object[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() }
            };

            return guids.Select(x => x.Take(howMany).ToArray());
        }

        [Fact]
        public void TestUniqueComparer()
        {
            IEqualityComparer<string> equalityComparer = StringComparer.InvariantCultureIgnoreCase;
            var col1 = new UniqueList<string>();
            var col2 = new UniqueList<DictionaryEntry>();
            var col3 = new UniqueList<string>(equalityComparer);

            Assert.NotNull(col1.Comparer);
            Assert.NotNull(col2.Comparer);
            Assert.NotNull(col3.Comparer);

            Assert.Equal(StringComparer.CurrentCulture, col1.Comparer);
            Assert.Equal(EqualityComparer<DictionaryEntry>.Default, col2.Comparer);
            Assert.Equal(equalityComparer, col3.Comparer);
        }

        [Theory]
        [MemberData(nameof(GetGuids), 2)]
        public void TestUniqueAdd(Guid guid1, Guid guid2)
        {
            var col = new UniqueList<Guid>(2);
            Assert.Empty(col);

            col.Add(guid1);
            Assert.Single(col);

            col.Add(guid1);
            Assert.Single(col);

            col.Add(guid2);
            Assert.Equal(2, col.Count);
            Assert.Single(col, guid1);
            Assert.Single(col, guid2);
        }

        [Theory]
        [MemberData(nameof(GetGuids), 2)]
        public void TestUniqueRemove(Guid guid1, Guid guid2)
        {
            var col = new UniqueList<Guid>(new Guid[] { guid1, guid2 });
            Assert.Equal(2, col.Count);

            Assert.True(col.Remove(guid1));
            Assert.Single(col);

            Assert.False(col.Remove(guid1));
            Assert.Single(col);
        }

        [Theory]
        [MemberData(nameof(GetGuids), 3)]
        public void TestSetAccessor(Guid guid1, Guid guid2, Guid guid3)
        {
            var col = new UniqueList<Guid>(new Guid[] { guid1, guid2 });

            col[0] = guid3;
            Assert.Equal(2, col.Count);
            Assert.Equal(guid3, col[0]);

            Assert.Equal(guid3, col[0]);

            col[1] = guid3;
            Assert.Equal(guid2, col[1]);
        }
    }
}
