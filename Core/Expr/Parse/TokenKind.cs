namespace Core.Expr.Parse {

    public enum TokenKind {

        Indentifier,
        StringLiteral,
        IntLiteral,
        Index,

        // Math
        LeftParent,
        RightParent,
        LeftBracket,
        RightBracket,
        MemberAccess,
        Not,
        Multiplication,
        Division,
        Modulus,
        Addition,
        Subtraction,

        GreaterThan,
        LessThan,
        GreaterOrEqualTo,
        LessOrEqualTo,
        Equal,
        NotEqual,

        LogicalAnd,
        LogicalOr,

        AndAlso,
        OrElse,

        Condition,
        Assignment

    }

}