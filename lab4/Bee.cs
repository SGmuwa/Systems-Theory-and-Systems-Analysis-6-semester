using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4
{
    class Bee : IComparable, IComparable<Bee>
    {
        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// Определяет сколько туда пчёл в дальнейшем послать
        /// </summary>
        public int PointCountBest { get; set; }

        public double ValueFunc { get; set; }

        public Bee(int X, int Y, int pointCountBest = 0, double valueFunc = 0)
        {
            this.X = X;
            this.Y = Y;
            PointCountBest = pointCountBest;
            ValueFunc = valueFunc;
        }

        public double AmountNectar()
            => -(Math.Pow(X, 2) + Math.Pow(Y, 2));

        public double Distance(int a, int b)
        {//Евклидово расстояние между точкой Y(x,y) и Y(a,b) 
            return Math.Sqrt(Math.Pow(X - a, 2) + Math.Pow(Y - b, 2));
        }


        public int CompareTo(Bee other)
        {
            if (other == null)
                return 1;
            if (ValueFunc== other.ValueFunc) return 0;
            else if (ValueFunc< other.ValueFunc) return -1;
            else return 1;
        }

        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is Bee))
                return 1;
            else
                return CompareTo((Bee)obj);
        }

        public override string ToString()
            => "X: " + X + ", Y: " + Y + ", F: " + ValueFunc;
    }
}
