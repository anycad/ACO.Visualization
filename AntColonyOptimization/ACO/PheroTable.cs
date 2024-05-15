namespace AntColonyOptimization.ACO
{

    internal class PheroTable
    {
        Dictionary<int, double> _Table = new ();
        Config _Config;
        Random _Random = new Random();

        public Config Config { get { return _Config; } }

        public PheroTable(Config config)
        {
            _Config = config;
        }

        public void Initialize()
        {
            _Table.Clear();

            for (int ii = 0; ii < _Config.TargetCount - 1; ii++)
            {
                for (int jj = ii+1; jj < _Config.TargetCount; jj++)
                {
                    var idx = ii * _Config.TargetCount + jj;

                    _Table.Add(idx, _Config.InitPHER);
                }
            }
        }

        void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
        public double GetPhero(int from, int to)
        {
            if (to < from)
            {
                Swap(ref from, ref to);
            }
            var idx = from * _Config.TargetCount + to;

            return _Table[idx];
        }

        void SetPhero(int from, int to, double phero)
        {
            if (to < from)
            {
                Swap(ref from, ref to);
            }
            var idx = from * _Config.TargetCount + to;
            _Table[idx] = phero;
        }


        public void UpdateTrails(List<Ant> ants)
        {
            var cityCount = _Config.TargetCount;

            for (int from = 0; from < cityCount-1; from++)
            {
                for (int to = from+1; to < cityCount; to++)
                {
                    var phero = GetPhero(from, to);
                    phero *= (1.0 - _Config.RHO);
                    if (phero < 0.0)
                    {
                        phero = _Config.InitPHER;
                    }
                    SetPhero(from, to, phero);
                }
            }

            for (int aa = 0; aa < ants.Count; aa++)
            {
                var ant = ants[aa];
                for (int idx = 0; idx < ant.Trajectory.Count - 1; idx++)
                {
                    int from = ant.Trajectory[idx];
                    int to = ant.Trajectory[idx + 1];

                    var phero = GetPhero(from, to);
                    phero += (_Config.QVAL / ant.Fitness);
                    SetPhero(from, to, phero);
                }
            }

            for (int from = 0; from < _Config.TargetCount-1; from++)
            {
                for (int to = from+1; to < _Config.TargetCount; to++)
                {
                    var phero = GetPhero(from, to);
                    phero *= _Config.RHO;
                    SetPhero(from, to, phero);
                }
            }

        }

        public int SelectNextTarget(Ant ant)
        {
            var config = Config;
            var probability = config.Probability;
            int to;
            double sum = 0.0;

            int from = ant.TargetId; 

            for (to = 0; to < config.TargetCount; to++)
            {
                if (from != to && !ant.Contains(to))
                {
                    sum += probability.Compute(from, to, this);
                }
            }

            do
            {
                double p;
                to++; 

                if (to >= config.TargetCount)
                    to = 0; 
                if (!ant.Contains(to))
                {
                    p = probability.Compute(from, to, this) / sum;

                    double x = _Random.NextDouble();
                    if (x < p)
                    {
                        break;
                    }
                }
            } while (true);

            return to;
        }
    }
}
