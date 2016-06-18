namespace Core.Expr.Parse.Operators
{
    public class TernaryOperator : Operator
    {
        public TernaryOperator(string symbol, int precedence, TokenKind kind) 
                : base(symbol, symbol, precedence, false, kind)
        {
        }

        public override bool IsBinary
        {
            get
            {
                return false;
            }
        }

        public override bool IsUnary
        {
            get
            {
                return false;
            }
        }

    }
}