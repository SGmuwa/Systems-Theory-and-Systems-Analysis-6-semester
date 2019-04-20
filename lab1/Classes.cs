using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace lab1
{
    [DataContract]
    public class ЗемляИПочва
    {
        [DataMember]
        public DateTime ДатаПоследнегоИспользования;
        [DataMember]
        public long ИндентификаторПоложенияЗемли;
    }
    [DataContract]
    public abstract class Сотрудник
    {
        [DataMember]
        public DateTime ДатаПринятияНаРаботу;
        [DataMember]
        public string Имя;
        [DataMember]
        public List<object> ОтветственныйЗа = new List<object>();
        [DataMember]
        public long ТелефонСотрудника;
        [DataContract]
        public class Менеджер : Сотрудник
        {
            [DataMember]
            public long ТелефонПоКоторомуЗвонятКлиенты;
        }
        [DataContract]
        public class Пахарь : Сотрудник
        {

        }
        [DataContract]
        public class Инжерен : Сотрудник
        {

        }
    }
    [DataContract]
    public abstract class Рукотворство
    {
        [DataMember]
        public DateTime ДатаИзмененияСостояния;
        [DataMember]
        public List<Сотрудник> ИсточникПоследнегоИзменения = new List<Сотрудник>();
        [DataMember]
        public double КоличественноеИзменение = 0;
        [DataContract]
        public class Здания : Рукотворство
        {
            [DataMember]
            public string Адрес;
        }
        [DataContract]
        public class ИнструментыИТехника : Рукотворство
        {
            [DataMember]
            public string Производитель;
            [DataMember]
            public List<БыстрыеРесурсы> Топливо;
        }
        [DataContract]
        public abstract class БыстрыеРесурсы : Рукотворство
        {
            [DataContract]
            public class Семена : БыстрыеРесурсы
            {

            }
            [DataContract]
            public class Жидкости : БыстрыеРесурсы
            {

            }
        }
    }
    [DataContract]
    public class КнижкаЗаказов
    {
        [DataMember]
        public DateTime ДатаДобавления;
        [DataMember]
        public List<Сотрудник.Менеджер> КтоДобавил = new List<Сотрудник.Менеджер>();
        [DataMember]
        public long ТелефонДляСвязиСКлиентом;
    }
}
