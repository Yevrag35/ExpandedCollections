using MG.Collections.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using MG.Collections;
using MG.Collections.Mocks;
using Xunit;
using Moq;
using MG.Collections.Exceptions;

namespace MG.Collections.Tests
{
    public class ManagedKeyTest
    {
        private static IStruct GetMock(int num = 24)
        {
            var guid = Guid.NewGuid();
            string str = "Just a String";

            var mock = new Mock<IStruct>();
            mock.SetupGet(x => x.Id).Returns(num);
            mock.SetupGet(x => x.UniqueId).Returns(guid);
            mock.SetupGet(x => x.Value).Returns(str);

            return mock.Object;
        }

        [Fact]
        public void AddTest()
        {
            IStruct mock = GetMock();

            var col = new ManagedKeySortedList<int, IStruct>(x => x.Id);
            Assert.Empty(col);

            col.Add(mock);

            Assert.True(col.Contains(mock));
            Assert.True(col.ContainsKey(24));

            Assert.Throws<KeyAlreadyExistsException>(() => col.Add(mock));
            Assert.Single(col);

            Assert.Throws<InvalidOperationException>(() => col.Add(null));
        }

        [Fact]
        public void TryGetValueTest()
        {
            IStruct mock = GetMock();

            var col = new ManagedKeySortedList<string, IStruct>(x => x.Value);
            Assert.Empty(col);

            col.Add(mock);

            bool result = col.TryGetValue(mock.Value, out IStruct outVal);
            Assert.True(result);
            Assert.Equal(mock, outVal);
        }

        [Fact]
        public void ClearTest()
        {
            IStruct mock = GetMock();
            var col = new ManagedKeySortedList<Guid, IStruct>(x => x.UniqueId);
            col.Add(mock);
            Assert.Single(col);

            col.Clear();
            Assert.Empty(col);
        }

        [Theory]
        [InlineData(0, 2)]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        public void GetRangeTest(int index, int count)
        {
            IStruct[] structs = new IStruct[3]
            {
                GetMock(),
                GetMock(360),
                GetMock(720)
            };
            var col = new ManagedKeySortedList<int, IStruct>(structs, x => x.Id);
            Assert.Equal(3, col.Count);

            var list = col.GetRange(index, count);

            Assert.Equal(count, list.Count);

            for (int i = 0; i < list.Count; i++)
            {
                IStruct colStr = col[i + index];
                IStruct listStr = list[i];

                Assert.Equal(colStr, listStr);
            }
        }

        [Fact]
        public void RemoveTest()
        {
            IStruct mock = GetMock();
            var col = new ManagedKeySortedList<string, IStruct>(x => x.Value)
            {
                mock
            };

            Assert.Single(col);

            Assert.True(col.Remove(mock.Value));
            Assert.Empty(col);

            col.Add(mock);
            Assert.Single(col);

            Assert.Throws<ArgumentNullException>(() => col.Remove(null));
            Assert.Single(col);

            Assert.Throws<InvalidOperationException>(() => col.RemoveValue(null));
            Assert.Single(col);
        }

        [Fact]
        public void IndexTest()
        {
            IStruct mock = GetMock();
            var col = new ManagedKeySortedList<string, IStruct>(x => x.Value);
            col.Add(mock);

            Assert.Equal(mock, col[0]);
            Assert.Equal(mock, col[mock.Value]);
            //Assert.Throws<NotSupportedException>(() => col[0] = mock);
        }
    }
}
