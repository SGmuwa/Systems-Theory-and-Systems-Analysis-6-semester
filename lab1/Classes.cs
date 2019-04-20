using System;
using System.Collections.Generic;

namespace lab1
{
    public class ЗемляИПочва
    {
        public DateTime ДатаПоследнегоИспользования;
        public long ИндентификаторПоложенияЗемли;
    }
    public abstract class Сотрудник
    {
        public DateTime ДатаПринятияНаРаботу;
        public string Имя;
        public List<Сотрудник> ОтветственныйЗа = new List<Сотрудник>();
        public long ТелефонСотрудника;
        public class Менеджер : Сотрудник
        {
            public long ТелефонПоКоторомуЗвонятКлиенты;
        }
        public class Пахарь : Сотрудник
        {

        }
        public class Инжерен : Сотрудник
        {

        }
    }
    public abstract class Рукотворство
    {
        public DateTime ДатаИзмененияСостояния;
        public List<Сотрудник> ИсточникПоследнегоИзменения = new List<Сотрудник>();
        public double КоличественноеИзменение = 0;
        public class Здания : Рукотворство
        {
            public string Адрес;
        }
        public class ИнструментыИТехника : Рукотворство
        {
            public string Производитель;
            public List<БыстрыеРесурсы> Топливо;
        }
        public abstract class БыстрыеРесурсы : Рукотворство
        {
            public class Семена : БыстрыеРесурсы
            {

            }
            public class Жидкости : БыстрыеРесурсы
            {

            }
        }
    }
    public class КнижкаЗаказов
    {
        public DateTime ДатаДобавления;
        public List<Сотрудник.Менеджер> КтоДобавил = new List<Сотрудник.Менеджер>();
        public long ТелефонДляСвязиСКлиентом;
    }
}
