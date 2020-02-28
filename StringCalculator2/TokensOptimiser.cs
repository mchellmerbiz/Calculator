using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringCalculator2
{
    class TokensOptimiser
    {
        internal List<Token> OptimiseTokens(List<Token> input)
        {
            //TODO: probably shouldn't turn into a tree after every optimisation
            var tokenTree = LinkTokensAsTree(input);

            var parentTokenIndex = Enumerable.Range(0, tokenTree.Count)
             .Where(i => tokenTree[i].ChildTokens != null && tokenTree[i].Type != "function")
             .ToList();

            foreach (var parentIndex in parentTokenIndex)
            {
                var parent = tokenTree[parentIndex];
                if (BranchResolvesToZero(parent))
                {

                    parent.Value = "0";
                    parent.Type = "number";

                    //Clear out child branches of a zeroing operation
                    var subparents = new List<Token>();
                    foreach (var index in parentTokenIndex)
                    {
                        if (index == parentIndex)
                        {
                            break;
                        }
                        subparents.Add(tokenTree[index]);
                    }
                    foreach (var subparent in subparents)
                    {
                        tokenTree.RemoveAll(token => subparent.ChildTokens.Contains(token));
                    }
                    tokenTree.RemoveAll(token => parent.ChildTokens.Contains(token));

                    return OptimiseTokens(tokenTree);
                }

                if (BranchResolvesToExponent(parent))
                {
                    parent.Value = "^";
                    parent.ChildTokens[0].Value = "2";
                    parent.ChildTokens[0].Type = "number";

                    return OptimiseTokens(tokenTree);
                }

                if (BranchResolvesToOne(parent))
                {

                    parent.Value = "1";
                    parent.Type = "number";
                    tokenTree.RemoveAll(token => parent.ChildTokens.Contains(token));

                    return OptimiseTokens(tokenTree);
                }

                if (BranchExtendsExponent(parent))
                {
                    //Define new tokens for Extended Exponent
                    var oldExponent = parent.ChildTokens.Find(token => token.Value == "^");
                    var oldExponentDegree = oldExponent.ChildTokens[0];
                    var oldExponentNumber = oldExponent.ChildTokens[1];

                    //Update branch to extended exponent
                    try
                    {
                        var parentParentIndex = parentTokenIndex.Find(ind => ind > parentIndex);
                        var parentParent = tokenTree[parentParentIndex];
                        parentParent.ChildTokens[1] = oldExponent;
                    }
                    catch (Exception)
                    {

                        
                    }

                    var degreeAdjustment = 1;
                    if (parent.Value == "/")
                    {
                        degreeAdjustment = -1;
                    }
                    oldExponentDegree.Value = (float.Parse(oldExponentDegree.Value) + degreeAdjustment).ToString();
                    

                    //Prune dead branch children
                    tokenTree.RemoveAll(token => token == parent || token == parent.ChildTokens.Find(t => t.Type != "operation"));

                    return OptimiseTokens(tokenTree);
                }
            }

            return tokenTree;
        }

        private bool BranchResolvesToZero(Token parent)
        {
            if (parent.Type == "function")
            {
                return false;
            }

            var leftChild = parent.ChildTokens[0];
            var rightChild = parent.ChildTokens[1];
            if (parent.Value == "*")
            {
                if (leftChild.Value == "0" || rightChild.Value == "0")
                {
                    return true;
                }
            }
            if (parent.Value == "/")
            {
                if (leftChild.Value == "0")
                {
                    throw new NotSupportedException("Attempt to divide by zero");
                }
                if (rightChild.Value == "0")
                {
                    return true;
                }
            }

            return false;
        }

        private bool BranchExtendsExponent(Token parent)
        {
            if (parent.Type == "function")
            {
                return false;
            }
            var leftChild = parent.ChildTokens[0];
            var rightChild = parent.ChildTokens[1];
            if (parent.Value == "*" || parent.Value == "/")
            {
                if (leftChild.Value == "^")
                {
                    if (leftChild.ChildTokens[1].Value == rightChild.Value)
                    {
                        return true;
                    }
                }

                if (rightChild.Value == "^")
                {
                    if (rightChild.ChildTokens[1].Value == leftChild.Value)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool BranchResolvesToExponent(Token parent)
        {
            if (parent.Type == "function")
            {
                return false;
            }
            var leftChild = parent.ChildTokens[0];
            var rightChild = parent.ChildTokens[1];
            if (parent.Value == "*")
            {
                if (leftChild.Type == "operation" || rightChild.Type == "operation")
                {
                    return false;
                }
                if (leftChild.Type == "function" || rightChild.Type == "function")
                {
                    return false;
                }
                if (leftChild.Value == rightChild.Value)
                {
                    return true;
                }
            }

            return false;
        }

        private bool BranchResolvesToOne(Token parent)
        {
            if (parent.Type == "function")
            {
                return false;
            }
            var leftChild = parent.ChildTokens[0];
            var rightChild = parent.ChildTokens[1];
            if (parent.Value == "/")
            {
                if (leftChild.Type == "function" || rightChild.Type == "function")
                {
                    return false;
                }
                if (leftChild.Value == rightChild.Value)
                {
                    return true;
                }
            }
            if (parent.Value == "^")
            {
                if (leftChild.Type == "function" || rightChild.Type == "function")
                {
                    return false;
                }
                if (leftChild.Value == "0")
                {
                    return true;
                }
            }

            return false;
        }

        private List<Token> LinkTokensAsTree(List<Token> input)
        {
            var tokenStack = new Stack<Token>();

            foreach (var token in input)
            {
                switch (token.Type)
                {
                    default:
                        tokenStack.Push(token);
                        break;
                    case "operation":
                        token.ChildTokens = new List<Token> { tokenStack.Pop(), tokenStack.Pop() };
                        tokenStack.Push(token);
                        break;
                    case "function":
                        token.ChildTokens = new List<Token> { tokenStack.Pop() };
                        tokenStack.Push(token);
                        break;
                }

            }
            return input;
        }

        private List<int> BuildParentIndexList(List<Token> input)
        {
            var parentIndex = new List<int>();

            for (int i = 0; i < input.Count; i++)
            {
                switch (input[i].Type)
                {
                    case "operation":
                        parentIndex.Add(i);
                        parentIndex.Add(i);
                        break;
                }
            }

            return parentIndex;
        }
    }
}
