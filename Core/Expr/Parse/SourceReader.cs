using System;
using Core.Expr.Helpers;
using Core.Primitive;

namespace Core.Expr.Parse {

    public class SourceReader {

        public static readonly char Eos = '\u0000';

        private readonly string _input;
        private int _pos;

        public SourceReader(string input) {
            Check.NotNull(input);
            _input = input;
            _pos = 0;
        }

        public string Source => _input;

        public int Position => _pos;

        public bool IsInBound => _pos < _input.Length;

        public void Run(Action<SourceReader> action) {
            while (IsInBound) {
                SkipWs();
                action(this);
                MoveNext();
            } 
        }

        public string Between(int startIndex, int endIndex) {
            return Strings.Between(_input, startIndex, endIndex);
        }

        // View Next 'n' char without changing position
        public char GetCurrentChar() {
            if (_pos < _input.Length) {
                return _input[_pos];
            }
            return Eos;
        }

        // read next char and move position
        public char GetNextChar() {
            MoveNext();
            if (IsInBound) {
                return _input[_pos];
            }
            return Eos;
        }

        public char ViewNextChar() {
            if (_pos + 1 < _input.Length) {
                return _input[_pos + 1];
            }
            return Eos;
        }

        public void MoveNext() {
            MoveNext(1);
        }

        // move position by n
        public void MoveNext(int n) {
            _pos += n;
        }

        public void SkipWs() {
            while (IsInBound) {
                var c = GetCurrentChar();
                if (ParseHelper.IsWhitespace(c)) {
                    MoveNext(1);
                }
                else {
                    break;
                }
            }
        }

        public void Reset() {
            Reset(0);
        }

        public void Reset(int position) {
            _pos = position;
        }

        public void Back() {
            _pos = _pos - 1;
        }

    }

}