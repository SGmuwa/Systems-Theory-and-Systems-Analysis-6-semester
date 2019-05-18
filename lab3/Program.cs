using System;
using System.Collections.Generic;

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
            int nk = 20;
            int t = 0;
            int v = 7;
            double Pa;
            double p = 0.2;//Скорость испарения феромона
            double a = 1.0;
            double betta = 1.0;
            int xk;//Выбранная вершина
            double[,] matrix = new double[v, v];
            double[,] weightMatrix = new double[v, v];
            List<CurrentPath> list_cur_path = new List<CurrentPath>();
            Random ran = new Random();
            Init(list_cur_path);
            InitMatrix(matrix, v);
            weightMatrix = InitWeightMatrix();
            double tmp_pheromone_concentration;
            List<Element> listPa = new List<Element>(); // Лист вероятностей попадания в вершину id

            do
            {
                for (int k = 0; k < nk; k++)
                {//Построение пути для каждого муравья
                    xk = 0;
                    list_cur_path[k].Path.Add(xk);
                    do
                    {
                        tmp_pheromone_concentration = 0;
                        for (int m = 0; m < v; m++)
                        {
                            if (list_cur_path[k].Path.Count == 1 && matrix[xk, m] != double.PositiveInfinity)
                            {
                                tmp_pheromone_concentration += (Math.Pow(matrix[xk, m], a)) * (1.0 / Math.Pow(weightMatrix[xk, m], betta));
                            }
                            else if (list_cur_path[k].Path.Count > 1 && matrix[xk, m] != double.PositiveInfinity && !list_cur_path[k].Path.Contains(m))
                            {
                                tmp_pheromone_concentration += (Math.Pow(matrix[xk, m], a)) * (1.0 / Math.Pow(weightMatrix[xk, m], betta));
                            }
                        }
                        for (int i = 0; i < v; i++)
                        {
                            if (matrix[xk, i] != double.PositiveInfinity && !list_cur_path[k].Path.Contains(i))
                            {
                                Pa = ((Math.Pow(matrix[xk, i], a)) * (1.0 / Math.Pow(weightMatrix[xk, i], betta))) / tmp_pheromone_concentration;
                                Pa *= 100;
                                listPa.Add(new Element(i, Pa));
                            }
                        }
                        if (listPa.Count == 0)
                        { //Если идти больше некуда. Удаляем весь путь и выходим. И не будем его учитывать
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
                                    list_cur_path[k].MasVesov.Add(weightMatrix[xk, listPa[i].Id]);
                                    xk = listPa[i].Id;
                                    list_cur_path[k].Path.Add(xk);
                                    i = temMas.Length - 1;
                                }
                            }
                        }
                        if (listPa.Count != 0) listPa.Clear();
                    } while (xk != 4);//Пока конечная вершина не достигнута
                    int j = 0;
                    Console.Write("Ant " + list_cur_path[k].IdAnt + "  path: " + "\t");
                    while (j < list_cur_path[k].Path.Count)
                    {
                        if (list_cur_path[k].Path.Count != 0)
                            Console.Write(list_cur_path[k].Path[j++] + "\t");
                    }
                    Console.WriteLine();
                }


                //Испарение феромона
                for (int i = 0; i < v; i++)
                    for (int j = 0; j < v; j++)
                        if (matrix[i, j] != double.PositiveInfinity)
                        {
                            double tmp = matrix[i, j];
                            matrix[i, j] = (1.0 - p) * tmp;
                        }



                //Увеличиваем концентрацию феромонов
                for (int k = 0; k < nk; k++)//Для каждого муравья
                {
                    if (list_cur_path[k].Path.Count != 0)
                    {//Если путь муравья не пуст, то считаем, сколько надо добавить феромонов

                        double tempVesPath = 0;
                        for (int i = 0; i < list_cur_path[k].MasVesov.Count; i++) // Идем по пути муравья и складываем веса всех дуг пути
                            tempVesPath += list_cur_path[k].MasVesov[i];

                        double countPheromone = (double)1.0 / tempVesPath;

                        int[] tmpMasVertix = new int[list_cur_path[k].Path.Count];
                        for (int i = 0; i < tmpMasVertix.Length; i++)
                            tmpMasVertix[i] = list_cur_path[k].Path[i];
                        for (int i = 0; i < tmpMasVertix.Length - 1; i++)
                        {
                            int output = tmpMasVertix[i];
                            int input = tmpMasVertix[i + 1];
                            matrix[output, input] += countPheromone;
                            if (output != input) matrix[input, output] += countPheromone;
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
                Console.WriteLine("t+1: " + t + "\n");
            } while (t < 20);
            Console.ReadKey();
        }

        private static void Init(List<CurrentPath> list_cur_path)
        {//Создается 10 муравьев
            for(int i = 0; i < 20; i++)
            {
                CurrentPath path = new CurrentPath();
                path.IdAnt = i;
                path.Path = new List<int>();
                path.MasVesov = new List<double>();
                list_cur_path.Add(path);
            }
        }

        private static void InitMatrix(double[,] matrix, int v)
        {
            for (int i = 0; i < v; i++)
                for (int j = 0; j < v; j++)
                    if ((i == 0 && j == 3) || (i == 0 && j == 5) || (i == 0 && j == 1) ||
                            (i == 1 && j == 2) || (i == 1 && j == 3) ||
                            (i == 2 && j == 4) || (i == 2 && j == 3) ||
                            (i == 3 && j == 4) || (i == 3 && j == 5) || (i == 3 && j == 6) ||
                            (i == 4 && j == 6) ||
                            (i == 5 && j == 6))
                    {
                        matrix[i, j] = 2.0;
                        matrix[j, i] = 2.0;
                    }

            Console.WriteLine("Матрица Феромонов на дугах:");
            for (int i = 0; i < v; i++)
                for (int j = 0; j < v; j++)
                    if (matrix[i, j] == 0.0) matrix[i, j] = double.PositiveInfinity;
            for (int i = 0; i < v; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();

        }

        /// <summary>
        /// Создание графа переходов.
        /// </summary>
        /// <param name="output">Матрица веосв.</param>
        /// <returns>Возвращает матрицу весов.</returns>
        private static double[,] InitWeightMatrix()
        {
            double inf = double.PositiveInfinity;
            double[,] output = new double[7, 7] {
                { inf, inf, inf, inf, inf, inf, inf },
                { inf, inf, inf, inf, inf, inf, inf },
                { inf, inf, inf, inf, inf, inf, inf },
                { inf, inf, inf, inf, inf, inf, inf },
                { inf, inf, inf, inf, inf, inf, inf },
                { inf, inf, inf, inf, inf, inf, inf },
                { inf, inf, inf, inf, inf, inf, inf }
            };
            for (int i = 0; i < output.GetLength(0); i++)
                for (int j = 0; j < output.GetLength(1); j++)
                    if (i == 0 && j == 1) output[i, j] = output[j, i] = 16.0;
                    else if (i == 0 && j == 3) output[i, j] = output[j, i] = 8.0;
                    else if (i == 0 && j == 5) output[i, j] = output[j, i] = 20.0;
                    else if (i == 1 && j == 2) output[i, j] = output[j, i] = 2.0;
                    else if (i == 1 && j == 3) output[i, j] = output[j, i] = 10.0;
                    else if (i == 2 && j == 4) output[i, j] = output[j, i] = 7.0;
                    else if (i == 2 && j == 3) output[i, j] = output[j, i] = 40.0;
                    else if (i == 3 && j == 4) output[i, j] = output[j, i] = 50.0;
                    else if (i == 3 && j == 5) output[i, j] = output[j, i] = 20.0;
                    else if (i == 3 && j == 6) output[i, j] = output[j, i] = 15.0;
                    else if (i == 4 && j == 6) output[i, j] = output[j, i] = 2.0;
                    else if (i == 5 && j == 6) output[i, j] = output[j, i] = 5.0;
            return output;
        }
    }
}
