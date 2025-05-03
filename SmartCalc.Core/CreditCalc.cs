using System;
using System.Runtime.InteropServices;


namespace SmartCalc.Core;
public class CreditCalc : IDisposable
{
    private IntPtr _calculator;

    [DllImport("libModel_calc.so")]
    private static extern IntPtr create_credit_calculator();

    [DllImport("libModel_calc.so")]
    private static extern void destroy_credit_calculator(IntPtr calculator);

    [DllImport("libModel_calc.so")]
    private static extern void credit_calc(IntPtr calculator, double amount, double interest, int type, int months, int years, string sdate);

    [DllImport("libModel_calc.so")]
    private static extern IntPtr credit_date(IntPtr calculator, int index);

    [DllImport("libModel_calc.so")]
    private static extern double credit_payment(IntPtr calculator, int index);

    [DllImport("libModel_calc.so")]
    private static extern double credit_rate(IntPtr calculator, int index);

    [DllImport("libModel_calc.so")]
    private static extern double credit_body(IntPtr calculator, int index);

    [DllImport("libModel_calc.so")]
    private static extern double credit_remainder(IntPtr calculator, int index);

    [DllImport("libModel_calc.so")]
    private static extern double credit_overpayment(IntPtr calculator);

    [DllImport("libModel_calc.so")]
    private static extern double credit_total_payment(IntPtr calculator);

    [DllImport("libModel_calc.so")]
    private static extern int credit_count_payment(IntPtr calculator);

    public CreditCalc()
    {
        _calculator = create_credit_calculator();
    }

    public void Calculate(double amount, double interest, int type, int months, int years, string sdate)
    {
        credit_calc(_calculator, amount, interest, type, months, years, sdate);
    }

    public string GetDate(int index)
    {
        IntPtr ptr = credit_date(_calculator, index);
        return Marshal.PtrToStringAnsi(ptr);
    }

    public double GetPayment(int index) => credit_payment(_calculator, index);
    public double GetRate(int index) => credit_rate(_calculator, index);
    public double GetBody(int index) => credit_body(_calculator, index);
    public double GetRemainder(int index) => credit_remainder(_calculator, index);
    public double GetOverpayment() => credit_overpayment(_calculator);
    public double GetTotalPayment() => credit_total_payment(_calculator);
    public int GetCountPayment() => credit_count_payment(_calculator);

    public void Dispose()
    {
        if (_calculator != IntPtr.Zero)
        {
            destroy_credit_calculator(_calculator);
            _calculator = IntPtr.Zero;
        }
    }
}