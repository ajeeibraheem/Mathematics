using System;
using BusinessEntities;
using Mathematics;
using Moq;
using Xunit;

namespace MathematicsTest
{
    public class BodmasOperationTest
    {
        private readonly Mock<IReadExpression> _readExpressionMock;
        private readonly Mock<IOrderOfExecution> _orderExecutionMock;
        private readonly BodmasOperation _bodmasOperation;

        public BodmasOperationTest()
        {
            _orderExecutionMock = new Mock<IOrderOfExecution>();
            _readExpressionMock = new Mock<IReadExpression>();
            _bodmasOperation = new BodmasOperation(_orderExecutionMock.Object, _readExpressionMock.Object);
        }

        [Fact]
        public void ShouldBe_0_When_1_Plus_3_Minus_4()
        {
            var expression = "1 Plus 3 Minus 4";
            _readExpressionMock.Setup(x => x.Parse(expression).Eval()).Returns(0);
            var result = _bodmasOperation.ExecuteExpression(expression);
            Assert.Equal(0, result);
        }

        [Fact]
        public void ShouldBe_20_When_10_Into_10_DividedBy_4()
        {
            var expression = "10 Into 10 Divide 4";
            _readExpressionMock.Setup(x => x.Parse(expression).Eval()).Returns(20);
            var result = _bodmasOperation.ExecuteExpression(expression);
            Assert.Equal(20, result);
        }

        [Fact]
        public void ShouldBe_35_When_7_Into_OpenBracke_10_DividedBy_4_CloseBracket()
        {
            var expression = "7 Into (10 Divide 2)";
            _readExpressionMock.Setup(x => x.Parse(expression).Eval()).Returns(35);
            var result = _bodmasOperation.ExecuteExpression(expression);
            Assert.Equal(35, result);
        }

        [Fact]
        public void ShouldBe_0_When_10_Plus_Minus_10()
        {
            var expression = "10 Plus Minus 10";
            _readExpressionMock.Setup(x => x.Parse(expression).Eval()).Returns(0);
            var result = _bodmasOperation.ExecuteExpression(expression);
            Assert.Equal(0, result);
        }
    }
}
