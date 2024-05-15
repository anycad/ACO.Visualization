namespace AntColonyOptimization.ACO
{
    internal class AntColony
    {
        List<Ant> _AntList = new List<Ant>();
        PheroTable _PheroTable;

        public PheroTable PheroTable { get => _PheroTable; }
        public List<Ant> Ants { get => _AntList; }

        public Config Config { get => PheroTable.Config; }

        public AntColony(Config config)
        {
            _PheroTable = new PheroTable(config);
        }

        public void Initialize()
        {
            _AntList.Clear();

            var config = _PheroTable.Config;
            // ants
            int to = 0;
            for (int ii = 0; ii < config.AntCount; ii++)
            {
                if (to == config.TargetCount)
                    to = 0;
                _AntList.Add(new Ant(to++));
            }

            _PheroTable.Initialize();
        }

        public void Reset()
        {
            var config = _PheroTable.Config;

            int to = 0;

            for (int ii = 0; ii < config.AntCount; ii++)
            {
                if (to == config.TargetCount)
                    to = 0;

                var ant = _AntList[ii];
                ant.Reset(to++);
            }
        }


        public void UpdateTrails()
        {
            _PheroTable.UpdateTrails(Ants);
        }


        public int Move()
        {
            var config = Config;

            int moving = 0;

            foreach (var ant in Ants)
            {
                if (ant.Count < config.TargetCount)
                {
                    var nextId = PheroTable.SelectNextTarget(ant);
                    ant.MoveTo(nextId);
                    moving++; 
                }
            }

            return moving;
        }
    }
}
