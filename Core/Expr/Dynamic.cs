using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Expr.Eval;
using Core.Expr.Helpers;
using Core.Expr.Parse;

namespace Core.Expr
{

    public sealed class Dynamic
    {

        public static object Evaluate(string expression)
        {
            return Evaluate(expression, EvaluationContext.Instance);
        }

        public static object Evaluate(string expression, EvaluationContext context)
        {
            var lambda = Compile(new ExpressionParser(context).Parse(new SourceReader(expression)));
            return lambda.Compile().DynamicInvoke();
        }

        public static T Evaluate<T>(string expression)
        {
            return Compile<T>(expression)();
        }

        public static T Evaluate<T>(string expression, EvaluationContext context)
        {
            return Compile<T>(expression, context)();
        }

        public static Func<T> Compile<T>(string source)
        {
            return Compile<T>(source, EvaluationContext.Instance);
        }

        public static Func<T> Compile<T>(string source, EvaluationContext context)
        {
            var lambda = Compile(new ExpressionParser(context).Parse(new SourceReader(source)));
            return (Func<T>) lambda.Compile();
        }

        private static LambdaExpression Compile(Queue<IToken> tokens)
        {
            var stack = new ExpressionStack(new Stack<Expression>());
            while(tokens.Count > 0)
            {
                var token = tokens.Dequeue();
                if(token.IsIdentifier)
                {
                    var iden = token.AsIdentifier();
                    if(iden.IsType)
                    {
                        stack.Push(Expression.Constant(iden.Type));
                    }
                    else if(iden.IsIstance)
                    {
                        stack.Push(Expression.Constant(iden.Value, iden.Type));
                    }
                }
                else if(token.IsOperator)
                {
                    var oper = token.AsOperator();
                    if(oper.IsMemberOperator)
                    {
                        var memberOperator = oper.AsMemberOperator();
                        var args = new List<Expression>();
                        if(memberOperator.IsMethodAccess)
                        {
                            if(memberOperator.Arguments > 0)
                            {
                                args = stack.Pop(memberOperator.Arguments);
                                args.Reverse();
                            }
                        }

                        var parent = stack.Pop();
                        stack.Push(DefaultEvaluator.Instance.EvalMemberAccess(memberOperator, parent, memberOperator.IsMethodAccess, args));
                    }
                    else
                    {
                        if(oper.IsBinary)
                        {
                            var args = stack.Pop(2);
                            args.Reverse();
                            stack.Push(DefaultEvaluator.Instance.EvalBinary(oper, args[0], args[1]));
                        }
                        else if(oper.IsUnary)
                        {
                            stack.Push(DefaultEvaluator.Instance.EvalUnary(oper, stack.Pop()));
                        }
                        else if(oper.IsTernary)
                        {
                            var args = stack.Pop(3);
                            args.Reverse();
                            stack.Push(DefaultEvaluator.Instance.EvalCondition(oper, args[0], args[1], args[2]));
                        }
                    }
                }
                else
                {
                    throw new Exception("unrecognised token :" + token.Name);
                }
            }
            return Expression.Lambda(stack.Pop());
        }

    }

}