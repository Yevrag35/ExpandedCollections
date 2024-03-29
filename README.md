# <img src="assets/Collections_Red.png" width="40" height="40"> Expanded Specialized Collections

[![version](https://img.shields.io/nuget/v/MG.Collections?style=flat-square)](https://www.nuget.org/packages/MG.Collections) [![downloads](https://img.shields.io/nuget/dt/MG.Collections?style=flat-square&color=darkgreen)](https://www.nuget.org/packages/MG.Collections) [![Codacy Badge](https://app.codacy.com/project/badge/Grade/49c8278baa0a4435967f0b9317ae374a)](https://www.codacy.com/gh/Yevrag35/ExpandedCollections/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Yevrag35/ExpandedCollections&amp;utm_campaign=Badge_Grade)

Helper libraries that provide some collection classes that I use in my projects.  Some examples include:

## __UniqueList\<T\>__

* _Same as <code>System.Collections.Generic.List\<T\></code> but ensures that each element is unique according to a custom <code>IEqualityComparer\<T\></code> or the default._

```csharp

// input is: "test test Test"
using MG.Collections;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var col = new UniqueList<string>(args);

            Console.WriteLine(col.Count); // outputs '1'
            Console.WriteLine(col[0]);    // outputs 'test'.
        }
    }
}
```

## __ManagedKeySortedList\<TKey, TValue\>__

* Similar to <code>System.Collections.Generic.SortedList\<TKey, TValue\></code>.  The difference is that the 'key' is retrieved from an incoming object using a specified <code>Func\<TValue, TKey\></code> function defined in the constructor.  Elements can be added then, and the 'key' will be retrieved automatically from it.

* The other difference is that the __default__ enumerator for this collection uses the simple List enumerator instead of a Dictionary one.

```csharp
// input is: 1 2 87 2 "what up"
using MG.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var sortedList = new ManagedKeySortedList<int, StringInt>(x => x.IntValue);

            Array.ForEach((a) => {
                sortedList.Add(a);
            });

            Console.WriteLine(sortedList.Count); // outputs '4'
        }
    }

    struct StringInt
    {
        public int IntValue;
        public string StrValue;

        public static implicit operator StringInt(string parsingStr)
        {
            int intVal = -1;
            if (int.TryParse(parsingStr, out int tryInt))
            {
                intVal = tryInt;
            }

            return new StringInt
            {
                IntValue = intVal,
                StrValue = parsingStr
            };
        }
    }
}
```

## __ReadOnlyList\<T\> & ReadOnlySet\<T\>__ 

* A List and a Set readonly wrapper (similiar to <code>System.Collections.ObjectModel.ReadOnlyCollection\<T\></code>) exposing only the 'readonly' methods of the corresponding collection types.

    * _\*NOTE\*_ - In .NET5 projects, <code>ReadOnlySet\<T\></code> implements <code>System.Collections.Generic.IReadOnlySet\<T\></code>.  All other target frameworks, it implements a custom <code>MG.Collections.IReadOnly\<T\></code>. 
    
## __ForEach Extensions in .NET 5+__ ##
```csharp

using MG.Collections.Extensions.ForEach.Ref;

var greetings = new List<string>() { "hi", "hello", "what up" };
var newArray = new string[greetings.Count];

foreach (int i in greetings.Count) {
    newArray[i] = greetings[i];
}
```
