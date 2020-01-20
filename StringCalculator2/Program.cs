using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("StringCalculatorUnitTests")]
namespace StringCalculator2
{
    class Program
    {
        static void Main(string[] args)
        {
            ////String retriever builds and stores/returns some string
            //StringRetriever sr = new StringRetriever();
            ////String parser takes a string and parses/stores it
            //StringParser sp = new StringParser();

            ////Get the string stored in the string retriever
            //string rawExpression = sr.RetrieveString();
            ////Parse string will return a list of tokens as elements of the raw string given
            //var expressionTokens = sp.ParseString(rawExpression);

            var tokenParser = new TokensParser();
            var unparsedTokens = new List<Token>();
            unparsedTokens.Add(new Token() { TokenId = 0, Type = "bracket", Value = "(" });
            //unparsedTokens.Add(new Token() { TokenId = 0, Type = "bracket", Value = "(" });
            unparsedTokens.Add(new Token() { TokenId = 0, Type = "number", Value = "1" });
            unparsedTokens.Add(new Token() { TokenId = 0, Type = "operator", Value = "-" });
            unparsedTokens.Add(new Token() { TokenId = 0, Type = "number", Value = "2" });
            unparsedTokens.Add(new Token() { TokenId = 0, Type = "bracket", Value = ")" });
            //unparsedTokens.Add(new Token() { TokenId = 0, Type = "bracket", Value = ")" });
            unparsedTokens.Add(new Token() { TokenId = 0, Type = "operator", Value = "*" });
            unparsedTokens.Add(new Token() { TokenId = 0, Type = "bracket", Value = "(" });
            unparsedTokens.Add(new Token() { TokenId = 0, Type = "number", Value = "3" });
            unparsedTokens.Add(new Token() { TokenId = 0, Type = "operator", Value = "+" });
            unparsedTokens.Add(new Token() { TokenId = 0, Type = "number", Value = "3" });
            unparsedTokens.Add(new Token() { TokenId = 0, Type = "bracket", Value = ")" });
            //unparsedTokens.Add(new Token() { TokenId = 0, Type = "operator", Value = "*" });
            //unparsedTokens.Add(new Token() { TokenId = 0, Type = "number", Value = "4" });
            var parsedTokens = tokenParser.ReversePolishParse(unparsedTokens);

            foreach (var expressionToken in parsedTokens)
            {
                Console.WriteLine($"{expressionToken.Type}: {expressionToken.Value}");
            }
            Console.ReadKey();
        }
    }
}
