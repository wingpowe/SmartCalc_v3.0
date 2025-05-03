using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using ScottPlot.Avalonia;
using SmartCalc.Gui.ViewModels;
using System;
using System.Text.Json;
using Avalonia.Media;

namespace CalculatorApp
{
    public partial class MainWindow : Window
    {
        private IConfiguration _configuration;
        public MainWindow()
        {
            InitializeComponent();
            LoadConfiguration();
            ApplySettings();
            DataContext = new MainWindowViewModel 
            { logRotationPeriod = _configuration["Settings:LogRotationPeriod"] };
            if (DataContext is MainWindowViewModel viewModel)
            {
                HistoryListBox.ItemsSource = viewModel.History.ToList();
            }
        }

        private void LoadConfiguration()
        {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                _configuration = builder.Build();
        }

        private void ApplySettings()
        {
            var backgroundColor = _configuration["Settings:BackgroundColor"];
            var fontSize = double.Parse(_configuration["Settings:FontSize"]);
            this.Background = new SolidColorBrush(Color.Parse(backgroundColor));
            InputTextBox.FontSize = fontSize;
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                InputTextBox.Text += button.Content?.ToString();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)=>
            InputTextBox.Text = string.Empty;
        //

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.AddToHistory(InputTextBox.Text);
                HistoryListBox.ItemsSource = viewModel.History.ToList();
                InputTextBox.Text = viewModel.Calculate();
            }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            var helpWindow = new HelpWindow();
            helpWindow.Show();
        }

        private void LoadFromHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel && HistoryListBox.SelectedItem != null)
            {
                InputTextBox.Text = HistoryListBox.SelectedItem.ToString();
            }
        }

        private void ClearHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.ClearHistory();
                HistoryListBox.ItemsSource = viewModel.History.ToList();
            }
        }
        private void BuildGraphButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                var graphWindow = new GraphWindow();
                viewModel.BuildGraph(graphWindow.Plot);
                graphWindow.Show();
            }
        }
        private void CalculateCreditButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                if (double.TryParse(CreditAmountTextBox.Text, out double amount) &&
                    int.TryParse(CreditTermTextBox.Text, out int term) &&
                    double.TryParse(CreditRateTextBox.Text, out double rate) &&
                    CreditStartDatePicker.SelectedDate.HasValue)
                {
                    int type = CreditTypeComboBox.SelectedIndex; // 0 - �����������, 1 - ������������������
                    DateTime startDate = CreditStartDatePicker.SelectedDate.Value.DateTime;

                    viewModel.CalculateCredit(amount, term, rate, type, startDate);

                    // �������� ���� � �������� ��������
                    var scheduleWindow = new CreditScheduleWindow(viewModel);
                    scheduleWindow.Show();
                }
            }
        }
    }
}