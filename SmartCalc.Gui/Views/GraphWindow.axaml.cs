using Avalonia.Controls;
using ScottPlot.Avalonia;

namespace CalculatorApp
{
    public partial class GraphWindow : Window
    {
        public GraphWindow()
        {
            InitializeComponent();
        }

        public AvaPlot Plot => this.Find<AvaPlot>("AvaPlot1");
    }
}
