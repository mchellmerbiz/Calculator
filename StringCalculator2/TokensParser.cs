using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringCalculator2
{
    class TokensParser
    {
        private string _parsingState = "await number";
        private int _openBrackets;
        
        public List<Token> ReversePolishParse(List<Token> unsortedTokens)
        {
            var validPolish = new List<Token>();
            var operationStack = new List<Token>();
            var bracketOperationStack = new List<Token>();
            var functionCache = new List<Token>();
            var openFunctionBracket = false;

            foreach (var token in unsortedTokens)
            {
                if (token.Type == "bracket")
                {
                    _parsingState = "handle bracket";
                }
                else if(token.Type == "function")
                {
                    _parsingState = "function";
                }

                switch (_parsingState)
                {
                    case "handle bracket":
                        //Open brackets should act like creating a new fresh polish, we should cache any stored operators
                        if (token.Value == "(")
                        {
                            //We need to add a placeholder token to the funciton cache to track open brackets not associated with functions
                            if (openFunctionBracket)
                            {
                                openFunctionBracket = false;
                            }
                            else
                            {
                                functionCache.Add(token);
                            }
                            _openBrackets += 1;
                            if (operationStack.Count > 0)
                            {
                                bracketOperationStack.Add(operationStack[0]);
                                operationStack.RemoveAt(0);
                            }
                            _parsingState = "await number";
                        }
                        //Closing brackets should finish some sub polish somewhere, cached operators should be restored
                        else
                        {
                            _openBrackets -= 1;
                            if (operationStack.Count > 0)
                            {
                                validPolish.Add(operationStack[0]);
                                operationStack.RemoveAt(0);
                            }
                            if (bracketOperationStack.Count > 0)
                            {
                                operationStack.Add(bracketOperationStack[0]);
                                bracketOperationStack.RemoveAt(0);
                            }

                            if (functionCache.Count < 1)
                            {
                                throw new System.ArgumentException("Input tokens contain unopened brackets");
                            }

                            var closedFunction = functionCache.Last();
                            if (closedFunction.Value != "(")
                            {
                                validPolish.Add(closedFunction);
                            }
                            functionCache.RemoveAt(functionCache.Count-1);
                            _parsingState = "await operator";
                        }
                        break;

                    case "function":
                        functionCache.Add(token);
                        openFunctionBracket = true;
                        break;

                    case "await number":
                        if (token.Type == "operator")
                        {
                            if (token.Value == "-")
                            {
                                _parsingState = "await negative number";
                                break;
                            }

                            throw new System.ArgumentException("An operator can only follow a valid number or bracket");
                        }
                        validPolish.Add(token);
                        _parsingState = "await operator";
                        break;

                    case "await negative number":
                        if (token.Type == "operator")
                        {
                            throw new System.ArgumentException("An operator can only follow a valid number or bracket");
                        }
                        validPolish.Add(NegativeTokenBuilder(token.Value));
                        _parsingState = "await operator";
                        break;

                    //Set this upcoming operator aside to compare later
                    case "await operator":
                        if (operationStack.Count == 0)
                        {
                            operationStack.Add(token);
                            _parsingState = "await number";
                            break;
                        }
                        operationStack.Add(token);
                        _parsingState = "operation check";
                        break;

                    //Set precedence of stored operators then add number
                    case "operation check":
                        bool newOperationTakesPrecedence = OperatorPrecedenceCheck(operationStack[0], operationStack[1]);
                        if (newOperationTakesPrecedence)
                        {
                            validPolish.Add(token);
                            validPolish.Add(operationStack[1]);
                            operationStack.RemoveAt(1);
                            _parsingState = "await operator";
                        }
                        else
                        {
                            validPolish.Add(operationStack[0]);
                            validPolish.Add(token);
                            operationStack.RemoveAt(0);
                            _parsingState = "await operator";
                        }
                        break;
                }
            }

            //Add any cached operation
            if (operationStack.Count > 0)
            {
                validPolish.Add(operationStack[0]);
            }

            ValidateOutputPolish(validPolish);
            return validPolish;
        }

        private Token NegativeTokenBuilder(string tokenValue)
        {
            var negativeToken = new Token();
            negativeToken.Type = "number";
            negativeToken.Value = $"-{tokenValue}";
            return negativeToken;
        }

        private void ValidateOutputPolish(List<Token> validPolish)
        {
            //Exception when tokens contain unclosed bracket
            if (_openBrackets != 0)
            {
                throw new System.ArgumentException("Input tokens contain unclosed brackets");
            }

            //A valid polish notation has at least three elements
            if (validPolish.Count < 2)
            {
                throw new System.ArgumentException("Input tokens must produce a polish of size 3 or more");
            }
        }

        private bool OperatorPrecedenceCheck(Token operatorFirst, Token operatorSecond)
        {
            if (operatorFirst.Precedence < operatorSecond.Precedence)
            {
                return true;
            }

            return false;
        }
    }
}
