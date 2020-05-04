using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculatorGraph.Models
{
    public class GraphVariableGenerator
    {
        public List<double> GenerateIndependentVariables(double start, double end, double interval)
        {
            var indVar = start;
            var indVars = new List<double>();
            while (indVar <= end)
            {
                indVars.Add(indVar);
                indVar += interval;
            }
            return indVars;
        }
    }
}
