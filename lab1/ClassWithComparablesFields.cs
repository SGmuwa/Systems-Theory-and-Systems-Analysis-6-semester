using System;
using System.Collections.Generic;
using System.Collections;

namespace lab1
{
    public class ClassWithComparablesFields
    {
        public readonly List<ParserWithIComparable> comparables;

        public ClassWithComparablesFields(params ParserWithIComparable[] comparables)
        {
            this.comparables = new List<ParserWithIComparable>(comparables);
        }
    }
    public struct ParserWithIComparable
    {
        public ParserWithIComparable(Func<string, object> Parser, IComparable Comparable, string Name)
        {
            this.Parser = Parser;
            this.Comparable = Comparable;
            this.Name = Name;
        }

        public string Name;
        public Func<string, object> Parser;
        public IComparable Comparable;
    }
}
