using System.Collections.Generic;

namespace lab3
{
    public class CurrentPath
    {
        public CurrentPath(int idAnt)
            => IdAnt = idAnt;

        public List<double> MasVesov { get; set; } = new List<double>();

        public int IdAnt { get; set; }

        public List<int> Path { get; set; } = new List<int>();
    }

}
