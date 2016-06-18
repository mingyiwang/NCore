using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Expr.Helpers {

    public sealed class ExpressionStack {

        private readonly Stack<Expression> _stack;

        public int Count => _stack.Count;

        public ExpressionStack(Stack<Expression> stack) {
            Preconditions.CheckNotNull(stack);
            _stack = stack;
        }

        public Expression Pop() {
            return _stack.Pop();
        }

        public List<Expression> Pop(int n) {
            if (n == 0) {
                return new List<Expression>();
            }

            var lists = new List<Expression>();
            while (_stack.Count > 0 && lists.Count < n) {
                lists.Add(_stack.Pop());
            }

            if (lists.Count != n) {
                throw new ArgumentOutOfRangeException();
            }

            return lists;
        }

        public Expression Peek() {
            return _stack.Peek();
        }

        public void Push(Expression expr) {
            _stack.Push(expr);
        }

        public void Clear() {
            _stack.Clear();
        }


    }
}