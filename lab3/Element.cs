namespace lab3
{
    public class Element
    {
        public Element(int id, double veroyatnost)
        {
            Id = id;
            Veroyatnost = veroyatnost;
        }

        /// <summary>
        /// Возможная следующая вершина
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Вероятность попасть в эту вершину
        /// </summary>
        public double Veroyatnost { get; set; }
    }
}
