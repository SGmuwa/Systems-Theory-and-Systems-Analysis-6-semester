using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;

namespace lab1
{
    public class ParserWithIComparable<T> where T : IComparable<T>
    {
        public ParserWithIComparable(string NameField, T Value = default(T))
        {
            this.NameField = NameField;
            this.Value = Value;
        }

        public string NameField { get; }
        public T Value { get; set; }
        public MethodInfo MethodTryParse = typeof(T).GetMethod("TryParse", BindingFlags.Static);

        private bool ParseSet(string str) 
        {
            T output = default(T);
            object[] args = new object[] { str, output };
            bool ret = (bool)MethodTryParse.Invoke(null, args);
            if(ret)
                Value = (T)args[1];
            return ret;
        }

        public override string ToString()
            => Value?.ToString();
    }
}
