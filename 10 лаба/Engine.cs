using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace lib10
{
    public interface IInit
    {
        void Init();
        void RandomInit();
        void Show();
    }

    public class Engine : IInit, ICloneable, IComparable<Engine>
    {
        private static Random rand = new Random();
        private string model;
        private double power;
        private string gender;
        private ForCopy size;
        public ForCopy Size
        {
            get { return size; }
            set { size = value; }
        }
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
        public double Power
        {
            get { return power; }
            set
            {
                if (value > 0)
                    power = value;
                else
                    try { throw new ArgumentOutOfRangeException(); }
                    catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
            }
        }
        public string Gender
        {
            get { return gender; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    gender = value;
                else
                    try { throw new ArgumentOutOfRangeException(); }
                    catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
            }
        }
        public int CompareTo(Engine other)
        {
            if (other == null) return 1;
            return Power.CompareTo(other.Power); 
        }
        public object Clone()
        {
            if (this.Size != null)
            {
                return new Engine(Model, Power, Gender, Size.Value)
                {
                    Size = new ForCopy(Size.Value)
                };
            }
            else
            {
                return new Engine(Model, Power, Gender, Size.Value)
                {
                    Size = null
                };
            }
        }

        public Engine ShallowCopy()
        {
            return (Engine)this.MemberwiseClone(); // Поверхностное копирование
        }
            
        public Engine()
        {
            Model = " ";
            Power = 1;
            Gender = "null";
            Size = null;
        }

        public Engine(string model, double power, string gender, int size)
        {
            Model = model;
            Power = power;
            Gender = gender;
            Size.Value = size;
        }

        public Engine(Engine other)
        {
            Model = other.Model;
            Power = other.Power;
            Gender = other.Gender;
            Size = other.Size;
        }
        public virtual void Init()
        {
            Console.Write("Введите модель:");
            Model = Console.ReadLine();
            Console.Write("Введите мощность:");
            int number = 0;
            while (!int.TryParse(Console.ReadLine(), out number) || number < 0)
            {
                Console.WriteLine("Ошибка,нужно число(больше 0)");
            }
            Power = number;
            Console.Write("Введите гендер(M или W):");
            string gend = Console.ReadLine();
            while (gend != "W" && gend != "M")
            {
                Console.WriteLine("Неправельный гендер");
                gend = Console.ReadLine();
            }
            Gender = gend;
        }
        public virtual void Show()
        {
            Console.WriteLine($"Model: {Model}\n" + $"Power: {Power} HP\n" + $"Gender: {Gender} ");
        }


        public void RandomInit()
        {
            Model = "Model " + rand.Next(1, 100).ToString();
            Power = rand.Next(50, 1000);
            Gender = rand.Next(2) == 0 ? "M" : "W";
        }

        // Метод для сравнения объектов
        public bool Equals(object obj)
        {
            if (obj is Engine otherEngine)
            {
                return this.Model == otherEngine.Model && this.Power == otherEngine.Power && this.Gender == otherEngine.Gender;
            }
            return false;
        }
    }
}
