using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace lib10
{
    public class AirPlane : IInit
    {
        private string brand;
        private string model;
        public string Model
        {
            get { return model; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    model = value;
                else
                    try { throw new ArgumentOutOfRangeException(); }
                    catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
            }
        }
        public string Brand
        {
            get { return brand; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    brand = value;
                else
                    try { throw new ArgumentOutOfRangeException(); }
                    catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
            }
        }
        public void Init()
        {
            Console.Write("Бренд: ");
            Brand = Console.ReadLine();
            Console.Write("Модель: ");
            Model = Console.ReadLine();
        }

        public void RandomInit()
        {
            var random = new Random();
            Brand = "Brand " + random.Next(1, 100);
            Model = "Model " + random.Next(1, 100);
        }

        public void Show()
        {
            Console.WriteLine($"Model: {Model}\n" + $"Brand: {Brand} HP\n");
        }
    }

}
