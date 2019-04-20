using System;
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
            /*
            list.FindClasses((Сотрудник.Пахарь t) => t.Имя == "Окоеш Акорп Оекгв")
                .ОтветственныйЗа.Add(list.FindClasses((ЗемляИПочва t) => t.ИндентификаторПоложенияЗемли == 1));
            list.FindClasses((Сотрудник.Пахарь t) => t.Имя == "Окоеш Акорп Оекгв")
                .ОтветственныйЗа.Add(list.FindClasses((ЗемляИПочва t) => t.ИндентификаторПоложенияЗемли == 2));

            list.FindClasses((Рукотворство.Здания t) => t.Адрес == "Огкипа 5")
                .ИсточникПоследнегоИзменения.Add(list.FindClasses((Сотрудник.Инжерен t) => t.Имя == "Шопок Оаклу Воалк"));
                */
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<object>), new DataContractJsonSerializerSettings()
            {
                KnownTypes = new Type[]
                { typeof(ЗемляИПочва), typeof(Сотрудник.Менеджер), typeof(Сотрудник.Инжерен), typeof(Сотрудник.Пахарь), typeof(Рукотворство.Здания) },
                DateTimeFormat = new DateTimeFormat("dd.MM.yyyy hh:mm:ss")
            } );
            MemoryStream stream1 = new MemoryStream();
            ser.WriteObject(stream1, list);
            stream1.Position = 0;
            Console.WriteLine(new StreamReader(stream1).ReadToEnd());
            Console.ReadLine();
        }

        public static SEARCH FindClasses<SEARCH, T>(this List<T> list, Predicate<SEARCH> match) where SEARCH : T
        {
            return ((SEARCH)list.Find((T o) => o is SEARCH && match.Invoke((SEARCH)o)));
        }
    }
}
