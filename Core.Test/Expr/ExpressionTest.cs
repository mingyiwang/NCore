using Core.Expr;
using Core.Expr.Eval;
using Core.Expr.Parse;

namespace Core.Test.Expr {

    public class ExpressionTest {

        public void TestScan() {
            var context = new EvaluationContext();
            context.RegisterInstance("fn", new Function());
            var parser = new ExpressionParser(context);
            parser.Parse(new SourceReader("false ? true || false : false"));
        }

        public void TestEvaluate1() {
            var context = new EvaluationContext();
            context.RegisterInstance("fn", new Function());
            var expr = Dynamic.Evaluate("false ? true || false : false", context);
        }

        public class Function {
            public B B => new B();

            public string Value => "Value";

            public string OutPut() {
                return "fn.OutPut()";
            }

            public B GetB() {
                return new B();
            }
        }

        public class B {

            public C C => new C();

            public string Value => "mm-DD-yyyy";

            public string ToStringB() {
                return Value;
            }

            public string ToStringB(string content) {
                return content;
            }

            public C GetC() {
                return C;
            }
        }

        public class C {

            public string Value => "C";

            public string ToStringC() {
                return Value;
            }

            public string ToStringC(string content) {
                return content;
            }
        }

    }
}