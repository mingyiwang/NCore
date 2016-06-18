using System;
using Core.Expr.Parse.Operators;

namespace Core.Expr.Parse {

    public abstract class Identifier : IToken {

        public string Name  { get; set; }
        public object Value { get; set; }
        public Type Type    { get; set; }

        public Location Location { get; set; }
        public abstract bool IsIstance { get; }
        public abstract bool IsType { get; }

        public TokenKind Kind {
            get {
                return TokenKind.Indentifier;
            }
        }

        public bool IsOperator {
            get {
                return false;
            }
        }

        public bool IsIdentifier {
            get {
                return true;
            }
        }

        public Identifier AsIdentifier() {
            return this;
        }

        public Operator AsOperator() {
            return null;
        }

    }

    public class TypeToken : Identifier {
        public TypeToken(string name, Type type, Location location) {
            Name = name;
            Type = type;
            Location = location;
            Value = type;
        }

        public override bool IsType {
            get {
                return true;
            }
        }

        public override bool IsIstance {
            get {
                return false;
            }
        }

    }

    public class InstanceToken : Identifier {

        public InstanceToken(string name, object value, Location location) {
            Type     = value.GetType();
            Name     = name;
            Location = location;
            Value    = value;
        }

        public override bool IsType {
            get {
                return false;
            }
        }

        public override bool IsIstance {
            get {
                return true;
            }
        }


    }

}