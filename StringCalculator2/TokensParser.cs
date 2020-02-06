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
            var bracketOperationStack = new List<List<Token>>();
            var functionCache = new List<Token>();
            var openFunctionBracket = false;

            foreach (var token in unsortedTokens)
            {
                //The system is set to build chains of operations and operands unless a bracket or function token is encountered
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
                            //We need to add a placeholder token to the function cache to track open brackets not associated with functions
                            if (openFunctionBracket)
                            {
                                openFunctionBracket = false;
                            }
                            else
                            {
                                functionCache.Add(token);
                            }
                            _openBrackets += 1;

                            bracketOperationStack.Add(CloneStack(operationStack));
                            operationStack.Clear();

                            _parsingState = "await number";
                        }
                        //Closing brackets should finish some sub polish somewhere, cached operators should be restored
                        else
                        {
                            _openBrackets -= 1;
                            if (operationStack.Count == 1)
                            {
                                validPolish.Add(operationStack[0]);
                            }
                            else if (operationStack.Count > 1)
                            {
                                bool lastOperationTakesPrecedence = OperatorPrecedenceCheck(operationStack[0], operationStack[1]);
                                if (lastOperationTakesPrecedence)
                                {
                                    validPolish.Add(operationStack[1]);
                                    validPolish.Add(operationStack[0]);
                                }
                                else
                                {
                                    validPolish.Add(operationStack[0]);
                                    validPolish.Add(operationStack[1]);
                                }
                                operationStack.Clear();
                            }
                            if (bracketOperationStack.Count > 0)
                            {
                                operationStack = CloneStack(bracketOperationStack.Last());
                                bracketOperationStack.RemoveAt(bracketOperationStack.Count-1);
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
                        //If there are operations awaiting precedence check and the first operation wins it should be added before the function handling
                        if (operationStack.Count > 1)
                        {
                            bool cachedLatestOperationTakesPrecedence = OperatorPrecedenceCheck(operationStack[0], operationStack[1]);
                            if (!cachedLatestOperationTakesPrecedence)
                            {
                                validPolish.Add(operationStack[0]);
                                operationStack.RemoveAt(0);
                            }
                        }
                        openFunctionBracket = true;
                        break;

                    case "await number":
                        if (token.Type == "operation")
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
                        if (token.Type == "operation")
                        {
                            throw new System.ArgumentException("An operator can only follow a valid number or bracket");
                        }
                        validPolish.Add(NegativeTokenBuilder(token.Value));
                        _parsingState = "await operator";
                        break;

                    //Set this upcoming operator aside to compare later
                    case "await operator":
                        if (token.Type == "number")
                        {
                            Console.WriteLine($"Two number tokens retrieved in succession, {validPolish.Last().Value} taken and {token.Value} discarded.");
                            break;
                        }
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
                        
                        if (token.Type == "operation")
                        {
                            if (token.Value == "-")
                            {
                                _parsingState = "await negative number";
                            }
                            else
                            {
                                throw new System.ArgumentException("An operator can only follow a valid number or bracket");
                            }
                        }

                        //Add correct operation and new number token, unless a negative number is indicated by a '-' operator token
                        if (newOperationTakesPrecedence)
                        {
                            if (_parsingState != "await negative number")
                            {
                                validPolish.Add(token);
                                _parsingState = "await operator";
                            }
                            validPolish.Add(operationStack[1]);
                            operationStack.RemoveAt(1);
                        }
                        else
                        {
                            validPolish.Add(operationStack[0]);
                            if (_parsingState != "await negative number")
                            {
                                validPolish.Add(token);
                                _parsingState = "await operator";
                            }
                            operationStack.RemoveAt(0);
                        }
                        break;
                }
            }

            //Add any cached operation
            if (operationStack.Count == 1)
            {
                validPolish.Add(operationStack[0]);
            }
            else if (operationStack.Count > 1)
            {
                bool lastOperationTakesPrecedence = OperatorPrecedenceCheck(operationStack[0], operationStack[1]);
                if (lastOperationTakesPrecedence)
                {
                    validPolish.Add(operationStack[1]);
                    validPolish.Add(operationStack[0]);
                }
                else
                {
                    validPolish.Add(operationStack[0]);
                    validPolish.Add(operationStack[1]);
                }
            }

            ValidateOutputPolish(validPolish);
            return validPolish;
        }

        private List<Token> CloneStack(List<Token> operationStack)
        {
            var stackClone = new List<Token>();
            foreach (var tok in operationStack)
            {
                var clone = new Token(){Precedence = tok.Precedence, TokenId = tok.TokenId, Type = tok.Type, Value = tok.Value};
                stackClone.Add(clone);
            }

            return stackClone;
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
