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
    public partial class MainWindow : Window
    {

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var scvm = new StringCalculatorViewModel(calcGraph.Height, calcGraph.Width);
            void SetDataContext()
            {
                calcGraph.Children.Add(scvm.XaxisPath);
                calcGraph.Children.Add(scvm.YaxisPath);
                calcGraph.Children.Add(scvm.DatasetPoly);
            }
            SetDataContext();
            InitializeComponent();
        }
    }
}
