using System;
namespace BusinessEntities.Model
{
    public sealed class NodeBinary : Node
    {
        private Node _lhs;
        private Node _rhs;
        private Func<double, double, double> _op;

        public NodeBinary(Node lhs, Node rhs, Func<double, double, double> op)
        {
            _lhs = lhs;
            _rhs = rhs;
            _op = op;
        }

        
        public override double Eval()
        {
            var lhsVal = _lhs.Eval();
            var rhsVal = _rhs.Eval();

            var result = _op(lhsVal, rhsVal);
            return result;
        }
    }
}
