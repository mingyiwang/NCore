namespace Core.Expr.Parse.Operators {

    public class BinaryOperator : Operator {
        
        public BinaryOperator(string symbol, int precedence, bool isLeftAssociate, TokenKind kind) 
             : base(symbol, string.Empty, precedence, isLeftAssociate, kind) {
             Arguments = 2;
        }

        public override bool IsBinary => true;
        public override bool IsUnary  => false;

    }

}