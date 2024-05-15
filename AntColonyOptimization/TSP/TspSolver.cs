using AntColonyOptimization.ACO;

namespace AntColonyOptimization.TSP
{
    internal enum EnumCode
    {
        Continue,
        NewGeneration,
        Finished
    }

    internal class TspSolver
    {
        protected Config _Config;
        protected CityMap _CityMap = new CityMap();
        protected AntColony _AntColony;

        protected double _Fitness = 0;
        protected List<int> _BestPath = new List<int>();

        public TspSolver()
        {
            _Config = new Config(new CityProbabilityCalculator(_CityMap));
            _AntColony = new AntColony(_Config);
        }

        public double Fitness { get { return _Fitness; } }
        public List<int> Result { get => _BestPath; }


        protected virtual void Initialize()
        {
            _Fitness = _CityMap.MaxDistance * _Config.TargetCount;
            _AntColony.Initialize();
        }

        public int SimulateCount { get; private set; } = 0;
        public void Compute()
        {
            Start();
            int curTime = 0;

            do
            {
                var code = Next(curTime++);
                if (code == EnumCode.Finished)
                {
                    break;
                }
                else if(code == EnumCode.NewGeneration)
                {
                    _AntColony.Reset();
                }
                        
            } while (true);
            
            SimulateCount = curTime;
        }

        public void Start()
        {
            SimulateCount = 0;
            Initialize();
        }


        public void ComputeFitness()
        {
            var ants = _AntColony.Ants;
            var bestIndex = -1;
            for (int ii = 0; ii < ants.Count; ii++)
            {
                var ant = ants[ii];
                ant.Fitness = _CityMap.ComputeDistance(ant.Trajectory);
                if (ant.Fitness < _Fitness)
                {
                    _Fitness = ant.Fitness;
                    bestIndex = ii;
                }
            }

            if (bestIndex > -1)
            {
                _BestPath.Clear();
                _BestPath.AddRange(ants[bestIndex].Trajectory);
            }

        }

        public EnumCode Next(int curTime)
        {
            EnumCode end = EnumCode.Continue;
            if (_AntColony.Move() == 0)
            {
                ComputeFitness();

                _AntColony.UpdateTrails();

                end = EnumCode.NewGeneration;
            }
            return curTime < _Config.MaxTime ? end : EnumCode.Finished;
        }
    }
}
