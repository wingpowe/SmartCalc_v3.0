using System;
using Xunit;

namespace SmartCalc.Core.Tests
{
    public class ModelCalcTests
    {
        [Fact]
        public void TestAdd()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("1+1", 0.0);
            Assert.Equal(1.0 + 1.0, calc.Calculate(0.0));
            calc.ParsExpression("0.00001 + 1e-6", 0.0);
            Assert.Equal(0.00001 + 1e-6, calc.Calculate(0.0));
        }

        [Fact]
        public void TestSub()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("1-1", 0.0);
            Assert.Equal(1.0 - 1.0, calc.Calculate(0.0));
            calc.ParsExpression("0.00001 - 1e-6", 0.0);
            Assert.Equal(0.00001 - 1e-6, calc.Calculate(0.0));
        }

        [Fact]
        public void TestMult()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("53*17", 0.0);
            Assert.Equal(53.0 * 17.0, calc.Calculate(0.0));
            calc.ParsExpression("0.0005 * 3e-6", 0.0);
            Assert.Equal(0.0005 * 3e-6, calc.Calculate(0.0));
        }

        [Fact]
        public void TestDiv()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("0.00015 / 3e-6", 0.0);
            Assert.Equal(0.00015 / 3e-6, calc.Calculate(0.0));
            calc.ParsExpression("10 / 3", 0.0);
            Assert.Equal(10.0 / 3.0, calc.Calculate(0.0));
        }

        [Fact]
        public void TestMod()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("10 mod 3", 0.0);
            Assert.Equal(Math.IEEERemainder(10.0, 3.0), calc.Calculate(0.0));
        }

        [Fact]
        public void TestPow()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("2 ^ 2 ^ 3", 0.0);
            Assert.Equal(Math.Pow(2.0, Math.Pow(2.0, 3.0)), calc.Calculate(0.0));
            calc.ParsExpression("5 ^ 2e-5", 0.0);
            Assert.Equal(Math.Pow(5.0, 2e-5), calc.Calculate(0.0));
        }

        [Fact]
        public void TestUnarMinus()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("-5", 0.0);
            Assert.Equal(-5.0, calc.Calculate(0.0));
            calc.ParsExpression("-(-(-5))", 0.0);
            Assert.Equal(-(-(-5.0)), calc.Calculate(0.0));
        }

        [Fact]
        public void TestUnarPlus()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("+5", 0.0);
            Assert.Equal(+5.0, calc.Calculate(0.0));
            calc.ParsExpression("+(+(+5))", 0.0);
            Assert.Equal(+(+(+5.0)), calc.Calculate(0.0));
        }

        [Fact]
        public void TestPercent()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("50%", 0.0);
            Assert.Equal(50.0 / 100.0, calc.Calculate(0.0));
        }

        [Fact]
        public void TestSin()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("sin(2)", 0.0);
            Assert.Equal(Math.Sin(2.0), calc.Calculate(0.0));
        }

        [Fact]
        public void TestX()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("55+x", 5.0);
            Assert.Equal(55.0 + 5.0, calc.Calculate(5.0));
        }

        [Fact]
        public void TestError1()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("55+x-soe", 5.0);
            Assert.Equal("invalid characters used", calc.GetResult());
        }

        [Fact]
        public void TestError2()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("55+x(", 5.0);
            Assert.Equal("incorrect placement of parentheses", calc.GetResult());
        }

        [Fact]
        public void TestError3()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("55+x+", 5.0);
            Assert.Equal("wrong number operators", calc.GetResult());
        }

        [Fact]
        public void TestError4()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("", 5.0);
            Assert.Equal("empty expression", calc.GetResult());
        }

        [Fact]
        public void TestError5()
        {
            using var calc = new ModelCalc();
            string str = new string('x', 259);
            calc.ParsExpression(str, 5.0);
            Assert.Equal("too long expression", calc.GetResult());
        }

        [Fact]
        public void TestCos()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("cos(2)", 0.0);
            Assert.Equal(Math.Cos(2.0), calc.Calculate(0.0));
        }

        [Fact]
        public void TestTan()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("tan(2)", 0.0);
            Assert.Equal(Math.Tan(2.0), calc.Calculate(0.0));
        }

        [Fact]
        public void TestAtan()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("atan(2)", 0.0);
            Assert.Equal(Math.Atan(2.0), calc.Calculate(0.0));
        }

        [Fact]
        public void TestAcos()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("acos(1)", 0.0);
            Assert.Equal(Math.Acos(1.0), calc.Calculate(0.0));
        }

        [Fact]
        public void TestAsin()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("asin(1)", 0.0);
            Assert.Equal(Math.Asin(1.0), calc.Calculate(0.0));
        }

        [Fact]
        public void TestSqrt()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("sqrt(4)", 0.0);
            Assert.Equal(Math.Sqrt(4.0), calc.Calculate(0.0));
        }

        [Fact]
        public void TestLog()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("log(100)", 0.0);
            Assert.Equal(Math.Log(100.0), calc.Calculate(0.0));
        }

        [Fact]
        public void TestLn()
        {
            using var calc = new ModelCalc();
            calc.ParsExpression("ln(100)", 0.0);
            Assert.Equal(Math.Log10(100.0), calc.Calculate(0.0));
        }
    }
}
