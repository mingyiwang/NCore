using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Core.Expr.Parse;

namespace Core.Expr.Eval {

    public abstract class BaseEvaluator : IEvaluator {

        public Expression EvalType(IToken token) {
            var iden = token.AsIdentifier();
            return Expression.Constant(iden.Type);
        }

        public Expression EvalInstance(IToken token) {
            var iden = token.AsIdentifier();
            return Expression.Constant(iden.Value, iden.Type);
        }

        public Expression EvalUnary(IToken token, Expression operand) {
            if (token.Kind == TokenKind.Not) {
                return Expression.Not(operand);
            }

            throw new NotSupportedException();
        }

        public Expression EvalBinary(IToken token, Expression left, Expression right) {
            switch (token.Kind) {
                case TokenKind.Addition         : return EvalAddition(left, right);
                case TokenKind.Subtraction      : return Expression.Subtract(left, right);
                case TokenKind.Multiplication   : return Expression.Multiply(left, right);
                case TokenKind.Division         : return Expression.Divide(left, right);
                case TokenKind.Modulus          : return Expression.Modulo(left, right);
                case TokenKind.GreaterThan      : return Expression.GreaterThan(left, right);
                case TokenKind.LessThan         : return Expression.LessThan(left, right);
                case TokenKind.GreaterOrEqualTo : return Expression.GreaterThanOrEqual(left, right);
                case TokenKind.LessOrEqualTo    : return Expression.LessThanOrEqual(left, right);
                case TokenKind.Equal            : return Expression.Equal(left, right);
                case TokenKind.NotEqual         : return Expression.NotEqual(left, right);
                case TokenKind.LogicalAnd       : return Expression.And(left, right);
                case TokenKind.LogicalOr        : return Expression.Or(left, right);
                case TokenKind.AndAlso          : return Expression.AndAlso(left, right);
                case TokenKind.OrElse           : return Expression.OrElse(left, right);
                case TokenKind.Assign           : return Expression.Assign(left, right);
            }

            throw new NotSupportedException();
        }

        public Expression EvalCondition(IToken token, Expression condition, Expression first, Expression second) {

            if (condition.Type != typeof(bool)) {
                throw new Exception("Condition type must be true.");
            }

            if (first.Type == second.Type) {
                return Expression.Condition(condition, first, second);
            }

            if (first.Type.IsAssignableFrom(second.Type)) {
                return Expression.Condition(condition, first, Expression.Convert(second, first.Type));
            }

            if (second.Type.IsAssignableFrom(first.Type)) {
                return Expression.Condition(condition, Expression.Convert(first, second.Type), second);
            }

            throw new Exception("Type is not compatible.");
        }

        public Expression EvalMemberAccess(IToken token, Expression parent, bool isMethod, List<Expression> arguments) {
            var constants = parent as ConstantExpression;
            if (constants != null) {
                var type = constants.Value as Type;
                if (type != null) {
                    return isMethod ? MakeStaticMethodAccess(type, token.Name, arguments)
                                    : MakeStaticMemberAccess(type, token.Name);
                }

                var method = constants.Type.GetMethod(token.Name, arguments.Select(e => e.Type).ToArray());
                return isMethod ? MakeMethodAccess(parent, method, arguments)
                                : Expression.PropertyOrField(parent, token.Name);

            }

            var member = parent as MemberExpression;
            if (member != null) {
                if (isMethod) {
                    var @params = arguments.ToList();
                    var method = member.Type.GetMethod(token.Name, @params.Select(e => e.Type).ToArray());
                    return MakeMethodAccess(member, method, @params);
                }

                var memberInfo = member.Type.GetMember(token.Name);
                return Expression.MakeMemberAccess(member, memberInfo[0]);
            }

            var methodCall = parent as MethodCallExpression;
            if (methodCall != null) {
                if (isMethod) {
                    var method = methodCall.Type.GetMethod(token.Name, arguments.Select(e => e.Type).ToArray());
                    return MakeMethodAccess(methodCall, method, arguments);
                }

                var memberInfo = methodCall.Type.GetMember(token.Name);
                return Expression.MakeMemberAccess(methodCall, memberInfo[0]);
            }

            throw new ArgumentException("Unexpected Parent Expression : " + parent);
        }

        private static Expression MakeStaticMemberAccess(Type type, string memberName) {
            return Expression.Property(null, type, memberName);
        }

        private static Expression MakeStaticMethodAccess(Type type, string methodName, List<Expression> parameters) {
            var method = type.GetMethod(methodName, parameters.Select(e => e.Type).ToArray());
            return Expression.Call(method, parameters);
        }

        private static Expression MakeMethodAccess(Expression parent, MethodInfo methodInfo, List<Expression> expressions) {
            return Expression.Call(parent, methodInfo, expressions);
        }

        private static Expression EvalAddition(Expression left, Expression right) {
            // should based on TypeConverter
            if (left.Type == typeof(string) || right.Type == typeof(string)) {
                return Expression.Add(left, right, typeof(string).GetMethod("Concat", new[] {
                       typeof(string),
                       typeof(string)
                }));
            }

            return Expression.Add(left, right);
        }


    }

}