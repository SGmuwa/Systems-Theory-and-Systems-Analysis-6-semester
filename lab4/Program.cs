﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4
{
    public class BeeAlgorithm
    {
        //Алгоритм для нескольких экстремумов
        //Области рассматриваются поочерёдно, пока не выпадет максимальная точка определённое количество раз подряд
        static readonly Random random = new Random();
        const double EVKLID_DISTANCE = 2; // Минимальное растояние между разведчиками
        static int break_parameter = 5;

        //Про пчёл
        static int scoutCount = 10;//Колличество пчёл разведчиков
        static int beeCont_toBest = 5;//Колличество пчёл, отправляемых на лучшие участки


        //Про области
        static int bestAreasCount = 2;//Колличество лучших участков
        static int selectedAreasCount = 3;//Колличество выбранных участков
        static int areaSize = 5; // Размер области для каждого участка

        //Границы поиска
        //Область D
        static int StartX = -100;
        static int StopX = 100;
        static int StartY = -100;
        static int StopY = 100;
        static List<Bee> TEM_CHEACK = new List<Bee>();
        static int tem_ch_count = 0;
        static readonly List<Bee> RES = new List<Bee>();
        public static void Main(string[] args)
        {


            List<Bee> listBee = new List<Bee>();//Точки разведчиков
                                                //Генерация случайных точек в области D, куда отправляются пчёлы - разведчики
            for (int i = 0; i < scoutCount; i++)
            {
                listBee.Add(new Bee(random.Next(StopX) + StartX, random.Next(StopY) + StartY));
                double valFunc = listBee[i].AmountNectar();
                listBee[i].ValueFunc = valFunc;
                listBee[i].PointCountBest = 0;
            }

            // Сортировка списка для поиска лучших точек
            listBee.Sort();
            listBee.Reverse();

            for (int i = 0; i < scoutCount; i++)
                Console.WriteLine(listBee[i].X + "  " + listBee[i].Y + "  " + listBee[i].ValueFunc);


            //Списки хранят лучшие точки и выбранные соответственно
            List<Bee> listBestArea = new List<Bee>();//Список лучших точек (первая подобласть)
            List<Bee> listOptionArea = new List<Bee>();//Список остальных точек (вторая подобласть)
            for (int i = 0; i < bestAreasCount; i++)
            {
                listBestArea.Add(new Bee(listBee[i].X, listBee[i].Y));
            }
            for (int i = bestAreasCount; i < bestAreasCount + selectedAreasCount; i++)
            {
                listOptionArea.Add(new Bee(listBee[i].X, listBee[i].Y));
            }
            for (int i = 0; i < listBestArea.Count; i++)
            {
                listBestArea[i].ValueFunc = listBestArea[i].AmountNectar();
                listBestArea[i].PointCountBest = 0;
            }
            for (int i = 0; i < listOptionArea.Count; i++)
            {
                listOptionArea[i].ValueFunc = listOptionArea[i].AmountNectar();
                listOptionArea[i].PointCountBest = 0;
            }

            //Считаем евклидово расстояние между точками с наибольшим значением функции и другими точками
            for (int i = 0; i < listBestArea.Count; i++)
                for (int j = 0; j < listOptionArea.Count; j++)
                    if (listBestArea[i].Distance(listOptionArea[i].X, listOptionArea[i].Y) < EVKLID_DISTANCE && listOptionArea[j].PointCountBest == 0)
                    {
                        listBestArea[i].PointCountBest = listBestArea[i].PointCountBest + 1;
                        listOptionArea[j].PointCountBest = -1;
                    }
            Console.WriteLine("listBestArea");
            for (int i = 0; i < listBestArea.Count; i++)
            {
                Console.WriteLine(listBestArea[i].X + "  " + listBestArea[i].Y + "  " + listBestArea[i].ValueFunc + "   " + listBestArea[i].PointCountBest);
            }
            Console.WriteLine("listOptionArea");
            for (int i = 0; i < listOptionArea.Count; i++)
            {
                Console.WriteLine(listOptionArea[i].X + "  " + listOptionArea[i].Y + "  " + listOptionArea[i].ValueFunc + "   " + listOptionArea[i].PointCountBest);
            }


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
            Console.WriteLine("\n" + "NEW listBee");
            for (int i = 0; i < listBee.Count; i++)
            {
                Console.WriteLine(listBee[i].X + "  " + listBee[i].Y + "  " + listBee[i].ValueFunc + "   " + listBee[i].PointCountBest);
            }

            List<Bee> listRes = new List<Bee>();

            //Рассматриваем области
            for (int i = 0; i < listBee.Count; i++)
            {
                int centerX = listBee[i].X;
                int centerY = listBee[i].Y;
                Console.WriteLine("\n" + "Область: " + i);
                do
                {
                    List<Bee> listN = new List<Bee> // Пчёлы, посылаемые в область
                    {
                        new Bee(centerX, centerY, 0)
                    };
                    listN[0].ValueFunc = listN[0].AmountNectar();
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
                    { //Генерируем точки в области в колличестве пчёл, посылыемых в область
                        int x = random.Next(upper_coordX - lower_coordX + 1) + lower_coordX;
                        int y = random.Next(upper_coordY - lower_coordY + 1) + lower_coordY;
                        listN.Add(new Bee(x, y, 0));
                        listN[j].ValueFunc = listN[j].AmountNectar();
                    }

                    //Сортировка списка для поиска лучших точек
                    listN.Sort();
                    listN.Reverse();
                    WriteList(listN);
                    bool ch = false;
                    for (int j = 0; j < TEM_CHEACK.Count; j++)
                        if (TEM_CHEACK[j].X == listN[0].X && TEM_CHEACK[j].Y == listN[0].Y)
                        {
                            ch = true;
                            break;
                        }
                    if (ch)
                    {
                        tem_ch_count++;
                        TEM_CHEACK.Add(new Bee(listN[0].X, listN[0].Y));
                        centerX = listN[0].X;
                        centerY = listN[0].Y;
                    }
                    else
                    {
                        tem_ch_count = 0;
                        TEM_CHEACK.Add(new Bee(listN[0].X, listN[0].Y));
                        centerX = listN[0].X;
                        centerY = listN[0].Y;
                    }

                    if (tem_ch_count >= break_parameter)
                        listRes.Add(new Bee(listN[0].X, listN[0].Y));
                    listN.Clear();
                } while (tem_ch_count < break_parameter);

                TEM_CHEACK.Clear();
                tem_ch_count = 0;
            }

            for (int i = 0; i < listRes.Count; i++)
            {
                listRes[i].ValueFunc = listRes[i].AmountNectar();
            }
            //Сортировка списка для поиска лучших точек
            listRes.Sort();
            listRes.Reverse();
            Console.WriteLine("\n" + "Варианты: ");
            for (int i = 0; i < listRes.Count; i++)
            {
                Console.WriteLine(listRes[i].ToString());
            }
            Console.WriteLine("\n" + "Лучший вариант: ");
            Console.WriteLine(listRes[0].ToString());
            Console.ReadLine();
        }
        private static void WriteList(IEnumerable list)
        {
            Console.WriteLine();
            foreach (object b in list)
                Console.WriteLine(b.ToString());
        }
    }
}