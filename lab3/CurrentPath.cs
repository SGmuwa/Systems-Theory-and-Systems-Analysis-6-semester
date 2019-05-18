using System.Collections.Generic;

namespace lab3
{
    public class CurrentPath
    {
        int idAnt;
        List<int> path;
        List<double> masVesov;

        public List<double> getMasVesov()
        {
            return masVesov;
        }

        public void setMasVesov(List<double> masVesov)
        {
            this.masVesov = masVesov;
        }


        public void createListPath(int idAnt)
        {
            this.path.Add(idAnt);
        }

        public int getIdAnt()
        {
            return idAnt;
        }

        public void setIdAnt(int idAnt)
        {
            this.idAnt = idAnt;
        }

        public List<int> getPath()
        {
            return path;
        }

        public void setPath(List<int> path)
        {
            this.path = path;
        }
    }

}
