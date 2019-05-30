﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace lab1
{
    public static class ParserWithIComparable
    {
        public static List<object> VisualSelect(IList<object> fromSelect)
        {
            Type type = SelectConsole(fromSelect, "Какой тип вас интересует?", (object o) => o.GetType());
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo field = SelectFromList(fields, "По какому полю вы хотите сделать поиск?");
            object min = GetValueFromConsole("Минимальное значение поля.", field.FieldType);
            object max = GetValueFromConsole("Максимальное значение поля.", field.FieldType);
            List<object> sortedList = new List<object>();
            Comparer comparer = new Comparer(type, field);
            foreach(object o in fromSelect)
            {
                if(o.GetType().Equals(type) && comparer.Compare(min, o) <= 0 && comparer.Compare(o, max) <= 0)
                {
                    sortedList.Add(o);
                }
            }
            sortedList.Sort(comparer);
            Console.WriteLine("Результат:");
            sortedList.WriteAllLine(true);
            return sortedList;
        }

        class Comparer : IComparer, IComparer<object>
        {
            public Comparer(Type @class, FieldInfo field, bool isReverse = false)
            {
                this.@class = @class;
                this.field = field;
                this.isReverse = isReverse;
            }

            public Type @class;
            public FieldInfo field;
            public bool isReverse;

            public int Compare(object x, object y)
            {
                if (x == null || y == null)
                    throw new ArgumentNullException();
                if (x.GetType().Equals(@class) && y.GetType().Equals(@class))
                {
                    return (isReverse ? -1 : 1) * ((IComparable)field.GetValue(x)).CompareTo(field.GetValue(y));
                }
                else
                    throw new NotSupportedException($"{x}, {y}, {@class}");
            }
        }

        private static T SelectConsole<T>(IList<object> from, string msg, Func<object, T> GetInList)
        {
            // Поиск уникальных типов.
            HashSet<T> typesSet = new HashSet<T>();
            foreach (object o in from)
                typesSet.Add(GetInList(o));

            List<T> types = new List<T>(typesSet);
            return SelectFromList(types, msg);
        }

        private static T SelectFromList<T>(IList<T> list, string msg)
        {
            Console.WriteLine("Список:");
            list.WriteAllLine(true);
            if (list.Count == 0)
                return default(T);
            int i;
            do
            {
                i = GetValueFromConsole<int>($"Разрешён диапазон с {0} до {list.Count - 1} " + msg);
            } while (i < 0 || i >= list.Count);
            return list[i];
        }

        private static object GetValueFromConsole(string msg, Type type)
        {
            //typeof(ParserWithIComparable).GetMethods().Single((m) => m.IsGenericMethod && m.IsPrivate && m.Name.Equals("GetValueFromConsole"));
            MethodInfo method = (from m in typeof(ParserWithIComparable).GetMethods() where 
             m.IsGenericMethod && m.IsPrivate && m.Name.Equals("GetValueFromConsole") select m).First();
            return method.MakeGenericMethod(type).Invoke(null, new object[] { msg });
        }

        private static T GetValueFromConsole<T>(string msg)
        {
            T output;
            Console.Write(msg);
            while (!TryParse(Console.ReadLine(), out output))
                Console.WriteLine("Ошибка при чтении. Попробуйте ещё раз.");
            return output;
        }

        private static bool TryParse<T>(string str, out T value)
        {
            MethodInfo MethodTryParse = GetParser(typeof(T));
            T output = default(T);
            object[] args = new object[] { str, output };
            bool ret = (bool)MethodTryParse.Invoke(null, args);
            if (ret)
                value = (T)args[1];
            else
                value = default(T); 
            return ret;
        }

        private static MethodInfo GetParser(Type t)
        {
            return t.GetMethod("TryParse", new Type[] { typeof(string), t.MakeByRefType() }) ??
                throw new NullReferenceException();
        }
    }
}
