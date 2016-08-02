using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Collection;

namespace Core
{
    public static class Predicates
    {
        public static Expression<Func<T, bool>> False<T>() { return x => false; }
        public static Expression<Func<T, bool>> True<T>() { return x => true; }

        public static Func<T, bool> And<T>(IEnumerable<Expression<Func<T, bool>>> exprs)
        {
            var exp = True<T>();
            exprs.ForEach(expression => {
                exp = exp.And(expression);
            });
            return exp.Compile();
        }

        public static Func<T, bool> Any<T>(IEnumerable<Expression<Func<T, bool>>> exprs)
        {
            var exp = False<T>();
            exprs.ForEach(expression => { exp = exp.Any(expression); });
            return exp.Compile();
        }

        public static Expression<Func<T, bool>> Any<T>(this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>(
                   Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters
            );
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>(
                   Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters
            );
        }


    }
}