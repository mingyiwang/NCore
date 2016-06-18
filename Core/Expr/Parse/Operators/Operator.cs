
namespace Core.Expr.Parse.Operators
{

    public abstract class Operator : IToken
    {
        public Location Location    { get; set; }
        public TokenKind Kind       { get; set; }

        public string Symbol        { get; set; }
        public string Name          { get; set; }

        public int  Precedence      { get; set; }
        public bool IsLeftAsscoiate { get; set; }
        public int  Arguments       { get; set; }

        public abstract bool IsBinary { get; }
        public abstract bool IsUnary  { get; }
       
        protected Operator(string symbol, string name, int precedence, bool isLeftAssociate, TokenKind kind)
        {
            Symbol = symbol;
            Name = name;
            Precedence = precedence;
            Kind = kind;
            IsLeftAsscoiate = isLeftAssociate;
            Location = Location.Nil;
        }

        public Operator AsOperator()
        {
            return this;
        }

        public Identifier AsIdentifier()
        {
            return null;
        }

        public MemberOperator AsMemberOperator()
        {
            return this as MemberOperator;
        }

        public bool IsMemberOperator
        {
            get
            {
                return Kind == TokenKind.MemberAccess;
            }
        }

        public bool IsOperator
        {
            get
            {
                return true;
            }
        }

        public bool IsIdentifier
        {
            get
            {
                return false;
            }
        }

        public bool IsParent
        {
            get
            {
                return IsLeftParent || IsRightParent;
            }
        }

        public bool IsLeftParent
        {
            get
            {
                return Kind == TokenKind.LeftParent;
            }
        }

        public bool IsRightParent
        {
            get
            {
                return Kind == TokenKind.RightParent;
            }
        }

        public bool IsTernary
        {
            get { return Kind == TokenKind.Condition; }
        }

    }

}