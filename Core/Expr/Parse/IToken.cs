using Core.Expr.Parse.Operators;

namespace Core.Expr.Parse {

    public interface IToken
    {
        string     Name     { get; }
        Location   Location { get; }
        TokenKind  Kind     { get; }

        bool IsOperator     { get; }
        bool IsIdentifier   { get; }

        Identifier AsIdentifier();
        Operator   AsOperator();

    }

}