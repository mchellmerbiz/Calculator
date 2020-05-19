using StringCalculatorGraph.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StringCalculatorGraph.ViewModels
{
    public class StringCalculatorViewModel : INotifyPropertyChanged
    {
        private string _expression;
        public string Expression
        {
            get { return _expression; }
            set
            {
                _expression = value;
                OnPropertyChanged("Expression");
            }
        }

        public double _expressionStart;
        public double ExpressionStart
        {
            get { return _expressionStart; }
            set
            {
                _expressionStart = value;
                OnPropertyChanged("ExpressionStart");
            }
        }

        public double _expressionEnd;
        public double ExpressionEnd
        {
            get { return _expressionEnd; }
            set
            {
                _expressionEnd = value;
                OnPropertyChanged("ExpressionEnd");
            }
        }

        private double _expressionInterval;
        public double ExpressionInterval
        {
            get { return _expressionInterval; }
            set
            {
                _expressionInterval = value;
                OnPropertyChanged("ExpressionInterval");
            }
        }

        private Path _xaxisPath;
        public Path XaxisPath
        {
            get { return _xaxisPath; }
            set
            {
                _xaxisPath = value;
                OnPropertyChanged("XaxisPath");
            }
        }

        private Path _yaxisPath;
        public Path YaxisPath
        {
            get { return _yaxisPath; }
            set
            {
                _yaxisPath = value;
                OnPropertyChanged("YaxisPath");
            }
        }

        private Polyline _datasetPoly;
        public Polyline DatasetPoly
        {
            get { return _datasetPoly; }
            set
            {
                _datasetPoly = value;
                OnPropertyChanged("DatasetPoly");
            }
        }

        private Double Width;
        private Double Height;
        private Matrix WtoDMatrix, DtoWMatrix;

        public event PropertyChangedEventHandler PropertyChanged;

        public void UpdateData()
        {
            // x graph values from user entry
            var vg = new GraphVariableGenerator();
            var indVars = vg.GenerateIndependentVariables(ExpressionStart, ExpressionEnd, ExpressionInterval);

            // y graph values calculated from x
            var depVars = new List<double>();
            foreach (var indVar in indVars)
            {
                var eval = EvaluateFromString(Expression, indVar.ToString());
                depVars.Add(eval);
            }

            // Setup graph sizes based on data
            double wxmin = ExpressionStart;
            double wxmax = ExpressionEnd;
            double wymin = depVars.Min();
            double wymax = depVars.Max();
            double xstep = 1;
            double ystep = 1;
            double xtic = 5;
            double ytic = 0.5;

            // Setup graph sizes based on window
            const double dmargin = 10;
            double dxmin = dmargin;
            double dxmax = Width - dmargin;
            double dymin = dmargin;
            double dymax = Height - dmargin;

            // Prepare the transformation matrices.
            PrepareTransformations(
                wxmin, wxmax, wymin, wymax,
                dxmin, dxmax, dymax, dymin);

            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            Point p0 = new Point(wxmin, 0);
            Point p1 = new Point(wxmax, 0);
            xaxis_geom.Children.Add(new LineGeometry(WtoD(p0), WtoD(p1)));

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;
            XaxisPath = xaxis_path;

            // Make the Y axis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            Point p2 = new Point(wxmin, 0);
            Point p3 = new Point(wxmax, 0);
            yaxis_geom.Children.Add(new LineGeometry(WtoD(p2), WtoD(p3)));

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;
            YaxisPath = yaxis_path;

            // Make data points
            PointCollection points = new PointCollection();
            for (int i = 0; i < indVars.Count; i += 1)
            {
                Point p = new Point(indVars[i], depVars[i]);
                points.Add(WtoD(p));
            }

            // Draw points
            Polyline polyline = new Polyline();
            polyline.StrokeThickness = 1;
            polyline.Stroke = Brushes.Green;
            polyline.Points = points;
            DatasetPoly = polyline;
        }

        // Prepare values for transformations
        private void PrepareTransformations(
        double wxmin, double wxmax, double wymin, double wymax,
        double dxmin, double dxmax, double dymin, double dymax)
        {
            // Make WtoD.
            WtoDMatrix = Matrix.Identity;
            WtoDMatrix.Translate(-wxmin, -wymin);

            double xscale = (dxmax - dxmin) / (wxmax - wxmin);
            double yscale = (dymax - dymin) / (wymax - wymin);
            WtoDMatrix.Scale(xscale, yscale);

            WtoDMatrix.Translate(dxmin, dymin);

            // Make DtoW.
            DtoWMatrix = WtoDMatrix;
            DtoWMatrix.Invert();
        }

        // Transform a point from world to device coordinates.
        private Point WtoD(Point point)
        {
            return WtoDMatrix.Transform(point);
        }

        static double EvaluateFromString(string inputString, string varValue)
        {
            var sp = new StringParser();
            var tp = new TokensParser();
            var tc = new TokenCalculator();

            var parsedTokens = sp.ParseString(inputString);
            var orderedTokens = tp.ReversePolishParse(parsedTokens);
            var varTokens = orderedTokens.FindAll(token => token.Type == "variable");
            var mappedVars = new List<string>();
            foreach (var varToken in varTokens)
            {
                if (!mappedVars.Contains(varToken.Value))
                {
                    mappedVars.Add(varToken.Value);
                    tc.BuildVariableMap(varToken.Value, varValue);
                }
            }
            var evaluation = tc.EvaluateReversePolishExpression(orderedTokens);

            return double.Parse(evaluation);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public StringCalculatorViewModel(double graphHeight, double graphWidth)
        {
            Height = graphHeight;
            Width = graphWidth;
            Expression = "x^3";
            ExpressionStart = -10;
            ExpressionEnd = 10;
            ExpressionInterval = 1;
            UpdateData();
        }
    }
}
