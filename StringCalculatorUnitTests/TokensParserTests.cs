using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StringCalculator2;
using Xunit;
using Xunit.Sdk;

namespace StringCalculatorUnitTests
{
    public class TokensParserTests
    {
        private readonly Token _plus = new Token() { Type = "operator", Value = "+" };
        private readonly Token _minus = new Token() { Type = "operator", Value = "-" };
        private readonly Token _multiply = new Token() { Type = "operator", Value = "*" };
        private readonly Token _divide = new Token() { Type = "operator", Value = "/" };
        private readonly Token _exponent = new Token() { Type = "operator", Value = "^" };
        private readonly Token _openBracket = new Token() { Type = "bracket", Value = "(" };
        private readonly Token _closeBracket = new Token() { Type = "bracket", Value = ")" };
        private readonly Token _one = new Token() { Type = "number", Value = "1" };
        private readonly Token _two = new Token() { Type = "number", Value = "2" };
        private readonly Token _three = new Token() { Type = "number", Value = "3" };
        private readonly Token _four = new Token() { Type = "number", Value = "4" };
        private readonly Token _negativeOne = new Token() { Type = "number", Value = "-1" };
        private readonly Token _negativeTwo = new Token() { Type = "number", Value = "-2" };

        [Fact]
        public void ParseTokensSingleOperation()
        {
            var unparsedTokens = new List<Token> { _one, _plus, _two };
            var expectedTokens = new List<Token> { _one, _two, _plus };

            TokensParser tp = new TokensParser();
            var parsedTokens = tp.ReversePolishParse(unparsedTokens);
            Assert.True(parsedTokens.SequenceEqual(expectedTokens));
        }

        [Fact]
        public void ParseTokensNegativeNumberOperation()
        {
            var unparsedTokens = new List<Token> { _minus, _one, _plus,_minus,_two  };
            var expectedTokens = new List<Token> { _negativeOne, _negativeTwo, _plus };

            TokensParser tp = new TokensParser();
            var parsedTokens = tp.ReversePolishParse(unparsedTokens);
            //Assert.True(parsedTokens.SequenceEqual(expectedTokens));
            for (int i = 0; i < expectedTokens.Count; i++)
            {
                Assert.True(expectedTokens[i].Value == parsedTokens[i].Value);
            }
        }

        [Fact]
        public void ParseTokensSingleOperationWithBrackets()
        {
            var unparsedTokens = new List<Token> { _openBracket, _one, _minus, _two, _closeBracket };
            var expectedTokens = new List<Token> { _one, _two, _minus };

            TokensParser tp = new TokensParser();
            var parsedTokens = tp.ReversePolishParse(unparsedTokens);
            Assert.True(parsedTokens.SequenceEqual(expectedTokens));
        }

        [Fact]
        public void ParseTokensPrecedenceInOrder()
        {
            var unparsedTokens = new List<Token> { _one, _multiply, _two, _divide, _three };
            var expectedTokens = new List<Token> { _one, _two, _multiply, _three, _divide };

            TokensParser tp = new TokensParser();
            var parsedTokens = tp.ReversePolishParse(unparsedTokens);
            for (int i = 0; i < expectedTokens.Count; i++)
            {
                Assert.True(expectedTokens[i].Value == parsedTokens[i].Value);
            }
        }

        [Fact]
        public void ParseTokensPrecedenceInOrderWithOuterBracket()
        {
            var unparsedTokens = new List<Token> { _openBracket, _one, _multiply, _two, _divide, _three, _closeBracket };
            var expectedTokens = new List<Token> { _one, _two, _multiply, _three, _divide };

            TokensParser tp = new TokensParser();
            var parsedTokens = tp.ReversePolishParse(unparsedTokens);
            for (int i = 0; i < expectedTokens.Count; i++)
            {
                Assert.True(expectedTokens[i].Value == parsedTokens[i].Value);
            }
        }

        [Fact]
        public void ParseTokensPrecedenceInOrderWithInnerBracket()
        {
            var unparsedTokens = new List<Token> { _openBracket, _one, _plus, _two, _closeBracket, _minus, _three };
            var expectedTokens = new List<Token> { _one, _two, _plus, _three, _minus };

            TokensParser tp = new TokensParser();
            var parsedTokens = tp.ReversePolishParse(unparsedTokens);
            for (int i = 0; i < expectedTokens.Count; i++)
            {
                Assert.True(expectedTokens[i].Value == parsedTokens[i].Value);
            }
        }

        [Fact]
        public void ParseTokensPrecedenceInOrderWithNestedInnerBracket()
        {
            var unparsedTokens = new List<Token> { _openBracket, _openBracket, _one, _divide, _two, _closeBracket, _minus, _three, _closeBracket, _plus, _four };
            var expectedTokens = new List<Token> { _one, _two, _divide, _three, _minus,_four, _plus };

            TokensParser tp = new TokensParser();
            var parsedTokens = tp.ReversePolishParse(unparsedTokens);
            Assert.Equal(expectedTokens, parsedTokens);
        }

