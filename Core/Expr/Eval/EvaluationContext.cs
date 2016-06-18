using System;
using Core.Collection;
using Core.Expr.Parse;
using Core.Expr.Parse.Operators;

namespace Core.Expr.Eval {

    public class EvaluationContext {

        public static EvaluationContext Instance = new EvaluationContext();

        private readonly HashMap<string, Item> _typeRepository;
        private readonly HashMap<string, Func<Operator>> _operatorRepository;

        public EvaluationContext() {
            _typeRepository = new HashMap<string, Item>();
            _operatorRepository = new HashMap<string, Func<Operator>>();
            Init();
        }

        public EvaluationContext RegisterType(string name, Type type) {
            _typeRepository.Put(name, new TypeItem(name, type));
            return this;
        }

        public EvaluationContext RegisterInstance(string name, object instance) {
            _typeRepository.Put(name, new InstanceItem(name, instance));
            return this;
        }

        public EvaluationContext RegisterOperator(string symbol, Func<Operator> oper) {
            _operatorRepository.Put(symbol, oper);
            return this;
        }

        public bool ContainsType(string identifer) {
            return _typeRepository.ContainsKey(identifer);
        }

        public Item GetItem(string identifier) {
            return _typeRepository.Get(identifier);
        }

        public bool ContainsOperator(string symbol) {
            return _operatorRepository.ContainsKey(symbol);
        }

        public Operator GetOperator(string identifer) {
            return _operatorRepository.Get(identifer)();
        }

        public MemberOperator GetMemberOperator() {
            var oper = GetOperator(".");
            return oper as MemberOperator;
        }

        public TernaryOperator GetTernaryOperator() {
            var oper = GetOperator("?:");
            return oper as TernaryOperator;
        }

        public ParentOperator GetLeftParentOperator() {
            var oper = GetOperator("(");
            return oper as ParentOperator;
        }

        public ParentOperator GetRightParentOperator() {
            var oper = GetOperator(")");
            return oper as ParentOperator;
        }

        private void Init() {
            RegisterOperator("(",  () => new ParentOperator("(", TokenKind.LeftParent));
            RegisterOperator(")",  () => new ParentOperator(")", TokenKind.RightParent));

            // This is for index
            RegisterOperator("[",  () => new ParentOperator("[", TokenKind.LeftBracket));
            RegisterOperator("]",  () => new ParentOperator("]", TokenKind.RightBracket));

            RegisterOperator(".",  () => new MemberOperator(".", 12));
            RegisterOperator("!",  () => new UnaryOperator("!",  11, TokenKind.Not));
            RegisterOperator("*",  () => new BinaryOperator("*", 10, true, TokenKind.Multiplication));
            RegisterOperator("/",  () => new BinaryOperator("/", 10, true, TokenKind.Division));
            RegisterOperator("%",  () => new BinaryOperator("%", 10, true, TokenKind.Modulus));

            RegisterOperator("+",  () => new BinaryOperator("+", 9,  true, TokenKind.Addition));
            RegisterOperator("-",  () => new BinaryOperator("-", 9,  true, TokenKind.Subtraction));

            RegisterOperator(">",  () => new BinaryOperator(">",  8, true, TokenKind.GreaterThan));
            RegisterOperator("<",  () => new BinaryOperator("<",  8, true, TokenKind.LessThan));
            RegisterOperator(">=", () => new BinaryOperator(">=", 8, true, TokenKind.GreaterOrEqualTo));
            RegisterOperator("<=", () => new BinaryOperator("<=", 8, true, TokenKind.LessOrEqualTo));

            RegisterOperator("!=", () => new BinaryOperator("!=", 7, true, TokenKind.NotEqual));
            RegisterOperator("==", () => new BinaryOperator("==", 7, true, TokenKind.Equal));

            RegisterOperator("&",  () => new BinaryOperator("&", 6, true, TokenKind.LogicalAnd));
            RegisterOperator("|",  () => new BinaryOperator("|", 5, true, TokenKind.LogicalOr));

            RegisterOperator("&&", () => new BinaryOperator("&&",  4, true, TokenKind.AndAlso));
            RegisterOperator("||", () => new BinaryOperator("||",  3, true, TokenKind.OrElse));
            RegisterOperator("?:", () => new TernaryOperator("?:", 2, TokenKind.Condition));
            RegisterOperator("=",  () => new BinaryOperator("=",   1, false, TokenKind.Assign));

            RegisterInstance("true",  true);
            RegisterInstance("True",  true);
            RegisterInstance("false", false);
            RegisterInstance("False", false);

            RegisterType("null",     typeof(object));
            RegisterType("Convert",  typeof(Convert));
            RegisterType("DateTime", typeof(DateTime));
            RegisterType("Math",     typeof(Math));
            RegisterType("byte",     typeof(byte));
            RegisterType("long",     typeof(long));
            RegisterType("int",      typeof(int));
            RegisterType("short",    typeof(short));
            RegisterType("string",   typeof(string));
            RegisterType("bool",     typeof(bool));
            RegisterType("double",   typeof(double));
            RegisterType("float",    typeof(float));
            RegisterType("decimal",  typeof(decimal));

        }

        public abstract class Item {

            public string Identifier { get; set; }
            public Type   Type       { get; set; }
            public object Instance   { get; set; }

            public abstract bool IsType { get; }
            public abstract bool IsInstance { get; }

            public abstract TypeItem AsType();
            public abstract InstanceItem AsInstance();

        }

        public sealed class TypeItem : Item {

            public override bool IsInstance => false;
            public override bool IsType     => true;

            public TypeItem(string identifier, Type type) {
                Identifier = identifier;
                Type = type;
                Instance = null;
            }

            public override TypeItem AsType() {
                return this;
            }

            public override InstanceItem AsInstance() {
                return null;
            }
            
        }

        public sealed class InstanceItem : Item {

            public override bool IsInstance => true;
            public override bool IsType     => false;

            public InstanceItem(string identifier, object instance) {
                Identifier = identifier;
                Type = instance.GetType();
                Instance = instance;
            }

            public override TypeItem AsType() {
                return null;
            }

            public override InstanceItem AsInstance() {
                return this;
            }

        }

    }

}