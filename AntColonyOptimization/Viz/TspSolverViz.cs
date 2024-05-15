using AntColonyOptimization.ACO;
using AntColonyOptimization.TSP;
using AnyCAD.Foundation;

namespace AntColonyOptimization.Viz
{
    internal class TspSolverViz
        : TspSolver
    {

        public AntColony AntColony { get => _AntColony; }

        PrimitiveSceneNode _AntActors;
        SegmentsSceneNode _AllPathActors;
        SegmentsSceneNode _TrailsActors;
        ColorLookupTable _LookupTable = new ColorLookupTable();
        PaletteWidget _PlatteWidget = new PaletteWidget();
        public TspSolverViz()
        {
            var count = (uint)(_Config.TargetCount * _Config.TargetCount);
            _AllPathActors = new SegmentsSceneNode(count, ColorTable.DarkGray, 1);
            _AllPathActors.SetPickable(false);

            _TrailsActors = new SegmentsSceneNode(count, ColorTable.LightGray, 1);
            _TrailsActors.SetPickable(false);

            var ptmaterial = PointsMaterial.Create("ant.point");
            ptmaterial.SetPointStyle(EnumPointStyle.Plus);
            ptmaterial.SetPointSize(10);
            ptmaterial.SetColor(ColorTable.Black);
            _AntActors = PrimitiveSceneNode.Create(GeometryBuilder.AtomPoint(), ptmaterial);

            _LookupTable.SetColorMap(ColorMapKeyword.Create(EnumSystemColorMap.Rainbow));
            _LookupTable.SetMaxValue(4);
            _LookupTable.SetMinValue(-1);


            _PlatteWidget.SetTitle("Phero");
            _PlatteWidget.SetTextColor(ColorTable.Black);
            _PlatteWidget.SetPosition(20, 20);
            _PlatteWidget.Update(_LookupTable);
        }

        public void RandomCities(IRenderView view)
        {
            var cityList = _CityMap.CityList;
            cityList.Clear();

            Random random = new Random();
            for (int ii = 0; ii < _Config.TargetCount; ii++)
            {
                int x = random.Next(0, _CityMap.MaxDistance);
                int y = random.Next(0, _CityMap.MaxDistance);
                var city = new City(x, y);
                city.ShowCity(view);
                cityList.Add(city);
            }
            _CityMap.Initialize();

            var lineOffset = cityList.Count;
            for (int ii = 0; ii < cityList.Count; ii++)
            {
                var start = cityList[ii].GetPosition(-2);
                for (int jj = ii + 1; jj < cityList.Count; jj++)
                {
                    var idx = (uint)(ii * lineOffset + jj);
                    _AllPathActors.SetPositions(idx, start, cityList[jj].GetPosition(-2));
                }
            }


            _AllPathActors.Update();
            view.ShowSceneNode(_AllPathActors);
            view.ShowSceneNode(_AntActors);

            _TrailsActors.SetVisible(false);
            view.ShowSceneNode(_TrailsActors);
            view.ShowSceneNode(_PlatteWidget);
        }

        protected override void Initialize()
        {
            base.Initialize();

            _BestPathNode?.SetVisible(false);

            for (int ii = 0; ii < _Config.TargetCount; ii++)
            {
                for (int jj = ii + 1; jj < _Config.TargetCount; jj++)
                {
                    int idx = ii * _Config.TargetCount + jj;
                    _TrailsActors.SetPositions((uint)idx, Vector3.Zero, Vector3.Zero);
                }
            }
            _TrailsActors.Update();
        }


        PrimitiveSceneNode? _BestPathNode;
        public void ShowPath(IRenderView view)
        {
            Float32Buffer points = new Float32Buffer(0);
            points.Reserve((uint)_BestPath.Count * 3);
            for (int ii = 0; ii < _BestPath.Count; ii++)
            {
                var pt = _CityMap.GetCity(_BestPath[ii]).Location;

                points.Append((float)pt.x, (float)pt.y, 0);
            }

            var lines = GeometryBuilder.CreateLines(new Float32Array(points), null, null);
            lines.SetPrimitiveType(EnumPrimitiveType.LINE_STRIP);

            if (null == _BestPathNode)
            {
                var material = LineMaterial.Create("path.line");
                material.SetColor(ColorTable.Red);
                material.SetLineWidth(3);
                _BestPathNode = PrimitiveSceneNode.Create(lines, material);

            }
            else
                _BestPathNode.SetGeometry(lines);
            _BestPathNode.SetVisible(true);

            view.ShowSceneNode(_BestPathNode);
        }


        public void ShowTrails(IRenderView view)
        {
            var table = _AntColony.PheroTable;
            for (int ii = 0; ii < _Config.TargetCount; ii++)
            {
                var start = _CityMap.GetCity(ii).GetPosition(-1);
                for (int jj = ii + 1; jj < _Config.TargetCount; jj++)
                {
                    var val = table.GetPhero(ii, jj);
                    if (val < 0.1)
                        continue;

                    var color = _LookupTable.GetColor(val);
                    int idx = ii * _Config.TargetCount + jj;
                    _TrailsActors.SetColors((uint)idx, color, color);
                    _TrailsActors.SetPositions((uint)idx, start, _CityMap.GetCity(jj).GetPosition(-2));
                }
            }
            _TrailsActors.Update();
            _TrailsActors.SetVisible(true);
        }

        public bool ShowAnts(double distance)
        {
            Float32Buffer buffer = new Float32Buffer(0);
            bool running = true;
            uint ii = 0;
            foreach(var ant in _AntColony.Ants)
            {
                var pt = _CityMap.ComputePointByDistance(ant.Trajectory, distance);
                if (pt != null)
                {
                    buffer.Append3(new Vector3(pt.x, pt.y, 2));
                }
                else
                {
                    running = false;
                }

                ++ii;
            }

            if(running)
            {
                _AntActors.SetGeometry(GeometryBuilder.CreateGeometry(EnumPrimitiveType.POINTS, buffer, null, null, null));
                _AntActors.RequestUpdate();
            }

            return running;
        }
    }
}
