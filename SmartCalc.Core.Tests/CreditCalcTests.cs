using System;
using Xunit;

namespace SmartCalc.Core.Tests;
public class CreditCalcTests
{
    [Fact]
    public void TestCredit1()
    {
        string datePay = "29.02.2020";
        using (var credit = new CreditCalc())
        {
            credit.Calculate(1000000.0, 6.0, 2, 12, 0, datePay);
            Assert.Equal(Math.Round(credit.GetOverpayment()), Math.Round(32797.0));
            Assert.Equal(credit.GetDate(1), "29.03.2020");
        }
    }

    [Fact]
    public void TestCredit2()
    {
        string datePay = "29.02.2020";
        using (var credit = new CreditCalc())
        {
            credit.Calculate(1000000.0, 6.0, 1, 12, 0, datePay);
            Assert.Equal(Math.Round(credit.GetOverpayment()), Math.Round(32500.0));
        }
    }
}