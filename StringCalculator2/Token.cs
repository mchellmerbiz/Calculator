namespace StringCalculator2
{
    class Token
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
                }
            }
        }
        public string Type { get; set; }
        public int Precedence { get; set; }
    }
}