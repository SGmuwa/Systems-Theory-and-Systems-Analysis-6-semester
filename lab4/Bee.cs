using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4
{
    class Bee
    {
        //Координаты центральной точки области
        int x;
        int y;
        double valueFunc;
        int pointCountBest;// Определяет сколько туда пчёл в дальнейшем послать


        public Bee(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void setPointCountBest(int pointCountBest)
        {
            this.pointCountBest = pointCountBest;
        }


        public void setValueFunc(double valueFunc)
        {
            this.valueFunc = valueFunc;
        }

        public double amount_nectar()
        {
            return -(Math.pow(x, 2) + Math.pow(y, 2));
            //return Math.sin(x)*y *Math.sin(y);
        }
        public double distance(int a, int b)
        {//Евклидово расстояние между точкой Y(x,y) и Y(a,b) 
            return Math.sqrt(Math.pow((x - a), 2) + Math.pow((y - b), 2));
        }

        public double getValueFunc()
        {
            return valueFunc;
        }
    }
}
