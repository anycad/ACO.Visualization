using AnyCAD.Foundation;

namespace AntColonyOptimization.TSP
{
    internal class CityMap
    {
        Dictionary<int, double> _Table = new();
        List<City> _CityList = new List<City>();

        public int MaxDistance = 50;
        public List<City> CityList { get { return _CityList; } }
        public CityMap()
        {

        }

        public City GetCity(int idx) { return _CityList[idx]; }
        public void Initialize()
        {
            _Table.Clear();

            for (int ii = 0; ii < _CityList.Count - 1; ii++)
            {
                for (int jj = ii + 1; jj < _CityList.Count; jj++)
                {
                    var idx = ii * _CityList.Count + jj;

                    var dist = _CityList[ii].Location.Distance(_CityList[jj].Location) + 0.1;

                    _Table.Add(idx, dist);
                }
            }
        }


        void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
        public double GetDistance(int from, int to)
        {
            if (to < from)
            {
                Swap(ref from, ref to);
            }
            var idx = from * _CityList.Count + to;
            return _Table[idx];
        }

        public double ComputeDistance(List<int> trajectory)
        {
            double dist = 0;
            for (int ii = 0; ii < trajectory.Count - 1; ++ii)
            {
                dist += GetDistance(trajectory[ii], trajectory[ii + 1]);
            }
            return dist;
        }

        public GXY? ComputePointByDistance(List<int> trajectory, double distance)
        {
            double dist = 0;

            var c1 = _CityList[trajectory[0]];

            for (int ii=1; ii < trajectory.Count; ++ii)
            {
                var c2 = _CityList[trajectory[ii]];
                var d = c2.Location.Distance(c1.Location);
                if (d == 0 || (dist + d) < distance)
                {
                    dist += d;
                    c1 = c2;
                }
                else
                {
                    var dir = new GDir2d(new GVec2d(c1.Location, c2.Location));
                    return c1.Location.XY().Added(dir.XY().Multiplied(distance - dist));
                }
            }            

            return null;
        }
    }
}
