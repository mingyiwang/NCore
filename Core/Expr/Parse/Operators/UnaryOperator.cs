
namespace Core.Expr.Parse.Operators
{
    public class UnaryOperator : Operator
    {
        public UnaryOperator(string symbol, int precedence, TokenKind kind) 
             : base(symbol, string.Empty, precedence, false, kind)
        {
            Arguments = 1;
        }

        public override bool IsUnary
        {
            get
            {
                return true;
            }
        }

        public override bool IsBinary
        {
            get
            {
                return false;
            }
        }

    }
}