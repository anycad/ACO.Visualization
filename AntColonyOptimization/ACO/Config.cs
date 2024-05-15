namespace AntColonyOptimization.ACO
{
    internal class Config
    {
        public double Alpha { get; set; } = 1.0;
        public double Beta { get; set; } = 5.0;
        public double RHO { get; set; } = 0.5;

        public double QVAL { get; set; } = 100;

        public int MaxCount { get; set; } = 20;

        public int TargetCount { get; set; } = 30;

        public int AntCount { get; set; } = 30;

        public double MaxTime { get => MaxCount * TargetCount; }

        public double InitPHER { get => 1.0 / TargetCount;  }

        public IProbabilityCalculator Probability;

        public Config(IProbabilityCalculator probability)
        {
            Probability = probability;
        }

    }
}
