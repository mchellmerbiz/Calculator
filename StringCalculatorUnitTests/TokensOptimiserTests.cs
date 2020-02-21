using StringCalculator2;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StringCalculatorUnitTests
{
    public class TokensOptimiserTests
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
        private readonly Token _varX = new Token() { Type = "variable", Value = "x" };
        private readonly Token _varY = new Token() { Type = "variable", Value = "y" };

        [Fact]
        public void OptimiseTokensToExponent()
        {
            TokensOptimiser to = new TokensOptimiser();

            var input = new List<Token>() { _one, CopyToken(_one), _multiply, CopyToken(_one), CopyToken(_multiply), CopyToken(_one), CopyToken(_multiply) };
            var expectedOutput = new List<Token>() { _one, _four, _exponent };

            var actualOutput = to.OptimiseTokens(input);
            var e = $"Expected Output:\n{TokensToString(expectedOutput)}\nActual Output:\n{TokensToString(actualOutput)}";
            for (int i = 0; i < expectedOutput.Count; i++)
            {
                Assert.True(expectedOutput[i].Value == actualOutput[i].Value, e);
                Assert.True(expectedOutput[i].Type == actualOutput[i].Type, e);
            }
            
        }

        private string TokensToString(List<Token> tokens)
        {
            var outputString = "";
            foreach (var token in tokens)
            {
                outputString += $"Type: {token.Type} Value: {token.Value}\n";
            }
            return outputString;
        }

        private Token CopyToken(Token inToken)
        {
            return new Token() { Type = inToken.Type, Value = inToken.Value };
        }
    }
}
