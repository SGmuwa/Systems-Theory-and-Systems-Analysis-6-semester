using System;
using System.Collections.Generic;
using System.Collections;

namespace lab1
{
    public static class SorterHelper
    {

    }


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
        public ParserWithIComparable(TryParser TryParser, Func<IComparable> GetValue, string NameField)
        {
            this.Parser = (string input, out object output) => { output = null; return int.Parse(input, output)};
            this.GetValue = GetValue;
            this.NameField = NameField;
        }

        public string NameField { get; }
        public TryParser Parser { get; }
        public Func<IComparable> GetValue { get; }

        public delegate bool TryParser<out T>(string input, out T output);
    }
}
