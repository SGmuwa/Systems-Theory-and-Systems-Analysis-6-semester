﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    static class Program
    {
        static void Main(string[] args)
        {
            List<object> list = new List<object>(new object[]{
                new ЗемляИПочва() { ДатаПоследнегоИспользования = new DateTime(1, 1, 2, 1, 0, 0), ИндентификаторПоложенияЗемли = 1 },
                new ЗемляИПочва() { ДатаПоследнегоИспользования = new DateTime(1, 1, 2, 1, 0, 0), ИндентификаторПоложенияЗемли = 2 },

                new Сотрудник.Менеджер() { ДатаПринятияНаРаботу = new DateTime(1, 1, 2, 1, 0, 0), Имя = "Рокаов Еаоу Окшалц", ТелефонПоКоторомуЗвонятКлиенты = 10485930485, ТелефонСотрудника = 2949850395903 },
                new Сотрудник.Инжерен() { ДатаПринятияНаРаботу = new DateTime(1, 2, 2, 1, 0, 0), Имя = "Шопок Оаклу Воалк", ТелефонСотрудника = 238348732832 },
                new Сотрудник.Пахарь() { ДатаПринятияНаРаботу = new DateTime(1, 2, 3, 1, 0, 0), Имя = "Окоеш Акорп Оекгв", ТелефонСотрудника = 12923949393 },

                new Рукотворство.Здания() { Адрес = "Огкипа 5", ДатаИзмененияСостояния = new DateTime(1, 2, 2, 1, 0, 0), КоличественноеИзменение = 1.0f }
            });
            
            list.FindClasses((Сотрудник.Пахарь t) => t.Имя == "Окоеш Акорп Оекгв")
                .ОтветственныйЗа.Add(list.FindClasses((ЗемляИПочва t) => t.ИндентификаторПоложенияЗемли == 1));
            list.FindClasses((Сотрудник.Пахарь t) => t.Имя == "Окоеш Акорп Оекгв")
                .ОтветственныйЗа.Add(list.FindClasses((ЗемляИПочва t) => t.ИндентификаторПоложенияЗемли == 2));

            list.FindClasses((Рукотворство.Здания t) => t.Адрес == "Огкипа 5")
                .ИсточникПоследнегоИзменения.Add(list.FindClasses((Сотрудник.Инжерен t) => t.Имя == "Шопок Оаклу Воалк"));

            list.WriteAllLine();

            ParserWithIComparable.VisualSelect(list);
            Console.ReadLine();
        }
        
        public static SEARCH FindClasses<SEARCH, T>(this List<T> list, Predicate<SEARCH> match) where SEARCH : T
        {
            return ((SEARCH)list.Find((T o) => o is SEARCH && match.Invoke((SEARCH)o)));
        }

        public static void WriteAll<T>(this IEnumerable<T> values)
            => Console.WriteLine(values.ToStringAll(", "));

        public static void WriteAllLine<T>(this IEnumerable<T> values, bool needNumbers = false)
            => Console.WriteLine(values.ToStringAll("\n", needNumbers));

        public static string ToStringAll<T>(this IEnumerable<T> values, string separator = ", ", bool needNumbers = false)
        {
            StringBuilder sb = new StringBuilder();
            IEnumerator<T> en = values.GetEnumerator();
            int i = 0;
            foreach(T v in values)
            {
                if(needNumbers)
                {
                    sb.Append(i++);
                    sb.Append(". ");
                }
                sb.Append(v);
                sb.Append(separator);
            }
            if (sb.Length >= separator.Length)
                sb.Length -= separator.Length;
            return sb.ToString();
        }
    }
}
