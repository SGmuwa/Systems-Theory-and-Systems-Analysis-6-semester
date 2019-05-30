using System;
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
            List<object> OnlyOneType = new List<object>(fromSelect);
            OnlyOneType.RemoveAll(c => c.GetType() != type);
            OnlyOneType.WriteAllLine();
            List<FieldInfo> fields = new List<FieldInfo>(type.GetFields(BindingFlags.Public | BindingFlags.Instance));
            fields.RemoveAll(m => m.FieldType.GetInterface("IEnumerable") != null);
            FieldInfo field = SelectFromList(fields, "По какому полю вы хотите сделать поиск?");
            object min = GetValueFromConsole("Минимальное значение поля.", field.FieldType);
            object max = GetValueFromConsole("Максимальное значение поля.", field.FieldType);
            List<object> sortedList = new List<object>();
            Comparer comparer = new Comparer(type, field);
            foreach(object o in OnlyOneType)
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

                if ((x.GetType().Equals(@class) || x.GetType().Equals(field.FieldType))
                    && (y.GetType().Equals(@class) || y.GetType().Equals(field.FieldType)))
                {
                    if (x.GetType().Equals(@class))
                        x = field.GetValue(x);
                    if (y.GetType().Equals(@class))
                        y = field.GetValue(y);
                    return (isReverse ? -1 : 1) * ((IComparable)x).CompareTo(y);
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
            
            MethodInfo method = typeof(ParserWithIComparable).GetMethods(BindingFlags.NonPublic | BindingFlags.Static).Single((m)
                => m.IsGenericMethod && m.Name.Equals("GetValueFromConsole"));
            return method.MakeGenericMethod(type).Invoke(null, new object[] { msg });
        }

        private static T GetValueFromConsole<T>(string msg)
        {
            T output;
            Console.Write(msg);
            while (!TryParse(Console.ReadLine(), out output))
                Console.WriteLine($"Ошибка при чтении. Попробуйте ещё раз. Например, {default(T)}");
            return output;
        }

        private static bool TryParse<T>(string str, out T value)
        {
            value = default(T);
            try
            {
                value = (T)Convert.ChangeType(str, typeof(T));
            } catch
            {
                return false;
            }
            return true;
        }
    }
}
