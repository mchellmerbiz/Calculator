using System;
using System.Collections.Generic;
using System.Text;

namespace StringCalculatorIntegrationTests.POCOs
{
    public class CalculatorString
    {
        public string StringValue { get; set; }

        public CalculatorString(string calculatorString)
        {
            StringValue = calculatorString;
        }
    }
}
