using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Globalization;
using ScottPlot;
using SmartCalc.Core;
using System.Collections.Generic;
using System;
using ScottPlot.Avalonia;
using System.Linq;
using Serilog;
using Serilog.Events;


namespace SmartCalc.Gui.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private ModelCalc _calc = new();
    private const string HistoryFilePath = "history.json";
    private const string LogDirectory = "logs";
    private ILogger _logger = Log.Logger;

    public string Expression { get; set; } = string.Empty;
    public string X { get; set; } = "0";
    public ObservableCollection<string> History { get; set; } = new ObservableCollection<string>();
    public double StartX { get; set; } = -100;
    public double EndX { get; set; } = 100;
    public string logRotationPeriod = "Day";
    public double MonthlyPayment { get; private set; }
    public double Overpayment { get; private set; }
    public double TotalPayment { get; private set; }
    public ObservableCollection<Payment> PaymentSchedule { get; set; } = new ObservableCollection<Payment>();


    public MainWindowViewModel()
    {
        LoadHistory();
        ConfigureLogger();
    }

    private void ConfigureLogger()
    {
        if (!Directory.Exists(LogDirectory))
        {
            Directory.CreateDirectory(LogDirectory);
        }

         
        var logFilePath = Path.Combine(LogDirectory, $"logs_{DateTime.Now:dd-MM-yy-HH-mm-ss}.txt");

        RollingInterval rollingInterval = logRotationPeriod switch
        {
            "Hour" => RollingInterval.Hour,
            "Day" => RollingInterval.Day,
            "Month" => RollingInterval.Month,
            _ => RollingInterval.Day
        };
        _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(logFilePath, rollingInterval: rollingInterval, retainedFileCountLimit: 7)
                .CreateLogger();
    }

    public void AddToHistory(string expression)
    {
        History.Add(expression);
        SaveHistory();
        _logger.Information($"Added to history: {expression}");
    }

    public void ClearHistory()
    {
        History.Clear();
        SaveHistory();
        _logger.Information("History cleared");
    }

    private void SaveHistory()
    {
        var json = JsonSerializer.Serialize(History);
        File.WriteAllText(HistoryFilePath, json);
    }

    private void LoadHistory()
    {
        if (File.Exists(HistoryFilePath))
        {
            var json = File.ReadAllText(HistoryFilePath);
            var history = JsonSerializer.Deserialize<ObservableCollection<string>>(json);
            if (history != null)
            {
                History = history;
            }
        }
    }

    public string Calculate()
    {
        if (!double.TryParse(X, NumberStyles.Any, CultureInfo.InvariantCulture, out double x))
        {
            _logger.Error($"Invalid input for X: {X}");
            return "Error: Invalid input";
        }
        _calc.ParsExpression(Expression, x);
        Expression = _calc.GetResult();
        _logger.Information($"Calculated expression: {Expression} with X: {x}");
        return Expression;
    }

    public void BuildGraph(AvaPlot plot)
    {
        double stepX = (EndX - StartX) / 5000.0;

        double[] dataX = new double[5000];
        double[] dataY = new double[5000];
        _calc.ParsExpression(Expression, StartX);
        int index = 0;
        for (double x = StartX ; x <= EndX && index < 5000 && x >= -1000000.0 && x <= 1000000.0; x += stepX, index++)
            dataY[index] = _calc.Calculate(dataX[index] = x);
        plot.Plot.Add.Scatter(dataX, dataY);
        plot.Plot.Axes.SetLimits(dataX.Min(), dataX.Max(), dataY.Min(), dataY.Max());
        plot.Interaction.Disable();
        _logger.Information($"Built graph for expression: {Expression} from {StartX} to {EndX}");

    }

    public void CalculateCredit(double amount, int term, double rate, int type, DateTime startDate)
    {
        // Очистка предыдущего графика платежей
        PaymentSchedule.Clear();

        using (var creditCalc = new CreditCalc())
        {
            // Преобразование даты в строку
            string startDateString = startDate.ToString("dd.MM.yyyy");

            // Выполнение расчета
            creditCalc.Calculate(amount, rate, type+1, term, 0, startDateString);

            // Получение количества платежей
            int countPayments = creditCalc.GetCountPayment();

            // Заполнение графика платежей
            for (int i = 0; i < countPayments; i++)
            {
                PaymentSchedule.Add(new Payment
                {
                    DatePayment = creditCalc.GetDate(i),
                    CreditPayment = creditCalc.GetPayment(i),
                    Rate = creditCalc.GetRate(i),
                    Body = creditCalc.GetBody(i),
                    Remainder = creditCalc.GetRemainder(i)
                });
            }

            // Получение общей суммы переплаты и общей суммы платежей
            Overpayment = creditCalc.GetOverpayment();
            TotalPayment = creditCalc.GetTotalPayment();
        }
    }

}
public class Payment
{
    public string DatePayment { get; set; }
    public double CreditPayment { get; set; }
    public double Rate { get; set; }
    public double Body { get; set; }
    public double Remainder { get; set; }
}