        [Fact]
        public void ParseTokensPrecedenceNotInOrder()
        {
            var unparsedTokens = new List<Token> { _one, _plus, _two, _divide, _three };
            var expectedTokens = new List<Token> { _one, _two, _three, _divide, _plus };

            TokensParser tp = new TokensParser();
            var parsedTokens = tp.ReversePolishParse(unparsedTokens);
            Assert.Equal(expectedTokens, parsedTokens);
        }

        [Fact]
        public void ParseTokensPrecedenceNotInOrderWithOuterBracket()
        {

            var unparsedTokens = new List<Token> { _openBracket, _one, _plus, _two, _divide, _three, _closeBracket };
            var expectedTokens = new List<Token> { _one, _two, _three, _divide, _plus };

            TokensParser tp = new TokensParser();
            var parsedTokens = tp.ReversePolishParse(unparsedTokens);
            Assert.Equal(expectedTokens, parsedTokens);
        }

        [Fact]
        public void ParseTokensPrecedenceNotInOrderWithInnerBracket()
        {
            var unparsedTokens = new List<Token> { _openBracket, _one, _plus, _two, _closeBracket, _multiply, _three };
            var expectedTokens = new List<Token> { _one, _two, _plus, _three, _multiply };

            TokensParser tp = new TokensParser();
            var parsedTokens = tp.ReversePolishParse(unparsedTokens);
            Assert.Equal(expectedTokens, parsedTokens);
        }

        [Fact]
        public void ParseTokensPrecedenceNotInOrderWithNestedInnerBracket()
        {
            var unparsedTokens = new List<Token> {_openBracket,_one,_plus,_openBracket,_two,_minus,_three,_divide,_four,_closeBracket,_closeBracket };
            var expectedTokens = new List<Token> { _one, _two, _three, _four, _divide, _minus, _plus };

            TokensParser tp = new TokensParser();
            var parsedTokens = tp.ReversePolishParse(unparsedTokens);
            Assert.Equal(expectedTokens, parsedTokens);
        }

        [Fact]
        public void ParseTokensPrecedenceInOrderWithMultipleBrackets()
        {
            var unparsedTokens = new List<Token> { _openBracket, _one, _multiply, _two, _closeBracket, _minus, _openBracket, _three, _minus, _four, _closeBracket };
            var expectedTokens = new List<Token> { _one, _two, _multiply, _three, _four, _minus, _minus };

            TokensParser tp = new TokensParser();
            var parsedTokens = tp.ReversePolishParse(unparsedTokens);
            Assert.Equal(expectedTokens, parsedTokens);
        }

        [Fact]
        public void ParseTokensPrecedenceNotInOrderWithMultipleBrackets()
        {
            var unparsedTokens = new List<Token> { _openBracket, _one, _plus, _two, _closeBracket, _plus, _openBracket, _three, _multiply, _four, _closeBracket };
            var expectedTokens = new List<Token> { _one, _two, _plus, _three, _four, _multiply, _plus };

            TokensParser tp = new TokensParser();
            var parsedTokens = tp.ReversePolishParse(unparsedTokens);
            Assert.Equal(expectedTokens, parsedTokens);
        }

        [Fact]
        public void ParseTokensPrecedenceVaries()
        {
            var unparsedTokens = new List<Token> { _one, _plus, _two, _exponent, _three,_divide, _four };
            var expectedTokens = new List<Token> { _one, _two, _three, _exponent, _four, _divide, _plus };

            TokensParser tp = new TokensParser();
            var parsedTokens = tp.ReversePolishParse(unparsedTokens);
            Assert.Equal(expectedTokens, parsedTokens);
        }

        [Fact]
        public void ExceptionUnopenedBrackets()
        {
            var unparsedTokens = new List<Token> { _closeBracket, _one, _plus, _two };

            TokensParser tp = new TokensParser();
            Assert.Throws<ArgumentException>(() => tp.ReversePolishParse(unparsedTokens));
        }

        [Fact]
        public void ExceptionUnclosedBrackets()
        {
            var unparsedTokens = new List<Token> { _openBracket, _one, _plus, _two };

            TokensParser tp = new TokensParser();
            Assert.Throws<ArgumentException>(() => tp.ReversePolishParse(unparsedTokens));
        }

        [Fact]
        public void ExceptionChainOperation()
        {
            var unparsedTokens = new List<Token> { _plus, _multiply };

            TokensParser tp = new TokensParser();
            Assert.Throws<ArgumentException>(() => tp.ReversePolishParse(unparsedTokens));
        }

        [Fact]
        public void ExceptionOperationNotParseToBaseSizePolish()
        {
            var unparsedTokens = new List<Token> { _one };

            TokensParser tp = new TokensParser();
            Assert.Throws<ArgumentException>(() => tp.ReversePolishParse(unparsedTokens));
        }
    }
}
