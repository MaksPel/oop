using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DynamicWPFApp
{
    public partial class MainWindow : Window
    {
        private Path currentPath;
        private Brush currentBrush = Brushes.Black;

        public MainWindow()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Создание основного окна и его элементов без использования конструктора

            Title = "Графический редактор";
            Width = 600;
            Height = 400;

            Grid grid = new Grid();
            Content = grid;

            // Панель для рисования
            Canvas drawingCanvas = new Canvas();
            drawingCanvas.Background = Brushes.White;
            drawingCanvas.MouseDown += DrawingCanvas_MouseDown;
            drawingCanvas.MouseMove += DrawingCanvas_MouseMove;
            drawingCanvas.MouseUp += DrawingCanvas_MouseUp;
            grid.Children.Add(drawingCanvas);

            // Кнопки для смены цвета линии
            Button blackButton = CreateColorButton(Brushes.Black);
            Button redButton = CreateColorButton(Brushes.Red);
            Button blueButton = CreateColorButton(Brushes.Blue);

            blackButton.Margin = new Thickness(10, 300, 470, 10);
            redButton.Margin = new Thickness(20, 300, 330, 10);
            blueButton.Margin = new Thickness(30, 300, 190, 10);

            grid.Children.Add(blackButton);
            grid.Children.Add(redButton);
            grid.Children.Add(blueButton);
        }

        private Button CreateColorButton(Brush color)
        {
            Button button = new Button();
            button.Width = 60;
            button.Height = 30;
            button.Background = color;
            button.Click += (sender, e) => currentBrush = color;
            return button;
        }

        private void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas drawingCanvas = (Canvas)sender;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point mousePosition = e.GetPosition(drawingCanvas); // Получаем относительные координаты
                currentPath = new Path();
                currentPath.Stroke = currentBrush;
                currentPath.StrokeThickness = 2;
                currentPath.Data = new PathGeometry();
                ((PathGeometry)currentPath.Data).Figures.Add(new PathFigure(mousePosition, new List<PathSegment>() { new LineSegment(mousePosition, true) }, false)); // Используем полученные координаты
                drawingCanvas.Children.Add(currentPath);
            }
        }

        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas drawingCanvas = (Canvas)sender;
            if (currentPath != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point mousePosition = e.GetPosition(drawingCanvas); // Получаем относительные координаты
                ((PathGeometry)currentPath.Data).Figures[0].Segments.Add(new LineSegment(mousePosition, true)); // Используем полученные координаты
            }
        }


        private void DrawingCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            currentPath = null;
        }

        private void BlackButton_Click(object sender, RoutedEventArgs e)
        {
            currentBrush = Brushes.Black;
        }

        private void RedButton_Click(object sender, RoutedEventArgs e)
        {
            currentBrush = Brushes.Red;
        }

        private void BlueButton_Click(object sender, RoutedEventArgs e)
        {
            currentBrush = Brushes.Blue;
        }
    }
}
