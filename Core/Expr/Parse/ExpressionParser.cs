using System;
using System.Collections.Generic;
using System.Text;
using Core.Expr.Eval;
using Core.Expr.Helpers;
using Core.Expr.Parse.Operators;
using Core.Primitive;

namespace Core.Expr.Parse
{

    public class ExpressionParser : IExpressionParser
    {

        private SourceReader _source;
        private readonly EvaluationContext _context;
        private readonly Queue<IToken>   _tokenQueue = new Queue<IToken>();
        private readonly Stack<Operator> _operators  = new Stack<Operator>();

        public ExpressionParser(EvaluationContext context)
        {
            _context = context;
        }

        public Queue<IToken> Parse(SourceReader source)
        {
            source.Reset();
            _source = source;
            _tokenQueue.Clear();
            InternalParse();
            return new Queue<IToken>(_tokenQueue);
        }

        private bool IsInBounds()
        {
            return _source.IsInBound;
        }

        private void InternalParse()
        {
            _tokenQueue.Clear();
            _source.Run(reader =>
            {
                var current = reader.GetCurrentChar();
                if(IsOperator())
                {
                    ParseOperator();
                }
                else if(ParseHelper.IsSingleQuote(current))
                {
                    ParseStringLiteral();
                }
                else if(ParseHelper.IsIdentifierStart(current))
                {
                    ParseIdentifier();
                }
                else if(ParseHelper.IsNumber(current))
                {
                    ParseNumber();
                }
                else if(ParseHelper.IsArgumentSeperator(current))
                {
                    while (_operators.Count > 0)
                    {
                        var oper = _operators.Peek();
                        if(!oper.IsLeftParent)
                        {
                            _tokenQueue.Enqueue(_operators.Pop());
                        }

                        var lParen = _operators.Pop();
                        var op = _operators.Peek();
                        if(op.IsMemberOperator)
                        {
                            var member = op.AsMemberOperator();
                            member.IsMethodAccess = true;
                        }

                        _operators.Push(lParen);
                        break;
                    }
                }
                else if(ParseHelper.IsQuestionMark(current))
                {
                    var oper = _context.GetTernaryOperator();
                    oper.Location = new Location(_source.Position, _source.Position);
                    while(_operators.Count > 0)
                    {
                        var op = _operators.Peek();
                        if(oper.Precedence < op.Precedence)
                        {
                            _tokenQueue.Enqueue(_operators.Pop());
                        }
                        else
                        {
                            break;
                        }
                    }
                    _operators.Push(oper);
                }
                else if(ParseHelper.IsSemiColon(current))
                {
                    while(_operators.Count > 0)
                    {
                        var op = _operators.Peek();
                        if(op.IsTernary)
                        {
                            break;
                        }
                        _tokenQueue.Enqueue(_operators.Pop());
                    }
                }
            });

            while(_operators.Count > 0)
            {
                var o = _operators.Pop();
                if(o == null)
                {
                    continue;
                }

                if(o.IsParent)
                {
                    throw new Exception($"no match for parenthese at location [{o.Location.StartIndex}, {o.Location.EndIndex}]");

                }
                _tokenQueue.Enqueue(o);
            }

        }

        private void ParseIdentifier()
        {
            var stringBuilder = new StringBuilder(10);
            stringBuilder.Append(_source.GetCurrentChar());

            var start = _source.Position;
            var last = start;

            while(IsInBounds())
            {
                var next = _source.ViewNextChar();
                if(ParseHelper.IsPartOfIdentifier(next))
                {
                    stringBuilder.Append(next);
                    _source.MoveNext();
                    last = _source.Position;
                }
                else
                {
                    break;
                }
            }

            var token = stringBuilder.ToString();
            if(_context.ContainsType(token))
            {
                var item = _context.GetItem(token);
                if(item.IsType)
                {
                    _tokenQueue.Enqueue(new TypeToken(token, item.Type, new Location(start, last)));
                }
                else if(item.IsInstance)
                {
                    _tokenQueue.Enqueue(new InstanceToken(token, item.Instance, new Location(start, last)));
                }
            }
            else
            {
                throw new Exception(string.Format("unrecognised type {0} at {1}-{2}", token, start, last));
            }
        }

        private void ParseMemberAccess()
        {
            var start = _source.Position;
            var last = start;

            var stringBuilder = new StringBuilder(10);
            while(IsInBounds())
            {
                var next = _source.ViewNextChar();
                if(ParseHelper.IsPartOfIdentifier(next))
                {
                    stringBuilder.Append(next);
                    last = _source.Position;
                    _source.MoveNext();
                }
                else
                {
                    break;
                }
            }

            while(_operators.Count > 0)
            {
                var op = _operators.Peek();
                if(op.IsMemberOperator)
                {
                    _tokenQueue.Enqueue(_operators.Pop());
                }
                else
                {
                    break;
                }
            }

            var oper = _context.GetMemberOperator();
            oper.Name = Strings.Of(stringBuilder.ToString());
            oper.Location = new Location(start, last);
            _operators.Push(oper);
        }

        private void ParseLeftParent()
        {
            var oper = _context.GetLeftParentOperator();
            oper.Location = new Location(_source.Position, _source.Position);
            _operators.Push(oper);
        }

