namespace Core.Expr.Parse.Operators {

    public class ParentOperator : Operator {

        public override bool IsBinary => false;
        public override bool IsUnary => false;

        public ParentOperator(string symbol, TokenKind kind) : base(symbol, string.Empty, int.MaxValue, false, kind) {
            Arguments = 0;
        }


    }

}