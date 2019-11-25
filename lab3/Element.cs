namespace lab3
{
    public class Element
    {
        public Element(int id, double probability)
        {
            Id = id;
            Probability = probability;
        }

        /// <summary>
        /// Возможная следующая вершина.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Вероятность попасть в эту вершину.
        /// </summary>
        public double Probability { get; set; }
    }
}
