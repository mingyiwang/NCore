
namespace Core.Expr.Parse.Operators
{
    public class MemberOperator : Operator
    {
        public bool IsPropertyAccess { get; set; }
        public bool IsMethodAccess   { get; set; }
        public bool IsIndexAccess    { get; set; }

        public MemberOperator(string symbol, int precedance) : base(symbol, string.Empty, precedance, true, TokenKind.MemberAccess)
        {
            Arguments = 0;
            IsPropertyAccess = true;
            IsMethodAccess = false;
            IsIndexAccess  = false;
        }

        public override bool IsBinary
        {
            get { return false; }
        }

        public override bool IsUnary
        {
            get { return false; }
        }
    }
}