        private void ParseRightParent()
        {
            if(_operators.Count == 0)
            {
                throw new Exception("no matched parenthese.");
            }

            var endIndex = _source.Position;
            while(_operators.Count > 0)
            {
                var o = _operators.Peek();
                if(o.IsLeftParent)
                {
                    var startIndex = o.Location.StartIndex;
                    _operators.Pop();

                    if(_operators.Count == 0)
                    {
                        break;
                    }

                    var op = _operators.Peek();
                    if(op.IsMemberOperator)
                    {
                        var memeber = op.AsMemberOperator();
                        memeber.IsMethodAccess = true;
                        memeber.Arguments = Strings.Split(_source.Between(startIndex, endIndex).Trim(), ',').Length;
                    }
                    break;
                }

                _tokenQueue.Enqueue(_operators.Pop());
            }
        }

        private void ParseNumber()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(_source.GetCurrentChar());

            var start = _source.Position;
            var last = _source.Position;

            var isDecimal = false;
            while(IsInBounds())
            {
                var c = _source.ViewNextChar();
                if(ParseHelper.IsPartOfNumber(c))
                {
                    if(ParseHelper.IsFullStop(c))
                    {
                        if(isDecimal)
                        {
                            break;
                        }

                        isDecimal = true;
                    }

                    last = _source.Position;
                    stringBuilder.Append(c);
                    _source.MoveNext();
                }
                else
                {
                    break;
                }
            }

            var token = stringBuilder.ToString();
            try
            {
                if(isDecimal)
                {
                    _tokenQueue.Enqueue(new InstanceToken(token, double.Parse(token), new Location(start, last)));
                }
                else
                {
                    _tokenQueue.Enqueue(new InstanceToken(token, int.Parse(token), new Location(start, last)));
                }

            }
            catch(Exception)
            {
                throw new Exception("parse number failed, suffix is not supported yet");
            }
        }

        private void ParseStringLiteral()
        {
            var stringParsed = false;
            var stringBuilder = new StringBuilder(10);
            var start = _source.Position;
            var last = start;

            while(IsInBounds())
            {
                var next = _source.ViewNextChar();
                if(ParseHelper.IsSingleQuote(next))
                {
                    last = _source.Position;
                    stringParsed = true;
                    _source.MoveNext();
                    break;
                }

                stringBuilder.Append(next);
                _source.MoveNext();
            }

            if(!stringParsed)
            {
                throw new Exception($"single quote at position : {start} doesn't have match");
            }

            var value = stringBuilder.ToString();
            _tokenQueue.Enqueue(new InstanceToken(value, value, new Location(start, last)));
        }

        private bool IsOperator()
        {
            var flag = false;
            var start = _source.Position;
            var current = _source.GetCurrentChar();

            var builder = new StringBuilder(5);
            builder.Append(current);

            while(IsInBounds() && _context.ContainsOperator(builder.ToString()))
            {
                flag = true;
                builder.Append(_source.GetNextChar());
            }

            _source.Reset(start);
            return flag;
        }

        private void ParseOperator()
        {
            var current = _source.GetCurrentChar();

            if(ParseHelper.IsFullStop(current))
            {
                ParseMemberAccess();
                return;
            }

            if(ParseHelper.IsLeftParent(current))
            {
                ParseLeftParent();
                return;
            }

            if(ParseHelper.IsRightParent(current))
            {
                ParseRightParent();
                return;
            }

            var builder = new StringBuilder(5);
            builder.Append(current);

            var start = _source.Position;
            var last = start;

            var symbol = builder.ToString();
            while(IsInBounds() && _context.ContainsOperator(builder.ToString()))
            {
                last = _source.Position;
                symbol = builder.ToString();
                builder.Append(_source.GetNextChar());
            }

            var oper = _context.GetOperator(symbol);
            if(oper == null)
            {
                throw new Exception($"unrecognised operator at [{_source.Position}]");
            }

            _source.Reset(_source.Position - 1);

            while(_operators.Count > 0)
            {
                var op = _operators.Peek();
                if(op == null)
                {
                    _operators.Pop();
                    continue;
                }

                if(op.IsParent)
                {
                    break;
                }

                if ((oper.IsLeftAsscoiate   && oper.Precedence <= op.Precedence)
                || ((!oper.IsLeftAsscoiate) && oper.Precedence < op.Precedence))
                {
                    _tokenQueue.Enqueue(_operators.Pop());
                }
                else
                {
                    break;
                }
            }

            oper.Location = new Location(start, last);
            _operators.Push(oper);
        }

        public string ToRPNString()
        {
            var builder = new StringBuilder();
            while(_tokenQueue.Count > 0)
            {
                var token = _tokenQueue.Dequeue();
                if(token.IsOperator)
                {
                    var op = token.AsOperator();
                    if(op.IsMemberOperator)
                    {
                        var memberOper = op.AsMemberOperator();
                        if(memberOper.IsMethodAccess)
                        {
                            builder.Append(op.Name + "(" + op.Arguments + ")");
                        }
                        else
                        {
                            builder.Append("[" + op.Name + "]");
                        }
                    }
                    else
                    {
                        builder.Append(op.Symbol);
                    }
                }
                else
                {
                    builder.Append(token.Name + "[" + token.AsIdentifier().Type.Name + "]");
                }
            }

            return builder.ToString();
        }

    }

}