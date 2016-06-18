
using System.Collections.Generic;

namespace Core.Expr.Parse {

    public interface IExpressionParser {

        Queue<IToken> Parse(SourceReader source);

    }
}