using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab1
{
    public static class CollectionWriter
    {
        public static string ToString<T>(this IEnumerable<T> l, string separator)
        {
            return "[" + string.Join(separator, l.Select(i => i.ToString()).ToArray()) + "]";
        }
        public static string ToString<T>(this T[,] l, string separatorX = ", ", string separatorY = "\n")
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < l.GetLength(1); y++)
            {
                if (y != 0)
                    sb.Append(separatorY);
                for (int x = 0; x < l.GetLength(0); x++)
                {
                    if (x != 0)
                        sb.Append(separatorX);
                    sb.Append(l[x, y]);
                }
            }
            return sb.ToString();
        }
    }
}
