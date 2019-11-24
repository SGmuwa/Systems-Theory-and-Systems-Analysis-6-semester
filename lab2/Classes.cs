using System;
using System.Collections.Generic;
using static lab1.Controller;

namespace lab1
{
    public static class Controller
    {
        public static int deep = 3;

        public static string ToStringSafeDeep(Func<string> toString)
        {
            string output;
            if (--deep >= 0)
                output = toString();
            else
                output = " ... ";
            deep++;
            return output;
        }
    }

    public class ЗемляИПочва
    {
        public DateTime ДатаПоследнегоИспользования;
        
        public long ИндентификаторПоложенияЗемли;

        public override string ToString()
            => ToStringSafeDeep(() => $"{typeof(ЗемляИПочва)} ДатаПоследнегоИспользования: {ДатаПоследнегоИспользования}, ИндентификаторПоложенияЗемли: {ИндентификаторПоложенияЗемли}");
        
    }
    
    public abstract class Сотрудник
    {
        
        public DateTime ДатаПринятияНаРаботу;
        
        public string Имя;
        
        public List<object> ОтветственныйЗа = new List<object>();
        
        public long ТелефонСотрудника;


        public override string ToString()
            => ToStringSafeDeep(() => $"{typeof(Сотрудник)} ДатаПринятияНаРаботу: {ДатаПринятияНаРаботу}, Имя: {Имя}, ОтветственныйЗа: [{ОтветственныйЗа.ToStringAll()}], ТелефонСотрудника: {ТелефонСотрудника}");

        public class Менеджер : Сотрудник
        {
            public long ТелефонПоКоторомуЗвонятКлиенты;

            public override string ToString()
                => ToStringSafeDeep(() => $"{base.ToString()}, {typeof(Менеджер)}, ТелефонПоКоторомуЗвонятКлиенты: {ТелефонПоКоторомуЗвонятКлиенты}");
        }
        
        public class Пахарь : Сотрудник
        {
            public override string ToString()
                => ToStringSafeDeep(() => $"{base.ToString()}, {typeof(Пахарь)}");
        }
        
        public class Инжерен : Сотрудник
        {
            public override string ToString()
                => ToStringSafeDeep(() => $"{base.ToString()}, {typeof(Инжерен)}");
        }
    }
    
    public abstract class Рукотворство
    {
        
        public DateTime ДатаИзмененияСостояния;
        
        public List<Сотрудник> ИсточникПоследнегоИзменения = new List<Сотрудник>();
        
        public double КоличественноеИзменение = 0;

        public override string ToString()
            => ToStringSafeDeep(() => $"{base.ToString()}, ДатаИзмененияСостояния: {ДатаИзмененияСостояния}, ИсточникПоследнегоИзменения: [{ИсточникПоследнегоИзменения.ToStringAll(", ")}], КоличественноеИзменение: {КоличественноеИзменение}");

        public class Здания : Рукотворство
        {
            public string Адрес;

            public override string ToString()
                => ToStringSafeDeep(() => $"{base.ToString()}, {typeof(Здания)}, Адрес: {Адрес}");
        }
        
        public class ИнструментыИТехника : Рукотворство
        {
            
            public string Производитель;
            
            public List<БыстрыеРесурсы> Топливо;

            public override string ToString()
                => ToStringSafeDeep(() => $"{base.ToString()}, {typeof(ИнструментыИТехника)}, Производитель: {Производитель}, Топливо: [{Топливо.ToStringAll(", ")}]");
        }
        
        public abstract class БыстрыеРесурсы : Рукотворство
        {
            
            public class Семена : БыстрыеРесурсы
            {
                public override string ToString()
                    => ToStringSafeDeep(() => $"{base.ToString()}, {typeof(Семена)}");
            }
            
            public class Жидкости : БыстрыеРесурсы
            {
                public override string ToString()
                    => ToStringSafeDeep(() => $"{base.ToString()}, {typeof(Жидкости)}");
            }
        }
    }
    
    public class КнижкаЗаказов
    {
        
        public DateTime ДатаДобавления;
        
        public List<Сотрудник.Менеджер> КтоДобавил = new List<Сотрудник.Менеджер>();
        
        public long ТелефонДляСвязиСКлиентом;

        public override string ToString()
            => ToStringSafeDeep(() => $"{base.ToString()}, ДатаДобавления: {ДатаДобавления}, КтоДобавил: [{КтоДобавил.ToStringAll(", ")}], ТелефонДляСвязиСКлиентом: {ТелефонДляСвязиСКлиентом}");
    }
}
