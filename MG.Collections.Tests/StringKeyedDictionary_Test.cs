using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using MG.Collections.Exceptions;

namespace MG.Collections.Tests
{
    [TestClass]
    public class StringKeyedDictionary_Test
    {
        //private static readonly List<string> Greetings = new List<string>(
        //    new string[7] { "Bonjour", "Hola", "Hello", "Guten Tag", "Konnichiwa", "Salve", "Zdravstvuyte" }
        //);
        private static readonly StringKeyedDictionary<CultureInfo> Cultures = new StringKeyedDictionary<CultureInfo>(true, 7)
        {
            { "Bonjour", CultureInfo.GetCultureInfo("fr-fr") },
            { "Hola", CultureInfo.GetCultureInfo("es-es") },
            { "Hello", CultureInfo.GetCultureInfo("en-us") },
            { "Guten Tag", CultureInfo.GetCultureInfo("de-de") },
            { "Konnichiwa", CultureInfo.GetCultureInfo("ja-jp") },
            { "Salve", CultureInfo.GetCultureInfo("it-it") },
            { "Zdravstvuyte", CultureInfo.GetCultureInfo("ru-ru") }
        };

        [TestMethod]
        public void Test1()
        {
            Assert.AreEqual(7, Cultures.Count);
            Assert.IsTrue(Cultures.ContainsKey("helLo"));

            Assert.ThrowsException<ArgumentException>(() =>
            {
                Cultures.Add("bONJOUR", CultureInfo.GetCultureInfo("en-UK"));
            });

            Assert.IsFalse(Cultures.IsReadOnly);
        }

        [TestMethod]
        public void Test2()
        {
            var dict = StringKeyedDictionary<CultureInfo>.NewReadOnly(Cultures, false);
            Assert.IsTrue(dict.IsReadOnly);
            Assert.ThrowsException<ReadOnlyException>(() =>
            {
                dict.Add("whatev", CultureInfo.InvariantCulture);
            });
            Assert.ThrowsException<ReadOnlyException>(() =>
            {
                dict.Remove("salve");
            });
        }
    }
}
