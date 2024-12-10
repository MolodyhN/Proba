using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib10
{
    public class JetEngine : Engine
    {
        private double thrust;

        public double Thrust
        {
            get { return thrust; }
            set
            {
                if (value > 0)
                    thrust = value;
                else
                    try { throw new ArgumentOutOfRangeException(); }
                    catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
            }
        }

        public JetEngine()
        {
            Thrust = 1;
        }

        public JetEngine(string model, double power, string gender, int size, double thrust)
            : base(model, power, gender, size)
        {
            Thrust = thrust;
        }
        public JetEngine(JetEngine other) : base(other)
        {
            Thrust = other.Thrust;
        }
        public override void Init()
        {
            base.Init();
            Console.Write("Введите осевую нагрузку:");
            double number = 0;
            while (!double.TryParse(Console.ReadLine(), out number) || number <= 0)
            {
                Console.WriteLine("Ошибка,нужно число(больше 0)");
            }
            Thrust = number;
        }
        public override void Show()
        {
            base.Show();
            Console.WriteLine($"Thrust: {Thrust} kN");
        }

        public new void RandomInit()
        {
            base.RandomInit();
            Random rand = new Random();
            Thrust = rand.Next(30, 150); 
        }
    }
}
