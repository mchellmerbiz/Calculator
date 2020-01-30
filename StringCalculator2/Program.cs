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
            var sp = new StringParser();
            var tp = new TokensParser();
            var tc = new TokenCalculator();

            Console.WriteLine("Enter an expression to evaluate: ");
            var rawString = Console.ReadLine();

            var parsedTokens = sp.ParseString(rawString);
            Console.WriteLine("String parsed to tokens:");
            foreach (var expressionToken in parsedTokens)
            {
                Console.WriteLine($"{expressionToken.Type}: {expressionToken.Value}");
            }

            var orderedTokens = tp.ReversePolishParse(parsedTokens);
            Console.WriteLine("Tokens parsed as reverse polish:");
            foreach (var expressionToken in orderedTokens)
            {
                Console.WriteLine($"{expressionToken.Type}: {expressionToken.Value}");
            }

            var evaluation = tc.EvaluateReversePolishExpression(orderedTokens);

            Console.WriteLine($"Expression {rawString} evaluated to {evaluation}.");
            Console.ReadKey();
        }
    }
}
