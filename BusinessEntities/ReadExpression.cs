using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities.Model;
using Mathematics.Model.Enum;

namespace BusinessEntities
{
    public class ReadExpression : IReadExpression
    {
        private readonly IOrderOfExecution _orderOfExecution;
        private readonly Dictionary<string, char> _expKeyValuePairs = new() {
            {"plus", '+'},
            {"minus", '-'},
            {"into", '*'},
            {"divide", '/' }
        };

        public ReadExpression()
        {

        }

        public ReadExpression(IOrderOfExecution orderOfExecution)
        {
            _orderOfExecution = orderOfExecution;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Node ParseExpression()
        { 
            var expr = ParseAddSubtract();

            if (_orderOfExecution.ExpOperator != ExpressionOperator.End)
                throw new Exception("Unexpected characters at end of expression");

            return expr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Node ParseAddSubtract()
        {
            var lhs = ParseMultiplyDivide();

            while (true)
            {
                Func<double, double, double> op = null;
                if (_orderOfExecution.ExpOperator == ExpressionOperator.Add)
                {
                    op = (a, b) => a + b;
                }
                else if (_orderOfExecution.ExpOperator == ExpressionOperator.Subtract)
                {
                    op = (a, b) => a - b;
                }

                if (op == null)
                    return lhs;

                _orderOfExecution.NextOperation();
                var rhs = ParseMultiplyDivide();
                lhs = new NodeBinary(lhs, rhs, op);
            }
        }

        /// <summary>
        /// Parse an sequence of add/subtract operators
        /// </summary>
        /// <returns></returns>
        public Node ParseMultiplyDivide()
        {
            var lhs = ParseUnary();

            while (true)
            {
                Func<double, double, double> op = null;
                if (_orderOfExecution.ExpOperator == ExpressionOperator.Multiply)
                {
                    op = (a, b) => a * b;
                }
                else if (_orderOfExecution.ExpOperator == ExpressionOperator.Divide)
                {
                    op = (a, b) => a / b;
                }

                if (op == null)
                {
                    return lhs;
                }
                _orderOfExecution.NextOperation();
                var rhs = ParseUnary();
                lhs = new NodeBinary(lhs, rhs, op);
            }
        }

        /// <summary>
        /// Parse a unary operator (eg: negative/positive)
        /// </summary>
        /// <returns></returns>
        public Node ParseUnary()
        {
            while (true)
            {
                if (_orderOfExecution.ExpOperator == ExpressionOperator.Add)
                {
                    _orderOfExecution.NextOperation();
                    continue;
                }

                if (_orderOfExecution.ExpOperator == ExpressionOperator.Subtract)
                {
                    _orderOfExecution.NextOperation();
                    var rhs = ParseUnary();
                    return new NodeUnary(rhs, (a) => -a);
                }
                return ParseLeaf();
            }
        }

        /// <summary>
        /// Parse a leaf node
        /// </summary>
        /// <returns></returns>
        public Node ParseLeaf()
        {
            if (_orderOfExecution.ExpOperator == ExpressionOperator.Number)
            {
                var node = new NodeNumber(_orderOfExecution.Number);
                _orderOfExecution.NextOperation();
                return node;
            }

            if (_orderOfExecution.ExpOperator == ExpressionOperator.OpenParens)
            {
                _orderOfExecution.NextOperation();
                var node = ParseAddSubtract();

                if (_orderOfExecution.ExpOperator != ExpressionOperator.CloseParens)
                    throw new Exception("Missing close parenthesis");
                _orderOfExecution.NextOperation();

                return node;
            }
            throw new Exception($"Unexpect Operation: {_orderOfExecution.ExpOperator}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ConvertInstructionToOperation(string str)
        {
            var expressionList = str.Split();
            var sb = new StringBuilder();
            foreach (var exp in expressionList)
            {
                _ = _expKeyValuePairs.TryGetValue(exp.ToLower(), out char value) ? sb.Append(value) : sb.Append(exp);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public Node Parse(string str)
        {   
            var expStr = ConvertInstructionToOperation(str);
            return Parse(new OrderOfExecution(new StringReader(expStr)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="execution"></param>
        /// <returns></returns>
        public static Node Parse(OrderOfExecution execution)
        {
            var readExpression = new ReadExpression(execution);
            return readExpression.ParseExpression();
        }

        
    }
}
