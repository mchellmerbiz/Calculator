using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using StringCalculator2;
using Xunit;

namespace StringCalculatorUnitTests
{
    public class StringParserTests
    {
        private readonly Token _plus = new Token() { Type = "operator", Value = "+" };
        private readonly Token _minus = new Token() { Type = "operator", Value = "-" };
        private readonly Token _multiply = new Token() { Type = "operator", Value = "*" };
        private readonly Token _divide = new Token() { Type = "operator", Value = "/" };
        private readonly Token _exponent = new Token() { Type = "operator", Value = "^" };
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
        [InlineData("12.34", "12.34")]
        [InlineData(".05", ".05")]
        public void TokeniseDecimalIdentifiesNumberWithDecimal(string testString, string value)
        {
            StringParser sp = new StringParser();
            var validTokens = sp.ParseString(testString);
            var testValue = validTokens[0].Value;
            Assert.True(testValue == value, $"Parser failed to recognise decimal number {value}, parsed value was {testValue}");
        }

        [Theory]
        [InlineData("12.34", "12.34")]
        [InlineData(".05", ".05")]
        public void TokeniseDecimalAssignsDecimalNumberAsNumberType(string testString, string value)
        {
            StringParser sp = new StringParser();
            var validTokens = sp.ParseString(testString);
            var testType = validTokens[0].Type;
            Assert.True(testType == "number", $"Parser failed to recognise decimal number {value} as type 'number', type assigned was {testType}");
        }

        [Theory]
        [InlineData("12..34", "12.34")]
        [InlineData("..2.01", ".201")]
        [InlineData("12.0.0..0", "12.000")]
        [InlineData("...05", ".05")]
        public void TokeniseDecimalRemoveExtraDecimal(string testString, string value)
        {
            StringParser sp = new StringParser();
            var validTokens = sp.ParseString(testString);
            var testValue = validTokens[0].Value;
            Assert.True(testValue == value, $"Parser failed to remove extra decimals correctly {value} was expected, parsed value was {testValue}");
        }

        [Theory]
        [InlineData("0000002", "0000002")]
        [InlineData("12465486435165468435165468465130000684652610000", "12465486435165468435165468465130000684652610000")]
        public void TokeniseIntegerIdentifiesInteger(string testString, string value)
        {
            StringParser sp = new StringParser();
            var validTokens = sp.ParseString(testString);
            var testValue = validTokens[0].Value;
            Assert.True(testValue == value, $"Parser failed to recognise integer number {value}, parsed value was {testValue}");
        }

        [Theory]
        [InlineData("0000002", "0000002")]
        [InlineData("12465486435165468435165468465130000684652610000", "12465486435165468435165468465130000684652610000")]
        public void TokeniseIntegerIdentifiesIntegerTypeAsNumber(string testString, string value)
        {
            StringParser sp = new StringParser();
            var validTokens = sp.ParseString(testString);
            var testType = validTokens[0].Type;
            Assert.True(testType == "number", $"Parser failed to recognise integer number {value} as type 'number', type assigned was {testType}");
        }

        [Theory]
        [InlineData("+", "+")]
        [InlineData("*", "*")]
        [InlineData("-", "-")]
        [InlineData("/", "/")]
        [InlineData("^", "^")]
        public void TokeniseOperatorIdentifiesOperator(string testString, string value)
        {
            StringParser sp = new StringParser();
            var validTokens = sp.ParseString(testString);
            var testValue = validTokens[0].Value;
            Assert.True(testValue == value, $"Parser failed to recognise operator, {value}, parsed value was {testValue}");
        }

        [Theory]
        [InlineData("+", "+")]
        [InlineData("*", "*")]
        [InlineData("-", "-")]
        [InlineData("/", "/")]
        [InlineData("^", "^")]
        public void TokeniseOperatorIdentifiesOperatorTypeAsOperation(string testString, string value)
        {
            StringParser sp = new StringParser();
            var validTokens = sp.ParseString(testString);
            var testType = validTokens[0].Type;
            Assert.True(testType == "operation", $"Parser failed to recognise operator {value} as type 'operator', type assigned was {testType}");
        }

        [Theory]
        [InlineData("(", "(")]
        [InlineData(")", ")")]
        public void TokeniseBracketIdentifiesBracket(string testString, string value)
        {
            StringParser sp = new StringParser();
            var validTokens = sp.ParseString(testString);
            var testValue = validTokens[0].Value;
            Assert.True(testValue == value, $"Parser failed to recognise bracket, {value}, parsed value was {testValue}");
        }

