using System;
using System.Collections.Generic;
using System.Text;
using StringCalculator2;

namespace StringCalculatorIntegrationTests.POCOs
{
    class CalculatorStringTokens
    {
        public List<Token> StringTokens { get; set; }

        public CalculatorStringTokens(List<Token> stringTokens)
        {
            StringTokens = stringTokens;
        }
    }
}
