using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            int nk = 20;
            int v = 7;
            int t = 0;
            double Pa;
            double p = 0.2;//Скорость испарения феромона
            double a = 1.0;
            double betta = 1.0;
            int xk;//Выбранная вершина
            double[][] matrix = new double[v][v];
            double[][] weightMatrix = new double[v][v];
            List<CurrentPath> list_cur_path = new List<CurrentPath>();
            init(list_cur_path);
            initMatrix(matrix, v);
            initWeightMatrix(weightMatrix, v);
            double tmp_pheromone_concentration;
            List<Element> listPa = new List<Element>(); // Лист вероятностей попадания в вершину id

            do
            {
                for (int k = 0; k < nk; k++)
                {//Построение пути для каждого муравья
                    xk = 0;
                    list_cur_path[k].getPath().Add(xk);
                    do
                    {
                        tmp_pheromone_concentration = 0;
                        for (int m = 0; m < v; m++)
                        {
                            if (list_cur_path[k].getPath().Count == 1 && matrix[xk][m] != -0.0)
                            {
                                tmp_pheromone_concentration += (Math.Pow(matrix[xk][m], a)) * (1.0 / Math.Pow(weightMatrix[xk][m], betta));
                            }
                            else if (list_cur_path[k].getPath().Count > 1 && matrix[xk][m] != -0.0 && !list_cur_path[k].getPath().Contains(m))
                            {
                                tmp_pheromone_concentration += (Math.Pow(matrix[xk][m], a)) * (1.0 / Math.Pow(weightMatrix[xk][m], betta));
                            }
                        }
                        for (int i = 0; i < v; i++)
                        {
                            if (matrix[xk][i] != -0.0 && !list_cur_path[k].getPath().Contains(i))
                            {
                                Pa = ((Math.Pow(matrix[xk][i], a)) * (1.0 / Math.Pow(weightMatrix[xk][i], betta))) / tmp_pheromone_concentration;
                                Pa *= 100;
                                listPa.Add(new Element(i, Pa));
                            }
                        }
                        if (listPa.Count == 0)
                        { //Если идти больше некуда. Удаляем весь путь и выходим. И не будем его учитывать
                            list_cur_path[k].getPath().Clear();
                            list_cur_path[k].getMasVesov().Clear();
                            xk = 4;
                        }
                        else
                        {// Если дальнейший путь может быть продолжен
                            double[] temMas = new double[listPa.Count + 1];
                            double tmpVer = 0.0;
                            temMas[0] = tmpVer;

                            for (int i = 1; i < temMas.Length; i++)
                            {
                                tmpVer += listPa[i - 1].getVeroyatnost();
                                temMas[i] = tmpVer;
                            }
                            double tempRandom = Math.Random() * 100;

                            for (int i = 0; i < temMas.Length - 1; i++)
                            {
                                if (temMas[i] < tempRandom && temMas[i + 1] > tempRandom)
                                {
                                    list_cur_path[k].getMasVesov().Add(weightMatrix[xk][listPa[i].id]);
                                    xk = listPa[i].id;
                                    list_cur_path[k].getPath().Add(xk);
                                    i = temMas.Length - 1;
                                }
                            }
                        }
                        if (listPa.Count != 0) listPa.Clear();
                    } while (xk != 4);//Пока конечная вершина не достигнута
                    int j = 0;
                    Console.Write("Ant " + list_cur_path[k].getIdAnt() + "  path: " + "\t");
                    while (j < list_cur_path[k].getPath().Count)
                    {
                        if (list_cur_path[k].getPath().Count != 0)
                            Console.Write(list_cur_path[k].getPath()[j++] + "\t");
                    }
                    Console.WriteLine();
                }


                //Испарение феромона
                for (int i = 0; i < v; i++)
                    for (int j = 0; j < v; j++)
                        if (matrix[i][j] != -0.0)
                        {
                            double tmp = matrix[i][j];
                            matrix[i][j] = (1.0 - p) * tmp;
                        }



                //Увеличиваем концентрацию феромонов
                for (int k = 0; k < nk; k++)//Для каждого муравья
                {
                    if (list_cur_path[k].getPath().Count != 0)
                    {//Если путь муравья не пуст, то считаем, сколько надо добавить феромонов

                        double tempVesPath = 0;
                        for (int i = 0; i < list_cur_path[k].getMasVesov().Count; i++) // Идем по пути муравья и складываем веса всех дуг пути
                            tempVesPath += list_cur_path[k].getMasVesov()[i];

                        double countPheromone = (double)1.0 / tempVesPath;

                        int[] tmpMasVertix = new int[list_cur_path[k].getPath().Count];
                        for (int i = 0; i < tmpMasVertix.Length; i++)
                            tmpMasVertix[i] = list_cur_path[k].getPath()[i];
                        for (int i = 0; i < tmpMasVertix.Length - 1; i++)
                        {
                            int output = tmpMasVertix[i];
                            int input = tmpMasVertix[i + 1];
                            matrix[output][input] += countPheromone;
                            if (output!=input)matrix[input][output] += countPheromone;
            }
                }
    }




    t++;
            for(int i=0;i<list_cur_path.Count;i++){
                if(list_cur_path[i].getPath().Count != 0)
                    list_cur_path[i].getPath().Clear();
                if(list_cur_path[i].getMasVesov().Count != 0)
                    list_cur_path[i].getMasVesov().Clear();
}
                Console.WriteLine("t+1: " + t + "\n");
        }while(t<20);
        
    }

    private static void init(List<CurrentPath> list_cur_path)
{//Создается 10 муравьев
    CurrentPath path0 = new CurrentPath();
    path0.setIdAnt(0);
    List<int> pathlist0 = new List<int>();
    path0.setPath(pathlist0);
    List<double> masVesov0 = new List<double>();
    path0.setMasVesov(masVesov0);
    list_cur_path.Add(path0);


    CurrentPath path1 = new CurrentPath();
    path1.setIdAnt(1);
    List<int> pathlist1 = new List<int>();
    path1.setPath(pathlist1);
    List<double> masVesov1 = new List<double>();
    path1.setMasVesov(masVesov1);
    list_cur_path.Add(path1);

    CurrentPath path2 = new CurrentPath();
    path2.setIdAnt(2);
    List<int> pathlist2 = new List<int>();
    path2.setPath(pathlist2);
    List<double> masVesov2 = new List<double>();
    path2.setMasVesov(masVesov2);
    list_cur_path.Add(path2);

    CurrentPath path3 = new CurrentPath();
    path3.setIdAnt(3);
    List<int> pathlist3 = new List<int>();
    path3.setPath(pathlist3);
    List<double> masVesov3 = new List<double>();
    path3.setMasVesov(masVesov3);
    list_cur_path.Add(path3);

    ///////////////////////////////////////////////
    CurrentPath path4 = new CurrentPath();
    path4.setIdAnt(4);
    List<int> pathlist4 = new List<int>();
    path4.setPath(pathlist4);
    List<double> masVesov4 = new List<double>();
    path4.setMasVesov(masVesov4);
    list_cur_path.Add(path4);

    CurrentPath path5 = new CurrentPath();
    path5.setIdAnt(5);
    List<int> pathlist5 = new List<int>();
    path5.setPath(pathlist5);
    List<double> masVesov5 = new List<double>();
    path5.setMasVesov(masVesov5);
    list_cur_path.Add(path5);

    CurrentPath path6 = new CurrentPath();
    path6.setIdAnt(6);
    List<int> pathlist6 = new List<int>();
    path6.setPath(pathlist6);
    List<double> masVesov6 = new List<double>();
    path6.setMasVesov(masVesov6);
    list_cur_path.Add(path6);

    CurrentPath path7 = new CurrentPath();
    path7.setIdAnt(7);
    List<int> pathlist7 = new List<int>();
    path7.setPath(pathlist7);
    List<double> masVesov7 = new List<double>();
    path7.setMasVesov(masVesov7);
    list_cur_path.Add(path7);


    CurrentPath path8 = new CurrentPath();
    path8.setIdAnt(8);
    List<int> pathlist8 = new List<int>();
    path8.setPath(pathlist8);
    List<double> masVesov8 = new List<double>();
    path8.setMasVesov(masVesov8);
    list_cur_path.Add(path8);

    CurrentPath path9 = new CurrentPath();
    path9.setIdAnt(9);
    List<int> pathlist9 = new List<int>();
    path9.setPath(pathlist9);
    List<double> masVesov9 = new List<double>();
    path9.setMasVesov(masVesov9);
    list_cur_path.Add(path9);

    CurrentPath path10 = new CurrentPath();
    path10.setIdAnt(10);
    List<int> pathlist10 = new List<int>();
    path10.setPath(pathlist10);
    List<double> masVesov10 = new List<double>();
    path10.setMasVesov(masVesov10);
    list_cur_path.Add(path10);





    CurrentPath path11 = new CurrentPath();
    path11.setIdAnt(11);
    List<int> pathlist11 = new List<>();
    path11.setPath(pathlist11);
    List<double> masVesov11 = new List<>();
    path11.setMasVesov(masVesov11);
    list_cur_path.Add(path11);

    CurrentPath path12 = new CurrentPath();
    path12.setIdAnt(12);
    List<int> pathlist12 = new List<>();
    path12.setPath(pathlist12);
    List<double> masVesov12 = new List<>();
    path12.setMasVesov(masVesov12);
    list_cur_path.Add(path12);

    CurrentPath path13 = new CurrentPath();
    path13.setIdAnt(13);
    List<int> pathlist13 = new List<>();
    path13.setPath(pathlist13);
    List<double> masVesov13 = new List<>();
    path13.setMasVesov(masVesov13);
    list_cur_path.Add(path13);

    ///////////////////////////////////////////////
    CurrentPath path14 = new CurrentPath();
    path14.setIdAnt(14);
    List<int> pathlist14 = new List<>();
    path14.setPath(pathlist14);
    List<double> masVesov14 = new List<>();
    path14.setMasVesov(masVesov14);
    list_cur_path.Add(path14);

    CurrentPath path15 = new CurrentPath();
    path15.setIdAnt(15);
    List<int> pathlist15 = new List<>();
    path15.setPath(pathlist15);
    List<double> masVesov15 = new List<>();
    path15.setMasVesov(masVesov15);
    list_cur_path.Add(path15);

    CurrentPath path16 = new CurrentPath();
    path16.setIdAnt(16);
    List<int> pathlist16 = new List<>();
    path16.setPath(pathlist16);
    List<double> masVesov16 = new List<>();
    path16.setMasVesov(masVesov16);
    list_cur_path.Add(path16);

    CurrentPath path17 = new CurrentPath();
    path17.setIdAnt(17);
    List<int> pathlist17 = new List<>();
    path17.setPath(pathlist17);
    List<double> masVesov17 = new List<>();
    path17.setMasVesov(masVesov17);
    list_cur_path.Add(path17);


    CurrentPath path18 = new CurrentPath();
    path18.setIdAnt(18);
    List<int> pathlist18 = new List<>();
    path18.setPath(pathlist18);
    List<double> masVesov18 = new List<>();
    path18.setMasVesov(masVesov18);
    list_cur_path.Add(path18);

    CurrentPath path19 = new CurrentPath();
    path19.setIdAnt(19);
    List<int> pathlist19 = new List<>();
    path19.setPath(pathlist19);
    List<double> masVesov19 = new List<>();
    path19.setMasVesov(masVesov19);
    list_cur_path.Add(path19);
}

