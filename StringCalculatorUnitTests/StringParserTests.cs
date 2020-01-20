using System;
using System.Linq;
using System.Runtime.CompilerServices;
using StringCalculator2;
using Xunit;

namespace StringCalculatorUnitTests
{
    public class StringParserTests
    {
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
        [InlineData("uiop('", "(")]
        [InlineData("1a234)v", "1234")]
        [InlineData("v+v ", "+")]
        [InlineData(" )v'", ")")]
        [InlineData("25!t..16", "25.16")]
        [InlineData("*_", "*")]
        [InlineData("/'", "/")]
        [InlineData("a-_", "-")]
        public void TokeniseInvalidCharHandledAndValueCorrect(string testString, string value)
        {
            StringParser sp = new StringParser();
            var validTokens = sp.ParseString(testString);
            var testValue = validTokens[0].Value;
            Assert.True(testValue == value, $"Parser failed to handle invalid char in string, expected a token value of {value}, parsed value was {testValue}");
        }

        [Theory]
         [InlineData("uiop('", "(", "bracket")]
         [InlineData("1a234)v", "1234", "number")]
         [InlineData("v+v ", "+", "operation")]
         [InlineData(" )v'", ")", "bracket")]
         [InlineData("25!t..16", "25.16", "number")]
         [InlineData("*_", "*", "operation")]
         [InlineData("/'", "/", "operation")]
         [InlineData("a-_", "-", "operation")]
        public void TokeniseInvalidCharHandledAndTypeCorrect(string testString, string value, string tokenType)
        {
            StringParser sp = new StringParser();
            var validTokens = sp.ParseString(testString);
            var testType = validTokens[0].Type;
            Assert.True(testType == tokenType, $"Parser failed to handle invalid char in string, token type for {value} exected as as type {tokenType}, type assigned was {testType}");
        }

        [Theory]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25", 0, "(")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25", 1, "1234")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25", 3, "+")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25", 21, ")")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25", 24, "25.16")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25", 5, "*")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25", 7, "/")]
        [InlineData("(1234)+2*12/-3+(6-12-(-23+6))*25.16+-0.25", 8, "-")]
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
