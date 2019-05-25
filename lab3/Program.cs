using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lab3
{
    class Program
    {
        /// <summary>
        /// Муравьиный алгоритм.
        /// </summary>
        public static void Main(string[] args)
        {
            //Берётся максимальная вероятность, что не правильно
            //Все равно отстой без веса ребер
            int t = 0; // Количество итераций
            double p = 0.2; //Скорость испарения феромона
            const double a = 1.0;
            double betta = 1.0;
            int xk;//Выбранная вершина
            double[,] pheromones;
            double[,] weights;
            List<CurrentPath> list_cur_path = new List<CurrentPath>();
            Random ran = new Random();
            InitPaths(list_cur_path);
            pheromones = InitMatrix();
            weights = InitWeightMatrix();
            double tmp_pheromone_concentration;
            List<Element> listPa = new List<Element>(); // Лист вероятностей попадания в вершину id

            do
            {
                for (int k = 0; k < list_cur_path.Count; k++)
                {//Построение пути для каждого муравья
                    xk = 0;
                    list_cur_path[k].Path.Add(xk);
                    do
                    {
                        tmp_pheromone_concentration = 0;
                        for (int i = 0; i < pheromones.GetLength(1); i++)
                        {
                            if (pheromones[xk, i] != double.PositiveInfinity && !list_cur_path[k].Path.Contains(i))
                            {
                                tmp_pheromone_concentration += Math.Pow(pheromones[xk, i], a) / Math.Pow(weights[xk, i], betta);
                                if (tmp_pheromone_concentration == 0)
                                    tmp_pheromone_concentration = double.Epsilon;
                            }
                        }
                        for (int i = 0; i < pheromones.GetLength(1); i++)
                        {
                            if (pheromones[xk, i] != double.PositiveInfinity && !list_cur_path[k].Path.Contains(i))
                            {
                                double Pa = Math.Pow(pheromones[xk, i], a) / Math.Pow(weights[xk, i], betta) / tmp_pheromone_concentration;
                                Pa *= 100;
                                if(Pa > 0)
                                    listPa.Add(new Element(i, Pa));
                            }
                        }
                        if (listPa.Count == 0)
                        { // Если идти больше некуда. Удаляем весь путь и выходим. И не будем его учитывать
                            list_cur_path[k].Path.Clear();
                            list_cur_path[k].MasVesov.Clear();
                            xk = 4;
                        }
                        else
                        {// Если дальнейший путь может быть продолжен
                            double[] temMas = new double[listPa.Count + 1];
                            double tmpVer = 0.0;
                            temMas[0] = tmpVer;

                            for (int i = 1; i < temMas.Length; i++)
                            {
                                tmpVer += listPa[i - 1].Veroyatnost;
                                temMas[i] = tmpVer;
                            }
                            double tempRandom = ran.NextDouble() * 100;

                            for (int i = 0; i < temMas.Length - 1; i++)
                            {
                                if (temMas[i] < tempRandom && temMas[i + 1] > tempRandom)
                                {
                                    list_cur_path[k].MasVesov.Add(weights[xk, listPa[i].Id]);
                                    xk = listPa[i].Id;
                                    list_cur_path[k].Path.Add(xk);
                                    i = temMas.Length - 1;
                                }
                            }
                        }
                        if (listPa.Count != 0) listPa.Clear();
                    } while (xk != 4); // Пока конечная вершина не достигнута
                    int j = 0;
                    Console.Write("Муравей " + list_cur_path[k].IdAnt + " путь: " + "\t");
                    while (j < list_cur_path[k].Path.Count)
                    {
                        if (list_cur_path[k].Path.Count != 0)
                            Console.Write(list_cur_path[k].Path[j++] + "\t");
                    }
                    Console.WriteLine();
                }

                // Испарение феромона
                Parallel.For(0, pheromones.GetLength(0), (int i) =>
                {
                    Parallel.For(0, pheromones.GetLength(1), (int j) =>
                        {
                            double tmp = pheromones[i, j];
                            pheromones[i, j] = (1.0 - p) * tmp;
                        });
                });
                
                // Увеличиваем концентрацию феромонов
                for (int k = 0; k < list_cur_path.Count; k++) // Для каждого муравья
                {
                    if (list_cur_path[k].Path.Count != 0)
                    { // Если путь муравья не пуст, то считаем, сколько надо добавить феромонов

                        double tempVesPath = 0;
                        for (int i = 0; i < list_cur_path[k].MasVesov.Count; i++) // Идём по пути муравья и складываем веса всех дуг пути
                            tempVesPath += list_cur_path[k].MasVesov[i];

                        double countPheromone = 1.0 / tempVesPath;

                        int[] tmpMasVertix = new int[list_cur_path[k].Path.Count];
                        for (int i = 0; i < tmpMasVertix.Length; i++)
                            tmpMasVertix[i] = list_cur_path[k].Path[i];
                        for (int i = 0; i < tmpMasVertix.Length - 1; i++)
                        {
                            int output = tmpMasVertix[i];
                            int input = tmpMasVertix[i + 1];
                            pheromones[output, input] += countPheromone;
                            if (output != input) pheromones[input, output] += countPheromone;
                        }
                    }
                }

                t++;
                for (int i = 0; i < list_cur_path.Count; i++)
                {
                    if (list_cur_path[i].Path.Count != 0)
                        list_cur_path[i].Path.Clear();
                    if (list_cur_path[i].MasVesov.Count != 0)
                        list_cur_path[i].MasVesov.Clear();
                }
                WriteMatrix(pheromones);
                Console.WriteLine("Шаг: " + t + "\n");
            } while (ConsoleGetBool("Сделать ещё одну итерацию?"));
            Console.ReadKey();
        }

        private static bool ConsoleGetBool(string message)
        {
            string line;
            while(true)
            {
                Console.Write(message + "\ny - yes, n - no.");
                line = Console.ReadLine();
                if (line.Length != 1)
                    continue;
                else if (line == "y")
                    return true;
                else if (line == "n")
                    return false;
                continue;
            }
        }

        private static void InitPaths(List<CurrentPath> list_cur_path)
        { // Создаётся 20 муравьёв
            for(int i = 0; i < 20; i++)
                list_cur_path.Add(new CurrentPath(i));
        }

        /// <summary>
        /// Иницциализация матриццы ферамонов.
        /// </summary>
        private static double[,] InitMatrix()
        {
            return new double[7, 7] {
                { 0, 2, 0, 2, 0, 5, 0 },
                { 0, 0, 2, 2, 0, 0, 0 },
                { 0, 0, 0, 2, 2, 0, 0 },
                { 0, 0, 0, 0, 2, 2, 2 },
                { 0, 0, 0, 0, 0, 0, 2 },
                { 0, 0, 0, 0, 0, 0, 2 },
                { 0, 0, 0, 0, 0, 0, 0 }
            };
        }

        private static void WriteMatrix(double[,] Matrix)
        {
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    Console.Write(Matrix[i, j].ToString("N1") + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Создание графа переходов.
        /// </summary>
        /// <returns>Возвращает матрицу весов.</returns>
        private static double[,] InitWeightMatrix()
        {
            double i = double.PositiveInfinity;
            double[,] output = new double[7, 7] {
                { 0,  16, i,  8,  i,  20, i },
                { 16, 0,  2,  10, i,  i,  i },
                { i,  2,  0,  40, 7,  i,  i },
                { 8,  10, 40, 0,  50, 20, 15 },
                { i,  i,  7,  50, 0,  i,  2 },
                { 20, i,  i,  20, i,  0,  5 },
                { i,  i,  i,  15, 2,  5,  0 }
            };
            return output;
        }
    }
}
