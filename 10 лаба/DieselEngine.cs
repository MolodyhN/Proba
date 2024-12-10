using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib10
{
    public class DieselEngine : ICEngine
    {
        private double maxTorque;

        public double MaxTorque
        {
            get { return maxTorque; }
            set
            {
                if (value > 0)
                    maxTorque = value;
                else
                    try { throw new ArgumentOutOfRangeException(); }
                    catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
            }
        }

        public DieselEngine()
        {
            MaxTorque = 1;
        }

        public DieselEngine(string model, double power, string gender, int size, double fuelEfficiency, double maxTorque)
            : base(model, power, gender, size, fuelEfficiency)
        {
            MaxTorque = maxTorque;
        }

        // Конструктор копирования
        public DieselEngine(DieselEngine other) : base(other)
        {
            MaxTorque = other.MaxTorque;
        }
        public override void Init()
        {
            base.Init();
            Console.Write("Введите крутящий момент:");
            double number = 0;
            while (!double.TryParse(Console.ReadLine(), out number) || number <= 0)
            {
                Console.WriteLine("Ошибка,нужно число(больше 0)");
            }
            MaxTorque = number;
        }
        public override void Show()
        {
            base.Show();
            Console.WriteLine($"Max Torque: {MaxTorque} Nm");
        }

        public new void RandomInit()
        {
            base.RandomInit();
            Random rand = new Random();
            MaxTorque = rand.Next(300, 700); // случайный крутящий момент
        }
        public object Clone()
        {
            return new DieselEngine(this);
        }
    }
}

