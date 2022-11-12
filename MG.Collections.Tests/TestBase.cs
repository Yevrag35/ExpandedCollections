using System.Collections.Generic;
using System.Security.Cryptography;

namespace MG.Collections.Tests
{
    public abstract class TestBase
    {
        public static string[] ReorderDataRandomly(string[] data)
        {
            string[] newArray = new string[data.Length];
            var set = new HashSet<int>();
            for (int i = 0; i < data.Length; i++)
            {
                int indexToInsert = GetRandomInt(data.Length, set);
                newArray[i] = data[indexToInsert];
            }

            return newArray;
        }
        public static int GetRandomInt(int upperBound = 1001)
        {
            return RandomNumberGenerator.GetInt32(upperBound);
        }
        private static int GetRandomInt(int totalCount, HashSet<int> cantBe)
        {
            int newNum = -1;
            do
            {
                newNum = GetRandomInt(totalCount);
            }
            while (cantBe.Contains(newNum));

            cantBe.Add(newNum);

            return newNum;
        }
    }
}
