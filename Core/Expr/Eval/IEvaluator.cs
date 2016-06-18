using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Expr.Parse;

namespace Core.Expr.Eval {

    public interface IEvaluator {

        Expression EvalType(IToken token);
        Expression EvalInstance(IToken token);
        Expression EvalBinary(IToken token, Expression left, Expression right);
        Expression EvalUnary(IToken token,  Expression operand);
        Expression EvalCondition(IToken token, Expression condition, Expression first, Expression second);
        Expression EvalMemberAccess(IToken token, Expression parent, bool isMethod, List<Expression> arguments);

    }

}