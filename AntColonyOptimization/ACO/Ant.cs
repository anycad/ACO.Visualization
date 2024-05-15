namespace AntColonyOptimization.ACO
{
    internal class Ant
    {
        public int TargetId { get => Trajectory[Trajectory.Count-1]; }
        public List<int> Trajectory { get; set; } = new List<int>();
        public int Count { get => Trajectory.Count; }
        public double Fitness { get; set; } = 0;

        public Ant(int target)
        {
            Reset(target);
        }

        public void Reset(int target)
        {
            Fitness = 0.0;
            Trajectory.Clear();
            Trajectory.Add(target);
        }

        public void MoveTo(int target)
        {
            Trajectory.Add(target);
        }

        public bool Contains(int target)
        {            
            return Trajectory.Contains(target);
        }

        
    }
}
