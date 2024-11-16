using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class Program
    {
        static double CalculateSN(double x, int n)
        {
            double sum = Math.Pow(x, 3) / 3;
            double nextElement = Math.Pow(x,3) / 3;
            double znm = 0;
            for (int i = 2; i <= n; i++)
            {
                znm = 4 * i * i - 1;
                nextElement = nextElement * (-1) * x * x;
                sum += nextElement / znm ;
            }
            return sum;
        }

        static double CalculateSE(double x, double e)
        {
            double sum = Math.Pow(x, 3) / 3;
            double nextElement = Math.Pow(x, 3) / 3;
            int n = 2;
            double znm = 0;
            while (Math.Abs(nextElement) > e)
            {
                znm = 4 * n * n - 1;
                nextElement = nextElement * (-1) * x * x;
                sum += nextElement / znm;
                n++;
            }
            return sum;
        }

        static void Main()
        {
            double a = 0.1;    // начальное значение x
            double b = 1.0;    // конечное значение x
            int k = 9;        // количество шагов
            int n = 30;        // количество членов ряда
            double e = 0.0001; // точность
            double step = (b - a) / k; // шаг изменения x
            Console.WriteLine("X\tSN\t\tSE\t\tY");
            for (int i = 0; i <= k; i++)
            {
                double x = a + i * step;
                double SN = CalculateSN(x, n);
                double SE = CalculateSE(x, e);
                double Y = ( 1 + x * x ) / 2 * Math.Atan(x) - x / 2;
                Console.WriteLine($"{x:F1}\t{SN:F8}\t{SE:F8}\t{Y:F8}");
            }
        }
    }
}

