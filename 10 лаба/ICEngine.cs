using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib10
{
    public class ICEngine : Engine
    {
        private double fuelEfficiency;

        public double FuelEfficiency
        {
            get { return fuelEfficiency; }
            set
            {
                if (value > 0)
                    fuelEfficiency = value;
                else
                    try { throw new ArgumentOutOfRangeException(); }
                    catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
            }
        }

        public ICEngine()
        {
            FuelEfficiency = 1;
        }

        public ICEngine(string model, double power, string gender, int size, double fuelEfficiency)
            : base(model, power, gender, size)
        {
            FuelEfficiency = fuelEfficiency;
        }

        public ICEngine(ICEngine other) : base(other)
        {
            FuelEfficiency = other.FuelEfficiency;
        }
        public override void Init()
        {
            base.Init();
            Console.Write("Введите эффективность топлива:");
            double number = 0;
            while (!double.TryParse(Console.ReadLine(), out number) || number <= 0)
            {
                Console.WriteLine("Ошибка,нужно число(больше 0)");
            }
            FuelEfficiency = number;
        }
        public override void Show()
        {
            base.Show();
            Console.WriteLine($"Fuel Efficiency: {FuelEfficiency} L/100km");
        }

        public new void RandomInit()
        {
            base.RandomInit();
            Random rand = new Random();
            FuelEfficiency = rand.Next(5, 15);
        }
        public object Clone()
        {
            return new ICEngine(this);
        }
    }
}