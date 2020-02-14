using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringCalculator2
{
    class TokenCalculator
    {
        private List<string> _operandTypes = new List<string>() { "number", "variable" };
        private List<TokenVariableMap> VariableMap { get; set; }
        public string EvaluateReversePolishExpression(List<Token> inputExpression)
        {
            var tokenStack = new List<Token>();
            string evaluation = "initial";
            foreach (var token in inputExpression)
            {
                var tokenState = token.Type;
                if (evaluation == "initial")
                {
                    tokenState = "initial";
                }

                switch (tokenState)
                {
                    case "initial":
                        if (token.Type == "variable")
                        {
                            evaluation = GetVariableValue(token.Value);
                            break;
                        }
                        else if (token.Type != "number")
                        {
                            throw new ArgumentException($"Invalid Input: Expected a number, got a {token.Type}, {token.Value}");
                        }

                        evaluation = token.Value;
                        break;
                    case "number":
                        tokenStack.Add(token);
                        break;

                    case "operation":
                        if (tokenStack.Count < 1)
                        {
                            throw new ArgumentException("Invalid Input: Operation has no two preceding tokens to operate on.");
                        } else if (tokenStack.Count == 1)
                        {
                            var newValue = tokenStack[tokenStack.Count - 1].Value;
                            tokenStack.RemoveRange(tokenStack.Count - 1, 1);
                            evaluation = UpdateEvaluation(evaluation, newValue, token);
                            break;
                        }
                        else
                        {
                            var interimValue = UpdateEvaluation(tokenStack[tokenStack.Count - 2].Value, tokenStack[tokenStack.Count - 1].Value, token);
                            var holderToken = new Token() {Value = interimValue, Type = "number"};
                            tokenStack.RemoveRange(tokenStack.Count - 2, 2);
                            tokenStack.Insert(0,holderToken);
                            break;
                        }

                    case "function":
                        if (tokenStack.Count < 1)
                        {
                            evaluation = UpdateEvaluation(evaluation, token);
                            break;
                        }
                        else
                        {
                            var newValue = UpdateEvaluation(tokenStack[tokenStack.Count - 1].Value, token);
                            var holderToken = new Token() { Value = newValue, Type = "number" };
                            tokenStack.RemoveRange(tokenStack.Count - 1, 1);
                            tokenStack.Add(holderToken);
                            break;
                        }

                    case "variable":
                        var replacementToken = new Token() { Type = "number", Value = GetVariableValue(token.Value) };
                        tokenStack.Add(replacementToken);
                        break;
                }
            }
            return evaluation;
        }

        private string GetVariableValue(string value)
        {
            return VariableMap.Find(tokenMap => value == tokenMap.VariableAlias).VariableValue;
        }

        public void BuildVariableMap(string varAlias, string varValue)
        {
            VariableMap.Add(new TokenVariableMap() { VariableAlias = varAlias, VariableValue = varValue });
        }

        private string UpdateEvaluation(string valueOne, string valueTwo, Token operation)
        {
            float floatOne = float.Parse(valueOne);
            float floatTwo = float.Parse(valueTwo);
            switch (operation.Value)
            {
                case "+":
                    return (floatOne + floatTwo).ToString();
                case "-":
                    return (floatOne - floatTwo).ToString();
                case "*":
                    return (floatOne * floatTwo).ToString();
                case "/":
                    return (floatOne / floatTwo).ToString();
                case "^":
                    return Math.Pow(floatOne, floatTwo).ToString();
            }

            return "";
        }

        private string UpdateEvaluation(string valueOne, Token operation)
        {
            float floatOne = float.Parse(valueOne);
            switch (operation.Value)
            {
                case "sin":
                    return Math.Sin(floatOne).ToString();
                case "cos":
                    return Math.Cos(floatOne).ToString();
                case "tan":
                    return Math.Tan(floatOne).ToString();
            }

            return "";
        }

        public TokenCalculator()
        {
            VariableMap = new List<TokenVariableMap>();
        }
    }
}