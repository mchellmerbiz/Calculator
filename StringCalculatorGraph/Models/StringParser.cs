using System;
using System.Collections.Generic;
using System.Linq;

namespace StringCalculatorGraph.Models
{
    class StringParser
    {
        private int TokensBuilt { get; set; }
        private string StoredString { get; set; } // Stored string used for parsing

        //Method to recieve a string and return a list of tokens
        public List<Token> ParseString(string rawString)
        {
            StoredString = rawString;
            var tokens = ParseStringToTokens(StoredString);
            return tokens;
        }

        private List<Token> ParseStringToTokens(string input)
        {
            TokensBuilt = 0;
            var tokens = new List<Token>();
            var validOperations = new List<char>() { '+', '-', '/', '*', '^' };
            var validBrackets = new List<char>() {'(', ')'};
            var validFunctions = new List<string>() { "sin", "cos", "tan" };
            var singleCharStates = new List<string>() { "operation", "bracket", "variable"};
            var substringStart = 0;
            var tokenState = "initial";

            //foreach (var chr in input)
            for (int i = 0; i < input.Length; i++)
            {
                var substringEnd = i;
                var currentChar = input[i];
                var pastTokenState = tokenState;

                //Set current state based on new char
                if (char.IsDigit(currentChar) || currentChar == '.')
                {
                    tokenState = "number";
                }
                else if (validOperations.Contains(currentChar))
                {
                    tokenState = "operation";
                }
                else if (char.IsLetter(currentChar))
                {
                    tokenState = "function";
                }
                else if (validBrackets.Contains(currentChar))
                {
                    tokenState = "bracket";
                }

                //Build token if necessary
                if ((tokenState != pastTokenState || singleCharStates.Contains(pastTokenState)) && pastTokenState != "initial")
                {
                    var newToken = BuildToken(pastTokenState, substringStart, substringEnd, validFunctions,
                        validOperations, validBrackets);
                    if (newToken.Value != null)
                    {
                        tokens.Add(newToken);
                    }
                    substringStart = substringEnd;
                }

                //Build token at final iteration
                if (i == input.Length - 1)
                {
                    var finalToken = BuildToken(tokenState, substringStart, input.Length, validFunctions,
                        validOperations, validBrackets);
                    if (finalToken.Value != null)
                    {
                        tokens.Add(finalToken);
                    }
                }
            }

            return tokens;
        }

        private Token BuildToken(string priorState, int substringStart, int substringEnd, List<string> validFunctions, List<char> validOperations, List<char> validBrackets)
        {
            var tokenValue = StoredString.Substring(substringStart, substringEnd - substringStart);
            tokenValue = CleanValueFromSubstring(tokenValue, priorState, validFunctions, validOperations,
                validBrackets);
            if (priorState == "function" && tokenValue.Length == 1)
            {
                priorState = "variable";
            }
            if (tokenValue != "")
            {
                var newToken = new Token() { Type = priorState, Value = tokenValue, TokenId = TokensBuilt };
                TokensBuilt += 1;
                return newToken;
            }
            return new Token();
        }

        private string CleanValueFromSubstring(string tokenValue, string tokenType, List<string> validFunctions, List<char> validOperations, List<char> validBrackets)
        {
            var rawSubstring = tokenValue;
            var cleanSubstring = "";

            switch (tokenType)
            {
                case "number":
                    bool decimalFound = false;
                    foreach (var chr in rawSubstring)
                    {
                        if (chr == '.')
                        {
                            if (!decimalFound)
                            {
                                decimalFound = true;
                                cleanSubstring += chr;
                            }
                        } else if (char.IsDigit(chr))
                        {
                            cleanSubstring += chr;
                        }
                    }
                    break;
                case "operation":
                    foreach (var chr in rawSubstring)
                    {
                        if (validOperations.Contains(chr))
                        {
                            cleanSubstring += chr;
                        }
                    }
                    break;
                case "bracket":
                    foreach (var chr in rawSubstring)
                    {
                        if (validBrackets.Contains(chr))
                        {
                            cleanSubstring += chr;
                        }
                    }
                    break;
                case "function":
                    cleanSubstring = ValidateFunction(validFunctions, rawSubstring);
                    break;
            }

            if (rawSubstring != cleanSubstring)
            {
                Console.WriteLine($"Token type {tokenType} value parsed: {rawSubstring}\n Correction: {cleanSubstring}\n");
            }
            return cleanSubstring;
        }

        private string ValidateFunction(List<string> validList, string partialFunction)
        {
            if (partialFunction.Length == 1)
            {
                return partialFunction;
            }

            var builtFun = "";
            foreach (var funChar in partialFunction)
            {
                var iBuiltFun = builtFun + funChar;
                var fun = validList.Where(function => function.Substring(0, builtFun.Length) == builtFun);
                var match = validList.Where(function => function == builtFun);
                if (fun.Count() != 0)
                {
                    builtFun = iBuiltFun;
                }

                if (match.Count() == 1)
                {
                    return builtFun;
                }
            }

            Console.WriteLine($"Could not form a function token from raw string {partialFunction}");
            return "";
        }
    }
}