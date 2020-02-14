using System;
using NUnit.Framework;
using StringCalculatorIntegrationTests.POCOs;
using StringCalculator2;
using TechTalk.SpecFlow;

namespace StringCalculatorIntegrationTests.StepDefinitions
{
    [Binding]
    public class StringCalculatorSteps
    {
        private CalculatorString _calculatorString;
        private CalculatorStringTokens _calculatorStringTokens;
        private CalculatorReversePolishTokens _calculatorReversePolishTokens;
        private string _evaluation;

        [Given(@"A string expression ""(.*)""")]
        public void GivenAStringExpression(string calculatorString)
        {
            this._calculatorString = new CalculatorString(calculatorString);
        }
        
        [When(@"I parse the string into calculator tokens")]
        public void WhenIParseTheStringIntoCalculatorTokens()
        {
            var sp = new StringParser();
            var stringTokens = sp.ParseString(_calculatorString.StringValue);
            this._calculatorStringTokens = new CalculatorStringTokens(stringTokens);
        }
        
        [When(@"I parse the calculator tokens into reverse polish notation")]
        public void WhenIParseTheCalculatorTokensIntoReversePolishNotation()
        {
            var tp = new TokensParser();
            var rpTokens = tp.ReversePolishParse(_calculatorStringTokens.StringTokens);
            this._calculatorReversePolishTokens = new CalculatorReversePolishTokens(rpTokens);
        }
        
        [When(@"I evaluate the reverse polish notation tokens")]
        public void WhenIEvaluateTheReversePolishNotationTokens()
        {
            var calc = new TokenCalculator();
            _evaluation = calc.EvaluateReversePolishExpression(_calculatorReversePolishTokens.ReversePolishTokens);

        }

        [When(@"I evaluate the reverse polish notation tokens with variables as follows:")]
        public void WhenIEvaluateTheReversePolishNotationTokensWithVariablesAsFollows(Table table)
        {
            var calc = new TokenCalculator();
            foreach (var row in table.Rows)
            {
                calc.BuildVariableMap(row[0], row[1]);
            }
            _evaluation = calc.EvaluateReversePolishExpression(_calculatorReversePolishTokens.ReversePolishTokens);

        }
        
        [Then(@"the result should be ""(.*)"" to ""(.*)"" decimal places")]
        public void ThenTheResultShouldBe(string expectedValue, string rounding)
        {
            float eval = float.Parse(_evaluation);
            int roundingA = int.Parse(rounding);
            var shortenedEval = Math.Round(eval, roundingA).ToString();
            if (expectedValue != shortenedEval)
            {
                Assert.Fail(
                    $"Calculator evaluation incorrect:\n" +
                    $"Input String: {_calculatorString}\n" +
                    $"Expected Evaluation: {expectedValue}\n" +
                    $"Actual Evaluation: {shortenedEval}"
                );
            }
            else
            {
                Assert.Pass();
            }
        }
    }
}