        [Theory]
        [InlineData("(", "(")]
        [InlineData(")", ")")]
        public void TokeniseBracketIdentifiesTypeAsBracket(string testString, string value)
        {
            StringParser sp = new StringParser();
            var validTokens = sp.ParseString(testString);
            var testType = validTokens[0].Type;
            Assert.True(testType == "bracket", $"Parser failed to recognise bracket {value} as type 'bracket', type assigned was {testType}");
        }

        [Theory]
        [InlineData("sin(1)")]
        [InlineData("cos(1)")]
        [InlineData("tan(1)")]
        public void TokeniseFunctionIdentifiesFunctionBoundaries(string testString)
        {
            var functionToken = new Token();
            switch (testString.Substring(0,3))
            {
                case "sin":
                    functionToken = _sin;
                    break;
                case "cos":
                    functionToken = _cos;
                    break;
                case "tan":
                    functionToken = _tan;
                    break;
            }
            var expectedTokens = new List<Token> {functionToken, _openBracket, _one, _closeBracket};
            StringParser sp = new StringParser();
            var validTokens = sp.ParseString(testString);
            for (int i = 0; i < expectedTokens.Count-1; i++)
            {
                Assert.True(expectedTokens[i].Value == validTokens[i].Value, $"Parser failed to parse function {testString} value, expected {expectedTokens[i].Value} parsed {validTokens[i].Value}");
                Assert.True(expectedTokens[i].Type == validTokens[i].Type, $"Parser failed to parse function {testString} token type, expected {expectedTokens[i].Type} parsed {validTokens[i].Type}");
            }
        }

        [Theory]
        [InlineData("uiop('", "(")]
        [InlineData("1234)v", "1234")]
        [InlineData("v+v ", "+")]
        [InlineData(" )v'", ")")]
        [InlineData("25..16n", "25.16")]
        [InlineData("*_", "*")]
        [InlineData("/'", "/")]
        [InlineData("a-_", "-")]
        [InlineData("l^ ", "^")]
        public void TokeniseInvalidCharHandledAndValueCorrect(string testString, string value)
        {
            StringParser sp = new StringParser();
            var validTokens = sp.ParseString(testString);
            var testValue = validTokens[0].Value;
            Assert.True(testValue == value, $"Parser failed to handle invalid char in string, expected a token value of {value}, parsed value was {testValue}");
        }

        [Theory]
         [InlineData("uiop('", "(", "bracket")]
         [InlineData("1234)v", "1234", "number")]
         [InlineData("v+v ", "+", "operation")]
         [InlineData(" )v'", ")", "bracket")]
         [InlineData("25..16n", "25.16", "number")]
         [InlineData("*_", "*", "operation")]
         [InlineData("/'", "/", "operation")]
         [InlineData("a-_", "-", "operation")]
        [InlineData("l^ ", "^", "operation")]
        public void TokeniseInvalidCharHandledAndTypeCorrect(string testString, string value, string tokenType)
        {
            StringParser sp = new StringParser();
            var validTokens = sp.ParseString(testString);
            var testType = validTokens[0].Type;
            Assert.True(testType == tokenType, $"Parser failed to handle invalid char in string, token type for {value} exected as as type {tokenType}, type assigned was {testType}");
        }

        [Theory]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25^2", 0, "(")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25^2", 1, "1234")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25^2", 3, "+")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25^2", 21, ")")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25^2", 24, "25.16")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25^2", 5, "*")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25^2", 7, "/")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25^2", 8, "-")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25^2", 28, "^")]
        public void TokeniserCreatesTokenWithCorrectValue(string testString, int tokenId, string value)
        {
            StringParser sp = new StringParser();
            var validTokens = sp.ParseString(testString);
            var testToken = validTokens[tokenId];
            Assert.True(testToken.Value == value, $"Parser failed to store {value} correctly");
        }

        [Theory]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25", 0, "bracket")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25", 1, "number")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25", 3, "operation")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25", 21, "bracket")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25", 24, "number")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25", 5, "operation")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25", 7, "operation")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25", 8, "operation")]
        public void TokeniserCreatesTokenWithCorrectType(string testString, int tokenId, string type)
        {
            StringParser sp = new StringParser();
            var validTokens = sp.ParseString(testString);
            var testToken = validTokens[tokenId];
            Assert.True(testToken.Type == type, $"Parser failed to identify {testToken.Value} as a {type}");
        }
    }
}
