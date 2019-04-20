using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    class ЗемляИПочва
    {
    }
    abstract class Сотрудник
    {
        class Менеджер : Сотрудник
        {

        }
        class Пахарь : Сотрудник
        {

        }
        class Инжерен : Сотрудник
        {

        }
    }
    abstract class Рукотворство
    {
        class Здания : Рукотворство
        {

        }
        class ИнструментыИТехника : Рукотворство
        {

        }
        abstract class БыстрыеРесурсы : Рукотворство
        {
            class Семена : БыстрыеРесурсы
            {

            }
            class Жидкости : БыстрыеРесурсы
            {

            }
        }
    }
    class КнижкаЗаказов
    {

    }
}
