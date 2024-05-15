using AntColonyOptimization.Viz;
using MahApps.Metro.Controls;
using System.Windows;

namespace AntColonyOptimization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        TspSolverViz _Algo = new TspSolverViz();
        private void OnStart(object sender, RoutedEventArgs e)
        {
            mRenderCtrl.ClearAll();

            _Algo.RandomCities(mRenderCtrl);

            mRenderCtrl.RequestDraw();

            mRenderCtrl.ZoomAll();

            mStatusCtrl.Content = $"Ready";
        }
        private void OnRun(object sender, RoutedEventArgs e)
        {
            _Algo.Compute();
            _Algo.ShowTrails(mRenderCtrl);
            _Algo.ShowPath(mRenderCtrl);

            mStatusCtrl.Content = $"Total：{_Algo.Fitness}  Iteration：{_Algo.SimulateCount}";
        }
        
        int _CurTimer = 0;
        private void OnRunByStep(object sender, RoutedEventArgs e)
        {
            int nIteration = 0;
            _CurTimer = 0;
            double time = 0;
            bool running = false;
            TSP.EnumCode lastCode = TSP.EnumCode.Continue;
            mRenderCtrl.SetAnimationCallback((float t) =>
            {                
                if(running)
                {
                    running = _Algo.ShowAnts((t - time)*10);
                }
                else
                {
                    if(lastCode == TSP.EnumCode.NewGeneration)
                    {
                        _Algo.AntColony.Reset();

                    }
                    lastCode = _Algo.Next(_CurTimer++);
                    if (lastCode == TSP.EnumCode.Finished)
                    {
                        mRenderCtrl.EnableAnimation(false);
                        _Algo.ShowPath(mRenderCtrl);
                    }
                    else if(lastCode == TSP.EnumCode.NewGeneration)
                    {
                        ++nIteration;
                        running = true;
                    }
                    time = t;
                    _Algo.ShowTrails(mRenderCtrl);
                    mStatusCtrl.Content = $"Generation: {nIteration}，Shortest Distance: { Math.Ceiling(_Algo.Fitness)}";
                }

                mRenderCtrl.RequestDraw();
            });

            _Algo.Start();
            mRenderCtrl.EnableAnimation(true);


        }

        private void mRenderCtrl_ViewerReady()
        {
            mRenderCtrl.SetBackgroundColor(200/255.0f, 200 / 255.0f, 200 / 255.0f, 1);
        }
    }
}