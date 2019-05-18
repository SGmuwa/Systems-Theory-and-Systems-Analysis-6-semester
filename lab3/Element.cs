namespace lab3
{
    public class Element
    {
        int id; //Возможная следующая вершина
        double veroyatnost; //Вероятность попасть в эту вершину

        public Element(int id, double veroyatnost)
        {
            this.id = id;
            this.veroyatnost = veroyatnost;
        }

        public int getId()
        {
            return id;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public double getVeroyatnost()
        {
            return veroyatnost;
        }

        public void setVeroyatnost(double veroyatnost)
        {
            this.veroyatnost = veroyatnost;
        }
    }
}
