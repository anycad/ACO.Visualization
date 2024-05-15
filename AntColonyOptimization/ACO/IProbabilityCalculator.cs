namespace AntColonyOptimization.ACO
{
    internal interface IProbabilityCalculator
    {
        double Compute(int from, int to, PheroTable table);
    }
}
