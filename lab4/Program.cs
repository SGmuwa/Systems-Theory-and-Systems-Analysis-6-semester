using System;
using System.Collections;
using System.Collections.Generic;

namespace lab4
{
    /// <summary>
    /// Класс представляет собой реализацию пчелиного алгоритма.
    /// </summary>
    public class Program
    {
        //Алгоритм для нескольких экстремумов
        //Области рассматриваются поочерёдно, пока не выпадет максимальная точка определённое количество раз подряд
        static readonly Random random = new Random();
        /// <summary>
        /// Минимальное растояние между разведчиками.
        /// </summary>
        const double EVKLID_DISTANCE = 2;
        /// <summary>
        /// Сколько раз одна и та же пчела в области являлась максимальной?
        /// Этот параметр задаёт ограничение на это количество.
        /// <seealso cref="tem_ch_count"/>
        /// </summary>
        const int break_parameter = 5;

        // Про пчёл

        /// <summary>
        /// Колличество пчёл разведчиков.
        /// </summary>
        private const int scoutCount = 10;
        /// <summary>
        /// Колличество пчёл, отправляемых на лучшие участки.
        /// </summary>
        private const int beeCont_toBest = 5;


        // Про области

        /// <summary>
        /// Колличество лучших участков.
        /// </summary>
        private const int bestAreasCount = 2;
        /// <summary>
        /// Колличество выбранных участков.
        /// </summary>
        private const int selectedAreasCount = 3;
        /// <summary>
        /// Размер области для каждого участка.
        /// </summary>
        private const int areaSize = 5;

        //Границы поиска
        //Область D
        private const int StartX = -100;
        private const int StopX = 100;
        private const int StartY = -100;
        private const int StopY = 100;

        /// <summary>
        /// Список предназначен для хранения истории найденых
        /// точек в области с максимальным значением функции.
        /// </summary>
        private static List<Bee> TEM_CHEACK = new List<Bee>();
        /// <summary>
        /// Сколько раз одна и та же пчела в области являлась максимальной?
        /// </summary>
        private static int tem_ch_count = 0;

        public static void Main(string[] args)
        {
            List<Bee> listBee = new List<Bee>(); // Точки разведчиков
            //Генерация случайных точек в области D, куда отправляются пчёлы - разведчики.
            for (int i = 0; i < scoutCount; i++)
            {
                listBee.Add(new Bee(random.Next(StopX) + StartX, random.Next(StopY) + StartY, 0));
            }

            // Сортировка списка для поиска лучших точек
            listBee.Sort((a, b) => -1 * a.CompareTo(b));

            for (int i = 0; i < scoutCount; i++)
                Console.WriteLine(listBee[i].ToString());


            //Списки хранят лучшие точки и выбранные соответственно
            List<Bee> listBestArea = new List<Bee>(); // Список лучших точек (первая подобласть)
            List<Bee> listOptionArea = new List<Bee>(); // Список остальных точек (вторая подобласть)
            for (int i = 0; i < bestAreasCount; i++)
            {
                listBestArea.Add(listBee[i]);
            }
            for (int i = bestAreasCount; i < bestAreasCount + selectedAreasCount; i++)
            {
                listOptionArea.Add(listBee[i]);
            }

            // Считаем евклидово расстояние между точками с наибольшим значением функции и другими точками
            for (int i = 0; i < listBestArea.Count; i++)
                for (int j = 0; j < listOptionArea.Count; j++)
                    if (listBestArea[i].Distance(listOptionArea[i].X, listOptionArea[i].Y) < EVKLID_DISTANCE && listOptionArea[j].PointCountBest == 0)
                    {
                        listBestArea[i].PointCountBest = listBestArea[i].PointCountBest + 1;
                        listOptionArea[j].PointCountBest = -1;
                    }
            Console.WriteLine("listBestArea");
            WriteList(listBestArea, ConsoleColor.White);
            Console.WriteLine("listOptionArea");
            WriteList(listOptionArea, ConsoleColor.White);
            
            //Объединяются лучшие области и остальные выбранные в один список для удобства
            listBee.Clear();
            for (int i = 0; i < listBestArea.Count; i++)
            {
                listBee.Add(listBestArea[i]);
            }
            for (int i = 0; i < listOptionArea.Count; i++)
            {
                if (listOptionArea[i].PointCountBest != -1)
                    listBee.Add(listOptionArea[i]);
            }
            Console.WriteLine("\nNEW listBee");
            WriteList(listBee, ConsoleColor.White);

            List<Bee> listRes = new List<Bee>();

            // Рассматриваем области
            for (int i = 0; i < listBee.Count; i++)
            {
                int centerX = listBee[i].X;
                int centerY = listBee[i].Y;
                Console.WriteLine($"\n------------------------------\nОбласть: {i}");
                do
                {
                    List<Bee> listN = new List<Bee> // Пчёлы, посылаемые в область
                    {
                        new Bee(centerX, centerY, 0)
                    };
                    int tempCountN;
                    if (i < bestAreasCount)
                        // Колличество пчёл, посылаемых на лучшие участки
                        tempCountN = beeCont_toBest + selectedAreasCount * listBee[i].PointCountBest;
                    else
                        // Колличество пчёл, посылаемых на остальные участки
                        tempCountN = selectedAreasCount;
                    // Определяем верхнии и нижние координаты участка (границы области)
                    int lower_coordX = centerX - areaSize;
                    int upper_coordX = centerX + areaSize;
                    int lower_coordY = centerY - areaSize;
                    int upper_coordY = centerY + areaSize;

                    for (int j = 1; j < tempCountN + 1; j++)
                    { // Генерируем точки в области в колличестве пчёл, посылыемых в область
                        int x = random.Next(upper_coordX - lower_coordX + 1) + lower_coordX;
                        int y = random.Next(upper_coordY - lower_coordY + 1) + lower_coordY;
                        listN.Add(new Bee(x, y, 0));
                    }

                    // Сортировка списка для поиска лучших точек
                    listN.Sort((a, b) => -1 * a.CompareTo(b));

                    WriteList(listN);
                    bool ch = false;
                    for (int j = 0; j < TEM_CHEACK.Count; j++)
                        if (TEM_CHEACK[j].X == listN[0].X && TEM_CHEACK[j].Y == listN[0].Y)
                        {
                            ch = true;
                            break;
                        }
                    if (ch)
                        tem_ch_count++;
                    else
                        tem_ch_count = 0;
                    TEM_CHEACK.Add(listN[0]);
                    centerX = listN[0].X;
                    centerY = listN[0].Y;
                    
                    if (tem_ch_count >= break_parameter)
                        listRes.Add(listN[0]);
                    listN.Clear();
                } while (tem_ch_count < break_parameter);

                TEM_CHEACK.Clear();
                tem_ch_count = 0;
            }
            
            // Сортировка списка для поиска лучших точек
            listRes.Sort((a, b) => -1 * a.CompareTo(b));

            Console.WriteLine("\nВарианты: ");
            for (int i = 0; i < listRes.Count; i++)
            {
                Console.WriteLine(listRes[i].ToString());
            }
            Console.WriteLine("\nЛучший вариант: ");
            Console.WriteLine(listRes[0].ToString());
            Console.ReadLine();
        }

        private static void WriteList(IEnumerable list, ConsoleColor color = ConsoleColor.Yellow)
        {
            Console.WriteLine();
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            foreach (object b in list)
                Console.WriteLine(b.ToString());
            Console.ForegroundColor = old;
        }
    }
}
