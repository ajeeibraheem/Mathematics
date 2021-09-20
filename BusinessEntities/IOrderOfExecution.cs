using System;
using Mathematics.Model.Enum;

namespace BusinessEntities
{
    public interface IOrderOfExecution
    {
        public ExpressionOperator ExpOperator { get; }
        public double Number { get; }
        void NextChar();
        void NextOperation();
    }
}
