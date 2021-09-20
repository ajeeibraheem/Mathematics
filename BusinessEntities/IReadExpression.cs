using System;
using BusinessEntities.Model;

namespace BusinessEntities
{
    public interface IReadExpression
    {
        Node Parse(string str);
        Node ParseExpression();
        Node ParseAddSubtract();
        Node ParseMultiplyDivide();
        Node ParseUnary();
        Node ParseLeaf();
    }
}
