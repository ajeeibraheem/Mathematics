using System;
namespace BusinessEntities.Model
{
    public sealed class NodeUnary : Node
    {
        private Node _rhs;
        private Func<double, double> _op;

        public NodeUnary(Node rhs, Func<double, double> op)
        {
            _rhs = rhs;
            _op = op;
        }

        public override double Eval()
        {
            var rhsVal = _rhs.Eval();

            var result = _op(rhsVal);
            return result;
        }
    }
}
