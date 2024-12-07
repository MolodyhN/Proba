using lib10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаба_11
{
    internal class Program
    {
        public static int ExitOrInput(int number)
        {
            number = 2;
            Console.WriteLine("\n1.Ввести еще\n" + "0.Выход");
            Console.Write(":");
            number = Checknumber(number);
            while (number != 1 && number != 0)
            {
                Console.WriteLine("ОШИБКА\n");
                Console.WriteLine("\n1.Ввести еще\n" + "0.Выход");
                Console.Write(":");
                number = Checknumber(number);
            }
            return number;
        }
        public static int Checknumber(int number)  //Проверка ввода
        {
            number = 0;
            while (!int.TryParse(Console.ReadLine(), out number))
            {
                Console.WriteLine("Ошибка");
            }
            return number;
        }
        public static int ManualInputOrDSH(int number)
        {
            number = 3;
            Console.WriteLine("1.Ручной ввод\n" + "2.ДСЧ\n" + "0.Выход");
            Console.Write(":");
            number = Checknumber(number);
            while (number == 1 && number == 2 && number == 0)
            {
                Console.WriteLine("ОШИБКА\n");
                Console.WriteLine("1.Ручной ввод\n" + "2.ДСЧ\n" + "0.Выход");
                Console.Write(":");
                number = Checknumber(number);
            }
            return number;
        }
        static void Main(string[] args)
        {
            int exit = 1;
            int forswitch = 0;
            int number = 0;
            Console.Write("Введите размер:");
            number = Checknumber(number);
            TestCollections<Engine, ICEngine> testCollections = new TestCollections<Engine, ICEngine>(Math.Abs(number));
            while (exit != 0)
            {
                Console.WriteLine("1.Добавить элемент\n" + "2.Удалить элемент\n" + "3.Время\n" + "4.Показать\n" + "0.Выход\n");
                forswitch = Checknumber(forswitch);
                if (forswitch == 0) { break; }
                while (forswitch != 1 && forswitch != 2 && forswitch != 3 && forswitch != 4 )
                {
                    Console.Write("Ошибка.Введите снова:");
                    forswitch = Checknumber(forswitch);
                }
                switch (forswitch)
                {
                    case 1:
                        ICEngine temp = new ICEngine();
                        number = ManualInputOrDSH(number);
                        while (exit != 0)
                        {
                            if (number == 1)
                            {
                                temp.Init();
                                testCollections.AddElement(temp);
                            }
                            else
                            {
                                temp.RandomInit();
                                testCollections.AddElement(temp);
                            }
                            Console.WriteLine(temp.ToString());
                            exit = ExitOrInput(exit);
                        }
                        exit = 1;
                        Console.Clear();
                        break;
                    case 2:
                        
                        Console.Write("Введите модель:");
                        string tempstr = Console.ReadLine();
                        testCollections.RemoveElement(tempstr.Trim());
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 3:
                        testCollections.MeasureSearchTime();
                        break;
                    case 4:
                        testCollections.DisplayCollections();
                        break;
                    default:break;
                }
            }
        }
    }
}
