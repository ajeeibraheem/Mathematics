using System;
using System.Threading.Tasks;
using BusinessEntities;
using Microsoft.Extensions.Logging;

namespace Mathematics
{
    public class BodmasOperation : IBodmasOperationInterface
    {
        private readonly IReadExpression _readExpression;
        private readonly IOrderOfExecution _orderOfExecution;

        public BodmasOperation(IOrderOfExecution orderOfExecution, IReadExpression readExpression)
        {
            _orderOfExecution = orderOfExecution ?? new OrderOfExecution();
            _readExpression = readExpression ?? new ReadExpression(_orderOfExecution);
        }

        /// <summary>
        /// Evaluates the expression to give the desired output
        /// </summary>
        /// <returns></returns>
        public double ExecuteExpression(string expression)
        {
         return _readExpression.Parse(expression).Eval();
        }
    }
}