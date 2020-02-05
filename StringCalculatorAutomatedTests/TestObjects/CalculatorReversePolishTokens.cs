using System;
using System.Collections.Generic;
using System.Text;
using StringCalculator2;

namespace StringCalculatorIntegrationTests.POCOs
{
    class CalculatorReversePolishTokens
    {
        public List<Token> ReversePolishTokens { get; set; }

        public CalculatorReversePolishTokens(List<Token> rpTokens)
        {
            ReversePolishTokens = rpTokens;
        }
    }
}