private static void initMatrix(double[][] matrix, int v)
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
                matrix[i][j] = 2.0;
                matrix[j][i] = 2.0;
            }


    System.out.println("Матрица Феромонов на дугах:");
    for (int i = 0; i < v; i++)
        for (int j = 0; j < v; j++)
            if (matrix[i][j] == 0.0) matrix[i][j] = -0.0;
    for (int i = 0; i < v; i++)
    {
        for (int j = 0; j < v; j++)
        {
            System.out.print(matrix[i][j] + "\t");
        }
        System.out.println();
    }

    System.out.println();

}

private static void initWeightMatrix(double[][] weightMatrix, int v)
{
    for (int i = 0; i < v; i++)
        for (int j = 0; j < v; j++)
            if ((i == 0 && j == 1))
            {
                weightMatrix[i][j] = 16.0;
                weightMatrix[j][i] = 16.0;
            }
            else if ((i == 0 && j == 3))
            {
                weightMatrix[i][j] = 8.0;
                weightMatrix[j][i] = 8.0;
            }
            else if ((i == 0 && j == 5))
            {
                weightMatrix[i][j] = 20.0;
                weightMatrix[j][i] = 20.0;
            }

            else if ((i == 1 && j == 2))
            {
                weightMatrix[i][j] = 2.0;
                weightMatrix[j][i] = 2.0;
            }
            else if ((i == 1 && j == 3))
            {
                weightMatrix[i][j] = 10.0;
                weightMatrix[j][i] = 10.0;
            }

            else if ((i == 2 && j == 4))
            {
                weightMatrix[i][j] = 7.0;
                weightMatrix[j][i] = 7.0;
            }
            else if ((i == 2 && j == 3))
            {
                weightMatrix[i][j] = 40.0;
                weightMatrix[j][i] = 40.0;
            }


            else if ((i == 3 && j == 4))
            {
                weightMatrix[i][j] = 50.0;
                weightMatrix[j][i] = 50.0;
            }
            else if ((i == 3 && j == 5))
            {
                weightMatrix[i][j] = 20.0;
                weightMatrix[j][i] = 20.0;
            }
            else if ((i == 3 && j == 6))
            {
                weightMatrix[i][j] = 15.0;
                weightMatrix[j][i] = 15.0;
            }


            else if ((i == 4 && j == 6))
            {
                weightMatrix[i][j] = 2.0;
                weightMatrix[j][i] = 2.0;
            }

            else if ((i == 5 && j == 6))
            {
                weightMatrix[i][j] = 5.0;
                weightMatrix[j][i] = 5.0;
            }
    for (int i = 0; i < v; i++)
        for (int j = 0; j < v; j++)
            if (weightMatrix[i][j] == 0.0) weightMatrix[i][j] = -0.0;

}
}
}
