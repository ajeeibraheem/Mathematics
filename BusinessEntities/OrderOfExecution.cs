using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Mathematics.Model.Enum;

namespace BusinessEntities
{
    public class OrderOfExecution : IOrderOfExecution
    {
        private readonly TextReader _reader;
        private char _currentChar;
        private ExpressionOperator _expressionOperator;
        private double _number;

        ExpressionOperator IOrderOfExecution.ExpOperator { get => _expressionOperator; }
        double IOrderOfExecution.Number { get => _number; }

        public OrderOfExecution(TextReader reader)
        {
            _reader = reader;
            NextChar();
            NextOperation();
        }

        public OrderOfExecution()
        {
        }

        public void NextChar()
        {
            int ch = _reader.Read();
            _currentChar = ch < 0 ? '\0' : (char)ch;
        }

        public void NextOperation()
        {
            while (char.IsWhiteSpace(_currentChar))
            {
                NextChar();
            }

            switch (_currentChar)
            {
                case '\0':
                    _expressionOperator = ExpressionOperator.End;
                    return;

                case '+':
                    NextChar();
                    _expressionOperator = ExpressionOperator.Add;
                    return;

                case '-':
                    NextChar();
                    _expressionOperator = ExpressionOperator.Subtract;
                    return;

                case '*':
                    NextChar();
                    _expressionOperator = ExpressionOperator.Multiply;
                    return;

                case '/':
                    NextChar();
                    _expressionOperator = ExpressionOperator.Divide;
                    return;

                case '(':
                    NextChar();
                    _expressionOperator = ExpressionOperator.OpenParens;
                    return;

                case ')':
                    NextChar();
                    _expressionOperator = ExpressionOperator.CloseParens;
                    return;
            }

            if (char.IsDigit(_currentChar) || _currentChar == '.')
            {
                var sb = new StringBuilder();
                bool haveDecimalPoint = false;
                while (char.IsDigit(_currentChar) || (!haveDecimalPoint && _currentChar == '.'))
                {
                    sb.Append(_currentChar);
                    haveDecimalPoint = _currentChar == '.';
                    NextChar();
                }
                _number = double.Parse(sb.ToString(), CultureInfo.InvariantCulture);
                _expressionOperator = ExpressionOperator.Number;
                return;
            }
        }
    }
}
