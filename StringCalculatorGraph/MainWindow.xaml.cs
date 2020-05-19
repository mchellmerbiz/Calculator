using StringCalculatorGraph.Models;
using StringCalculatorGraph.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
        StringCalculatorViewModel Scvm;
        private BackgroundWorker _bw = new BackgroundWorker();
        private bool _evalCalled = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Scvm = new StringCalculatorViewModel(calcGraph.Height, calcGraph.Width);
            void SetDataContext()
            {
                calcGraph.Children.Add(Scvm.XaxisPath);
                calcGraph.Children.Add(Scvm.YaxisPath);
                calcGraph.Children.Add(Scvm.DatasetPoly);
            }
            SetDataContext();
            InitializeComponent();
            _bw = new BackgroundWorker();
            _bw.DoWork += new DoWorkEventHandler(_bw_DoWork);
            _bw.RunWorkerAsync();
            this.DataContext = Scvm;
        }

        private void Window_Updated(object sender, RoutedEventArgs e)
        {
            _evalCalled = true;
        }

        void _bw_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (_evalCalled)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate {
                        Scvm.UpdateData();
                        calcGraph.Children.Clear();
                        calcGraph.Children.Add(Scvm.XaxisPath);
                        calcGraph.Children.Add(Scvm.YaxisPath);
                        calcGraph.Children.Add(Scvm.DatasetPoly);
                    });
                    Console.WriteLine("Updating Canvas");
                    _evalCalled = false;
                }
            }
        }
    }
}
