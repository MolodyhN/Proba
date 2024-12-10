using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using lib10;


namespace Лаба10_Пересборка1_
{
    class Program
    {
        public static int Checknumber(int number)  //Проверка ввода
        {
            number = 0;
            while (!int.TryParse(Console.ReadLine(), out number))
            {
                Console.WriteLine("Ошибка");
            }
            return number;
        }
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
        public static void CountByGender(List<Engine> engines)// Запрос 1: Посчитать по гендеру
        {
            int maleCount = 0, femaleCount = 0;

            foreach (var engine in engines)
            {
                if (engine.Gender == "M") maleCount++;
                if (engine.Gender == "W") femaleCount++;
            }

            Console.WriteLine($"Мужчин: {maleCount}, Девушек: {femaleCount}");
        }
        public static void FindAllDieselEngines(List<Engine> engines)
        {
            int engineCount = 0;
            foreach (var engine in engines)
            {
                if (engine is DieselEngine dieselEngine)
                {
                    engineCount += 1;
                    dieselEngine.Show(); // Вызов виртуального метода Show
                    Console.WriteLine("____________________________________________");
                    
                }
            }
            Console.WriteLine($"Найдено:{engineCount}");
        }// Запрос 2: Найти все DieselEngine
        public static void CalculateTotalPower(List<Engine> engines)
        {
            double totalPower = 0;

            foreach (var engine in engines)
            {
                totalPower += engine.Power;
            }

            Console.WriteLine($"Мощность: {totalPower} kW");
        }// Запрос 3: Подсчитать суммарную мощность всех двигателей
        public static void CreateArr5(ref Engine[] engines)
        {
            for (int i = 0; i < engines.Length; i++)
            {
                engines[i] = new Engine();
                engines[i].RandomInit();
            }
        }
        static void Main()
        {
            int exit = 1;
            int forswitch = 0;
            int number = 0;
            int numberofexit = 1;
            int temp = 0;
            List<Engine> myList = new List<Engine>();
            while (exit != 0)
            {

                Console.WriteLine("1.Двигатель\n" + "2.Двигатель внутреннего сгорания\n" + "3.Дизельный двигатель\n" + "4.Турбореактивный двигатель\n" + "5.Просмотр\n" + "6.Запросы\n" + "7.Сортировка(по мощности)\n" + "8.Сортировка(по другому критерию)(по модели)\n" + "9.Бинарный поиск\n" + "10.IInit\n" + "11.Клонирование\n" + "0.Выход\n");
                forswitch = Checknumber(forswitch);
                if (forswitch == 0) { break; }
                while (forswitch != 1 && forswitch != 2 && forswitch != 3 && forswitch != 4 && forswitch != 5 && forswitch != 6 && forswitch != 7  && forswitch != 8 && forswitch != 9 && forswitch != 10 && forswitch != 11)
                {
                    Console.Write("Ошибка.Введите снова:");
                    forswitch = Checknumber(forswitch);
                }
                switch (forswitch)
                {
                    case 1:
                        number = ManualInputOrDSH(number);
                        if (number == 0)
                        {
                            Console.Clear();
                            break;
                        }
                        numberofexit = 1;
                        while (numberofexit != 0)
                        {
                            Engine eng1 = new Engine();
                            if (number == 1)
                            {
                                eng1.Init();
                            }
                            else
                            {
                                eng1.RandomInit();
                            }
                            eng1.Show();
                            myList.Add(eng1);
                            numberofexit = ExitOrInput(number);
                        }
                        Console.Clear();
                        break;
                    case 2:
                        number = ManualInputOrDSH(number);
                        if (number == 0)
                        {
                            Console.Clear();
                            break;
                        }
                        numberofexit = 1;
                        while (numberofexit != 0)
                        {
                            Engine eng2 = new ICEngine();
                            if (number == 1)
                            {
                                eng2.Init();
                            }
                            else
                            {
                                eng2.RandomInit();
                            }
                            eng2.Show();
                            myList.Add(eng2);
                            numberofexit = ExitOrInput(number);
                        }
                        Console.Clear();
                        break;
                    case 3:
                        number = ManualInputOrDSH(number);
                        if (number == 0)
                        {
                            Console.Clear();
                            break;
                        }
                        numberofexit = 1;
                        while (numberofexit != 0)
                        {
                            Engine eng3 = new DieselEngine();
                            if (number == 1)
                            {
                                eng3.Init();
                            }
                            else
                            {
                                eng3.RandomInit();
                            }
                            eng3.Show();
                            myList.Add(eng3);
                            numberofexit = ExitOrInput(number);
                        }
                        Console.Clear();
                        break;
                    case 4:
                        number = ManualInputOrDSH(number);
                        if (number == 0)
                        {
                            Console.Clear();
                            break;
                        }
                        numberofexit = 1;
                        while (numberofexit != 0)
                        {
                            Engine eng4 = new JetEngine();
                            if (number == 1)
                            {
                                eng4.Init();
                            }
                            else
                            {
                                eng4.RandomInit();
                            }
                            eng4.Show();
                            myList.Add(eng4);
                            numberofexit = ExitOrInput(number);
                        }
                        Console.Clear();
                        break;
                    case 5:
                        foreach (Engine Eng in myList)
                        {
                            Eng.Show();
                            Console.WriteLine("____________________________________________________");

                        }
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 6:
                        number = 1;
                        while (number != 0)
                        {
                            Console.WriteLine("1.Посчитать по гендеру\n" + "2.Найти все DieselEngine\n" + "3.Подсчитать суммарную мощность всех двигателей\n" + "0.Выход\n" + ":");
                            number = Checknumber(number);
                            if (number == 0)
                            {
                                number = 0;
                                break;
                            }
                            while (number != 1 && number != 2 && number != 3)
                            {
                                Console.WriteLine("Такого нет.Введите снова");
                                number = Checknumber(number);
                            }
                            if (number == 1) { CountByGender(myList); }
                            else if (number == 2) { FindAllDieselEngines(myList); }
                            else { CalculateTotalPower(myList); }
                        }
                        Console.Clear();
                        break;
                    case 7:
                        Engine[] engines = new Engine[5];
                        CreateArr5(ref engines);
                        Array.Sort(engines);

                        Console.WriteLine("---------------ОТСОРТИРОВАНЫЙ---------------");
                        foreach (var engine in engines)
                        {
                            engine.Show();
                            Console.WriteLine("------------------------------------------");
                        }
                        Console.Write("Нажмите любую клавишу для выхода");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 8:
                        engines = new Engine[5];
                        CreateArr5(ref engines);
                        Array.Sort(engines, new ManufacturerComparer());

                        Console.WriteLine("---------------ОТСОРТИРОВАНЫЙ---------------");
                        foreach (var engine in engines)
                        {
                            engine.Show();
                            Console.WriteLine("------------------------------------------");
                        }
                        Console.Write("Нажмите любую клавишу для выхода");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 9:
                        engines = new Engine[5];
                        CreateArr5(ref engines);
                        Array.Sort(engines);
                        foreach (var engine in engines)
                        {
                            engine.Show();
                            Console.WriteLine("------------------------------------------");
                        }
                        Console.Write("\nВведите мощность для поиска:");
                        int pwr = 0;
                        pwr = Checknumber(pwr);
                        Engine target = new Engine { Power = pwr }; // Задаем мощность для поиска
                        int index = Array.BinarySearch(engines, target);
                        if (index >= 0)
                        {
                            Console.WriteLine($"Найден по номеру: {index + 1}\n" + $"({engines[index].Model},{engines[index].Power} kW,{engines[index].Gender})");
                        }
                        else
                        {
                            Console.WriteLine("Двигатель не найден");
                        }
                        Console.Write("Нажмите любую клавишу для выхода");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 10:
                        IInit[] initArray = new IInit[5];

                        initArray[0] = new Engine();
                        initArray[1] = new DieselEngine();
                        initArray[2] = new JetEngine();
                        initArray[3] = new ICEngine();
                        initArray[4] = new AirPlane();

                        foreach (var obj in initArray)
                        {
                            obj.RandomInit();
                            obj.Show();
                            Console.WriteLine("---------------------");
                        }
                        Console.Write("Нажмите любую клавишу для выхода");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 11:
                        Engine original = new Engine ("Original", 500, "M", 50);
                        original.Size = new ForCopy(100);
                        Engine deepCopy = (Engine)original.Clone();
                        Engine shallowCopy = original.ShallowCopy();

                        Console.WriteLine($"Original: {original.Model}, Power: {original.Power}, Gender: {original.Gender}");
                        Console.WriteLine($"Deep Copy: {deepCopy.Model}, Power: {deepCopy.Power}, Gender: {original.Gender}");
                        Console.WriteLine($"Shallow Copy: {shallowCopy.Model}, Power: {shallowCopy.Power}, Gender: {original.Gender}");

                        original.Model = "Modified";
                        original.Size.Value = 2222;

                        Console.WriteLine("\nПосле изменения оригинала:");
                        Console.WriteLine($"Original: {original.Model}, Power: {original.Power}, Gender: {original.Gender}, Size: {original.Size.Value}");
                        Console.WriteLine($"Deep Copy: {deepCopy.Model}, Power: {deepCopy.Power}, Gender: {deepCopy.Gender}, Size: {deepCopy.Size.Value}");
                        Console.WriteLine($"Shallow Copy: {shallowCopy.Model}, Power: {shallowCopy.Power}, Gender: {shallowCopy.Gender}, Size: {shallowCopy.Size.Value}");

                        Console.Write("Нажмите любую клавишу для выхода");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Такого нет =)");
                        break;
                }

            }
        }
    }

}