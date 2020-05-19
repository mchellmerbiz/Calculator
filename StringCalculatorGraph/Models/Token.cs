using System.Collections.Generic;

namespace StringCalculatorGraph.Models
{
    public class Token
    {
        private string _value;
        public int TokenId { get; set; }
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                if (value == "+" || value == "-")
                {
                    Precedence = 1;
                } else if (value == "*" || value == "/")
                {
                    Precedence = 2;
                } else if (value == "^")
                {
                    Precedence = 3;
                }
            }
        }
        public string Type { get; set; }
        public int Precedence { get; set; }
        public List<Token> ChildTokens { get; set; }
    }
}