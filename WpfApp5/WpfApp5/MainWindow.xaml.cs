using System;
using System.Windows;
using IntegralCalculator; 

namespace WpfIntegralCalculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            // Пример использования
            Integral.Function func = Math.Sin; // Интегрируемая функция - синус
            double a = double.Parse(AValueTextBox.Text); // Нижний предел интегрирования
            double b = double.Parse(BValueTextBox.Text); // Верхний предел интегрирования
            int n = int.Parse(NValueTextBox.Text); // Количество разбиений
            double result = Integral.CalculateRectangleMethod(func, a, b, n); // Вычисление интеграла
            ResultLabel.Content = $"Integral value: {result}";
        }
    }
}
