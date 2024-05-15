using AntColonyOptimization.ACO;

namespace AntColonyOptimization.TSP
{
    internal class CityProbabilityCalculator : IProbabilityCalculator
    {
        CityMap _CityMap;
        public CityProbabilityCalculator(CityMap cityMap)
        {
            _CityMap = cityMap;
        }

        double SelectProbability(double phero, double distance, Config config)
        {
            return Math.Pow(phero, config.Alpha) * Math.Pow(1.0 / distance, config.Beta);
        }
        public double Compute(int from, int to, PheroTable table)
        {
            return SelectProbability(table.GetPhero(from, to), _CityMap.GetDistance(from, to), table.Config);
        }
    }
}
