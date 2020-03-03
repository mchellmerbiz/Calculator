using StringCalculator2;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StringCalculatorUnitTests
{
    public class TokensOptimiserNondestructiveTests
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
        private readonly Token _zero = new Token() { Type = "number", Value = "0" };
        private readonly Token _two = new Token() { Type = "number", Value = "2" };
        private readonly Token _three = new Token() { Type = "number", Value = "3" };
        private readonly Token _four = new Token() { Type = "number", Value = "4" };
        private readonly Token _negativeOne = new Token() { Type = "number", Value = "-1" };
        private readonly Token _negativeTwo = new Token() { Type = "number", Value = "-2" };
        private readonly Token _varX = new Token() { Type = "variable", Value = "x" };
        private readonly Token _varY = new Token() { Type = "variable", Value = "y" };

        [Fact]
        public void OptimiseTokensToIncreaseExponent()
        {
            TokensOptimiserNondestructive to = new TokensOptimiserNondestructive();

            //var input = new List<Token>() { _one, CopyToken(_one), _multiply, CopyToken(_one), CopyToken(_multiply), CopyToken(_one), CopyToken(_multiply) };
            var input = new List<Token>() { _one, _one, _multiply, _one, _multiply, _one, _multiply };
            var expectedOutput = new List<Token>() { _one, _four, _exponent };

            to.InjectTokens(CopyTokens(input));
            to.OptimiseTokensNondestructive();
            var actualOutput = to.OptimisedTokens;
            var e = $"Expected Output:\n{TokensToString(expectedOutput)}\nActual Output:\n{TokensToString(actualOutput)}";
            for (int i = 0; i < expectedOutput.Count; i++)
            {
                Assert.True(expectedOutput[i].Value == actualOutput[i].Value, e);
                Assert.True(expectedOutput[i].Type == actualOutput[i].Type, e);
            }
            
        }

        [Fact]
        public void OptimiseTokensToDecreaseExponent()
        {
            TokensOptimiserNondestructive to = new TokensOptimiserNondestructive();

            //var input = new List<Token>() { _one, CopyToken(_one), _multiply, CopyToken(_one), CopyToken(_multiply), CopyToken(_one), CopyToken(_multiply) };
            var input = new List<Token>() { _one, _one, _multiply, _one, _multiply, _one, _divide };
            var expectedOutput = new List<Token>() { _one, _two, _exponent };

            to.InjectTokens(CopyTokens(input));
            to.OptimiseTokensNondestructive();
            var actualOutput = to.OptimisedTokens;
            var e = $"Expected Output:\n{TokensToString(expectedOutput)}\nActual Output:\n{TokensToString(actualOutput)}";
            for (int i = 0; i < expectedOutput.Count; i++)
            {
                Assert.True(expectedOutput[i].Value == actualOutput[i].Value, e);
                Assert.True(expectedOutput[i].Type == actualOutput[i].Type, e);
            }

        }

        [Fact]
        public void OptimiseTokensToExponentUsingVars()
        {
            TokensOptimiserNondestructive to = new TokensOptimiserNondestructive();

            //var input = new List<Token>() { _one, CopyToken(_one), _multiply, CopyToken(_one), CopyToken(_multiply), CopyToken(_one), CopyToken(_multiply) };
            var input = new List<Token>() { _varX, _varX, _multiply, _varX, _multiply, _varX, _multiply };
            var expectedOutput = new List<Token>() { _varX, _four, _exponent };

            to.InjectTokens(CopyTokens(input));
            to.OptimiseTokensNondestructive();
            var actualOutput = to.OptimisedTokens;
            var e = $"Expected Output:\n{TokensToString(expectedOutput)}\nActual Output:\n{TokensToString(actualOutput)}";
            for (int i = 0; i < expectedOutput.Count; i++)
            {
                Assert.True(expectedOutput[i].Value == actualOutput[i].Value, e);
                Assert.True(expectedOutput[i].Type == actualOutput[i].Type, e);
            }

        }


        [Fact]
        public void OptimiseTokensToExponentUsingFunctions()
        {
            TokensOptimiserNondestructive to = new TokensOptimiserNondestructive();

            //var input = new List<Token>() { _one, CopyToken(_one), _multiply, CopyToken(_one), CopyToken(_multiply), CopyToken(_one), CopyToken(_multiply) };
            var input = new List<Token>() { _varX, _sin, _varX, _varX, _multiply, _varX, _multiply, _varX, _multiply, _plus };
            var expectedOutput = new List<Token>() { _varX, _sin, _varX, _four, _exponent, _plus };

            to.InjectTokens(CopyTokens(input));
            to.OptimiseTokensNondestructive();
            var actualOutput = to.OptimisedTokens;
            var e = $"Expected Output:\n{TokensToString(expectedOutput)}\nActual Output:\n{TokensToString(actualOutput)}";
            for (int i = 0; i < expectedOutput.Count; i++)
            {
                Assert.True(expectedOutput[i].Value == actualOutput[i].Value, e);
                Assert.True(expectedOutput[i].Type == actualOutput[i].Type, e);
            }

        }

        [Fact]
        public void OptimiseTokensResolveToOne()
        {
            TokensOptimiserNondestructive to = new TokensOptimiserNondestructive();

            //var input = new List<Token>() { _one, CopyToken(_one), _multiply, CopyToken(_one), CopyToken(_multiply), CopyToken(_one), CopyToken(_multiply) };
            var input = new List<Token>() { _varX, _varX, _divide, _varX, _varX, _multiply, _plus };
            var expectedOutput = new List<Token>() { _one, _varX, _two, _exponent, _plus };

            to.InjectTokens(CopyTokens(input));
            to.OptimiseTokensNondestructive();
            var actualOutput = to.OptimisedTokens;
            var e = $"Expected Output:\n{TokensToString(expectedOutput)}\nActual Output:\n{TokensToString(actualOutput)}";
            for (int i = 0; i < expectedOutput.Count; i++)
            {
                Assert.True(expectedOutput[i].Value == actualOutput[i].Value, e);
                Assert.True(expectedOutput[i].Type == actualOutput[i].Type, e);
            }
        }

        [Fact]
        public void OptimiseTokensResolveToOneExponent()
        {
            TokensOptimiserNondestructive to = new TokensOptimiserNondestructive();

            var input = new List<Token>() { _varX, _zero, _exponent, _varX, _varX, _multiply, _plus };
            var expectedOutput = new List<Token>() { _one, _varX, _two, _exponent, _plus };

            to.InjectTokens(CopyTokens(input));
            to.OptimiseTokensNondestructive();
            var actualOutput = to.OptimisedTokens;
            var e = $"Expected Output:\n{TokensToString(expectedOutput)}\nActual Output:\n{TokensToString(actualOutput)}";
            for (int i = 0; i < expectedOutput.Count; i++)
            {
                Assert.True(expectedOutput[i].Value == actualOutput[i].Value, e);
                Assert.True(expectedOutput[i].Type == actualOutput[i].Type, e);
            }
        }

        [Fact]
        public void OptimiseTokensResolveToZero()
        {
            TokensOptimiserNondestructive to = new TokensOptimiserNondestructive();

            //var input = new List<Token>() { _one, CopyToken(_one), _multiply, CopyToken(_one), CopyToken(_multiply), CopyToken(_one), CopyToken(_multiply) };
            var input = new List<Token>() { _one, _sin, _one, _varX, _divide, _zero, _multiply, _plus };
            var expectedOutput = new List<Token>() { _one, _sin, _zero, _plus };

            to.InjectTokens(CopyTokens(input));
            to.OptimiseTokensNondestructive();
            var actualOutput = to.OptimisedTokens;
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

        private List<Token> CopyTokens(List<Token> inTokens)
        {
            var outTokens = new List<Token>();
            foreach (var token in inTokens)
            {
                outTokens.Add(new Token() { Type = token.Type, Value = token.Value });
            }
            return outTokens;
        }
    }
}
