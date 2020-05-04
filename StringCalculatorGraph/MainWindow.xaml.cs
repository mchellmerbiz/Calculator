using StringCalculatorGraph.Models;
using StringCalculatorGraph.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StringCalculatorGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            void SetDataContext()
            {
                var calcString = "x+1";
                var calcStart = 0;
                var calcEnd = 100;
                var calcInterval = 10;

                var vg = new GraphVariableGenerator();
                var indVars = vg.GenerateIndependentVariables(calcStart, calcEnd, calcInterval);
                var depVars = new List<double>();

                //x + 1
                foreach (var indVar in indVars)
                {
                    depVars.Add(indVar + 1);
                }
                var Points = new List<Point>();
                for (int i = 0; i < indVars.Count; i++)
                {
                    var np = new Point() { X = indVars[i], Y = depVars[i] };
                    Points.Add(np);
                }

                DataContext = new StringCalculatorViewModel
                {
                    Segments = new List<Segment>(Points.Zip(Points.Skip(1), (a, b) => new Segment { From = a, To = b }))
                };
            }
            SetDataContext();
            InitializeComponent();
        }
    }
}
