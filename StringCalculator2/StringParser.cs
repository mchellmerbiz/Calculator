using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using StringCalculator2;

namespace StringCalculator2
{
    class StringParser
    {
        private string TokenState { get; set; } = "initial"; // Present state of FSM parser
        private int TokensBuilt { get; set; }
        private string StoredString { get; set; } // Stored string used for parsing

        //Method to recieve a string and return a list of tokens
        public List<Token> ParseString(string rawString)
        {
            StoredString = rawString;
            var tokens = FSM(StoredString);
            return tokens;
        }

        private List<Token> FSM(string input)
        {
            var tokens = new List<Token>();
            var validOperations = new List<char>() { '+', '-', '/', '*', '^' };
            var validBrackets = new List<char>() { '(', ')' };
            var substringStart = 0;
            var substringEnd = 1;

            foreach (var chr in input)
            {
                if (!ValidateChar(chr, validOperations, validBrackets))
                {
                    HandleInvalidEntryRemoveCharFromString($"Invalid character '{chr}' ignored.", substringEnd);
                    continue;
                }

                var currentTokenState = TokenState;
                switch (currentTokenState)
                {
                    case "number":
                        UpdateState(chr, validOperations, validBrackets);
                        if (TokenState == "number")
                        {
                            substringEnd += 1;
                        }else if (chr == '.')
                        {
                            TokenState = "double";
                            substringEnd += 1;
                        }
                        else
                        {
                            tokens.Add(BuildToken("number", substringStart, substringEnd));
                            substringStart = substringEnd;
                            substringEnd += 1;
                        }
                        break;

                    case "double":
                        UpdateState(chr, validOperations, validBrackets);
                        if (chr == '.')
                        {
                            HandleInvalidEntryRemoveCharFromString($"Number already contains a decimal, ignoring character.", substringEnd);
                        }
                        else if (TokenState == "double")
                        {
                            substringEnd += 1;
                        }
                        else
                        {
                            tokens.Add(BuildToken("number", substringStart, substringEnd));
                            substringStart = substringEnd;
                            substringEnd += 1;
                        }
                        break;

                    case "operation":
                        UpdateState(chr, validOperations, validBrackets);
                        tokens.Add(BuildToken("operation", substringStart, substringEnd));
                        substringStart = substringEnd;
                        substringEnd += 1;
                        break;

                    case "bracket":
                        UpdateState(chr, validOperations, validBrackets);
                        tokens.Add(BuildToken("bracket", substringStart, substringEnd));
                        substringStart = substringEnd;
                        substringEnd += 1;
                        break;

                    default:
                        if (char.IsDigit(chr))
                        {
                            TokenState = "number";
                        }
                        else if (chr == '.')
                        {
                            TokenState = "double";
                        }
                        else if (validOperations.Contains(chr))
                        {
                            TokenState = "operation";
                        }
                        else if (validBrackets.Contains(chr))
                        {
                            TokenState = "bracket";
                        }
                        else
                        {
                            HandleInvalidEntryRemoveCharFromString($"Invalid character '{chr}' ignored.", substringEnd);
                        }
                        break;
                }
            }

            // The final state is a valid token, build it
            // The double and number states should produce a token of type, number
            if (TokenState == "double")
            {
                TokenState = "number";
            }
            tokens.Add(BuildToken(TokenState, substringStart, substringEnd));
            return tokens;
        }

        private bool ValidateChar(char chr, List<char> validList1, List<char> validList2)
        {
            if (!validList1.Contains(chr) && !validList2.Contains(chr) && !char.IsDigit(chr) && chr != '.')
            {
                return false;
            }

            return true;
        }

        private void HandleInvalidEntryRemoveCharFromString(string error, int valueEnd)
        {
            // Handle case where invalid char is first char in parsed string
            if (TokenState == "initial")
            {
                valueEnd = 0;
            }

            Console.WriteLine(error);
            StoredString = StoredString.Remove(valueEnd, 1);
        }

        private void UpdateState(char newChar, List<char> validOperations, List<char> validBrackets)
        {
            string charState = TokenState;
            if (char.IsDigit(newChar) && TokenState != "double")
            {
                charState = "number";
            }
            else if (newChar == '.' || char.IsDigit(newChar))
            {
                charState = "double";
            }
            else if (validOperations.Contains(newChar))
            {
                charState = "operation";
            }
            else if (validBrackets.Contains(newChar))
            {
                charState = "bracket";
            }

            TokenState = charState;
        }

        private Token BuildToken(string priorState, int valueStart, int valueEnd)
        {
            TokensBuilt += 1;
            var tokenSize = valueEnd - valueStart;
            var tokenValue = StoredString.Substring(valueStart, tokenSize);
            var newToken = new Token(){TokenId = TokensBuilt,Type = priorState, Value = tokenValue };
            return newToken;
        }

        public StringParser()
        {
            TokensBuilt = 0;
        }
    }
}