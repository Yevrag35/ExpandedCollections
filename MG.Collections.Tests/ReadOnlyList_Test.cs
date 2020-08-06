using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MG.Collections.Tests
{
    [TestClass]
    public class ReadOnlyList_Test
    {
        private static readonly List<string> Greetings = new List<string>(
            new string[7] { "Bonjour", "Hola", "Hello", "Guten Tag", "Konnichiwa", "Salve", "Zdravstvuyte" }
        );

        [TestMethod]
        public void Casts()
        {
            var col = new ReadOnlyCollection<string>(Greetings);

            var list1 = (ReadOnlyList<string>)col;
            Assert.AreEqual(Greetings.Count, list1.Count);

            var backToCol = (ReadOnlyCollection<string>)list1;

            var toList = (List<string>)list1;
            Assert.AreEqual(Greetings.Count, toList.Count);

            var andBack = (ReadOnlyList<string>)toList;
            Assert.AreEqual(Greetings.Count, andBack.Count);
        }

        [TestMethod]
        public void Indexers()
        {
            var list = new ReadOnlyList<string>(Greetings);
            Assert.AreEqual(Greetings.Count, list.Count);
            for (int i = 0; i < Greetings.Count; i++)
            {
                Assert.AreEqual(Greetings[i], list[i]);
            }
        }

        [TestMethod]
        public void TestReverse()
        {
            var list = new ReadOnlyList<string>(Greetings);

            list.Reverse(2);
            Assert.AreEqual("Zdravstvuyte", list[2]);
            Assert.AreEqual("Hello", list[6]);
        }
    }
}
