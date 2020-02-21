using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("StringCalculatorUnitTests")]
[assembly: InternalsVisibleTo("StringCalculatorAutomatedTests")]
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

            var varTokens = orderedTokens.FindAll(token => token.Type == "variable");
            var mappedVars = new List<string>();
            foreach (var varToken in varTokens)
            {
                if (!mappedVars.Contains(varToken.Value))
                {
                    mappedVars.Add(varToken.Value);
                    Console.WriteLine($"Variable {varToken.Value} detected, set a value: ");
                    var varValue = Console.ReadLine();
                    tc.BuildVariableMap(varToken.Value, varValue);
                }
            }
            var evaluation = tc.EvaluateReversePolishExpression(orderedTokens);

            Console.WriteLine($"Expression {rawString} evaluated to {evaluation}.");
            Console.ReadKey();
        }
    }
}
