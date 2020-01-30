using System;
using System.Collections.Generic;
using System.Text;
using StringCalculator2;
using Xunit;

namespace StringCalculatorUnitTests
{
    public class TokenCalculatorTests
    {
        private readonly Token _plus = new Token() { Type = "operation", Value = "+" };
        private readonly Token _minus = new Token() { Type = "operation", Value = "-" };
        private readonly Token _multiply = new Token() { Type = "operation", Value = "*" };
        private readonly Token _divide = new Token() { Type = "operation", Value = "/" };
        private readonly Token _exponent = new Token() { Type = "operation", Value = "^" };
        private readonly Token _openBracket = new Token() { Type = "bracket", Value = "(" };
        private readonly Token _closeBracket = new Token() { Type = "bracket", Value = ")" };
        private readonly Token _sin = new Token() { Type = "function", Value = "sin" };
        private readonly Token _cos = new Token() { Type = "function", Value = "cos" };
        private readonly Token _tan = new Token() { Type = "function", Value = "tan" };
        private readonly Token _one = new Token() { Type = "number", Value = "1" };
        private readonly Token _two = new Token() { Type = "number", Value = "2" };
        private readonly Token _three = new Token() { Type = "number", Value = "3" };
        private readonly Token _four = new Token() { Type = "number", Value = "4" };
        private readonly Token _negativeOne = new Token() { Type = "number", Value = "-1" };
        private readonly Token _negativeTwo = new Token() { Type = "number", Value = "-2" };

        [Theory]
        [InlineData("2+1")]
        [InlineData("2-1")]
        [InlineData("2*1")]
        [InlineData("2/1")]
        [InlineData("2^1")]
        public void CalculateArithmeticExpression(string inputString)
        {
            TokenCalculator tc = new TokenCalculator();
            var input = new List<Token>() { };
            var expectedOutput = "";
            switch (inputString[1])
            {
                case '+':
                    input = new List<Token>() { _two, _one, _plus };
                    expectedOutput = "3";
                    break;
                case '-':
                    input = new List<Token>() { _two, _one, _minus };
                    expectedOutput = "1";
                    break;
                case '*':
                    input = new List<Token>() { _two, _one, _multiply };
                    expectedOutput = "2";
                    break;
                case '/':
                    input = new List<Token>() { _two, _one, _divide };
                    expectedOutput = "2";
                    break;
                case '^':
                    input = new List<Token>() { _two, _one, _exponent };
                    expectedOutput = "2";
                    break;
            }
            string actualOutput = tc.EvaluateReversePolishExpression(input);

            Assert.True(expectedOutput == actualOutput, $"Expected Output: {expectedOutput}\nActual Output: {actualOutput}");
        }

        [Theory]
        [InlineData("sin(2*4)")]
        [InlineData("cos(2*4)")]
        [InlineData("tan(2*4)")]
        public void CalculateFunctionExpression(string inputString)
        {
            TokenCalculator tc = new TokenCalculator();
            var input = new List<Token>() { };
            var expectedOutput = "";
            switch (inputString.Substring(0,3))
            {
                case "sin":
                    input = new List<Token>() { _two, _four, _multiply, _sin };
                    expectedOutput = "0.9894";
                    break;
                case "cos":
                    input = new List<Token>() { _two, _four, _multiply, _cos };
                    expectedOutput = "-0.1455";
                    break;
                case "tan":
                    input = new List<Token>() { _two, _four, _multiply, _tan };
                    expectedOutput = "-6.7997";
                    break;
            }

            string actualOutput = tc.EvaluateReversePolishExpression(input);
            string roundedOutput = Math.Round(float.Parse(actualOutput), 4).ToString();
            Assert.True(expectedOutput == roundedOutput, $"Expected Output rounded to  fourth decimal: {expectedOutput}\nActual Output: {actualOutput} rounded to {roundedOutput}");
        }

        [Theory]
        [InlineData("2/1+3*4")]
        public void CalculateArithmeticExpressionChain(string inputString)
        {
            TokenCalculator tc = new TokenCalculator();
            var input = new List<Token>() { _two, _one, _divide, _three, _four, _multiply, _plus };
            var expectedOutput = "14";

            string actualOutput = tc.EvaluateReversePolishExpression(input);

            Assert.True(expectedOutput == actualOutput, $"Expected Output: {expectedOutput}\nActual Output: {actualOutput}");
        }

        [Theory]
        [InlineData("1+sin(2*4)")]
        public void CalculateFunctionExpressionChain(string inputString)
        {
            TokenCalculator tc = new TokenCalculator();
            var input = new List<Token>() { _one, _two,_four,_multiply, _sin, _plus };
            var expectedOutput = "1.9894";

            string actualOutput = tc.EvaluateReversePolishExpression(input);
            string roundedOutput = Math.Round(float.Parse(actualOutput), 4).ToString();
            Assert.True(expectedOutput == roundedOutput, $"Expected Output rounded to  fourth decimal: {expectedOutput}\nActual Output: {actualOutput} rounded to {roundedOutput}");
        }
    }
}
