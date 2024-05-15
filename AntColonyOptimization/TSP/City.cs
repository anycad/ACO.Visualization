using AnyCAD.Foundation;

namespace AntColonyOptimization.TSP
{
    internal class City
    {
        public GPnt2d Location { get; set; } = new GPnt2d();

        public City(double x, double y)
        {
            Location.x = x;
            Location.y = y;
        }

        public Vector3 GetPosition(double z) { return new Vector3(Location.x, Location.y, z); }
        public void ShowCity(IRenderView view)
        {
            var node = PrimitiveSceneNode.Create(GeometryBuilder.AtomSphere(), MaterialStore.Gold);
            node.SetTransform(Matrix4.makeTranslate(Location.x, Location.y, 1));
            view.ShowSceneNode(node);
        }
    }
}
