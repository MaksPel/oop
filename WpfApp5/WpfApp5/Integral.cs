using System;

namespace IntegralCalculator
{
    public class Integral
    {
        // Делегат Func для передачи функции
        public delegate double Function(double x);

        // Метод для вычисления интеграла методом прямоугольников
        public static double CalculateRectangleMethod(Function func, double a, double b, int n)
        {
            double h = (b - a) / n;
            double sum = 0.0;
            for (int i = 0; i < n; i++)
            {
                double x = a + i * h;
                sum += func(x);
            }
            return sum * h;
        }
    }
}
