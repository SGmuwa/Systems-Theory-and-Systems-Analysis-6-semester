using System;

namespace lab4
{
    class Bee : IComparable, IComparable<Bee>
    {
        private int _x;
        private int _y;
        public int X
        {
            get => _x; set
            {
                _x = value;
                AmountNectar = AmountNectarFunction(X, Y);
            }
        }
        public int Y
        {
            get => _y; set
            {
                _y = value;
                AmountNectar = AmountNectarFunction(X, Y);
            }
        }

        /// <summary>
        /// Определяет сколько туда пчёл в дальнейшем послать.
        /// </summary>
        public int PointCountBest { get; set; }

        public Bee(int X, int Y, int pointCountBest = 0)
        {
            this._x = X;
            this.Y = Y;
            PointCountBest = pointCountBest;
        }

        /// <summary>
        /// Получение количества нектара в точке.
        /// </summary>
        public double AmountNectar { get; private set; }
        
        private double AmountNectarFunction(double X, double Y)
            => -(Math.Pow(X, 2) + Math.Pow(Y, 2));

        /// <summary>
        /// Евклидово расстояние между точкой Y(x,y) и Y(a,b).
        /// </summary>
        public double Distance(int a, int b)
            => Math.Sqrt(Math.Pow(X - a, 2) + Math.Pow(Y - b, 2));

        public int CompareTo(Bee other)
        {
            if (other == null)
                return 1;
            if (AmountNectar == other.AmountNectar) return 0;
            else if (AmountNectar < other.AmountNectar) return -1;
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
            => $"({X, 3}, {Y, 3}) = {AmountNectar}";
    }
}
