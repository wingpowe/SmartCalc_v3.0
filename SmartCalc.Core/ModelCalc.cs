using System;
using System.Runtime.InteropServices;

namespace SmartCalc.Core;
public class ModelCalc : IDisposable
{
    private IntPtr calculatorHandle;

    [DllImport("libModel_calc.so", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr create_calculator();

    [DllImport("libModel_calc.so", CallingConvention = CallingConvention.Cdecl)]
    private static extern void destroy_calculator(IntPtr calculator);

    [DllImport("libModel_calc.so", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool parsing(IntPtr calculator, string expression, double x);
    
    [DllImport("libModel_calc.so", CallingConvention = CallingConvention.Cdecl)]
    private static extern double calculate(IntPtr calculator, double x);

    [DllImport("libModel_calc.so", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr result(IntPtr calculator);

    public ModelCalc() =>
        calculatorHandle = create_calculator();
    public bool ParsExpression(string expression, double x) 
        => parsing(calculatorHandle, expression, x);

    public  double Calculate(double x) 
        => calculate(calculatorHandle, x);   

 public string GetResult()
    {
        IntPtr Ptr = result(calculatorHandle);
        if (Ptr == IntPtr.Zero)
        {
            return string.Empty;
        }
        string res = Marshal.PtrToStringAnsi(Ptr);
        return res ?? string.Empty;
    }

    public void Dispose()
    {
        if (calculatorHandle != IntPtr.Zero)
        {
            destroy_calculator(calculatorHandle);
            calculatorHandle = IntPtr.Zero;
        }
    }
}