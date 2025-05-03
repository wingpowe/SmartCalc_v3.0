using Avalonia.Controls;
using SmartCalc.Gui.ViewModels;
using System.Linq;

namespace CalculatorApp
{
    public partial class CreditScheduleWindow : Window
    {
        public CreditScheduleWindow(object dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
            if (DataContext is MainWindowViewModel viewModel)
            {
                OverpaymentTextBlock.Text = viewModel.Overpayment.ToString("C");
                TotalPaymentTextBlock.Text = viewModel.TotalPayment.ToString("C");
                PaymentScheduleDataGrid.ItemsSource = viewModel.PaymentSchedule.ToList();

            }
        }
    }
}