using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаба9
{
    internal class Program
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
        static void Input(ref int r, ref int k)
        {
            bool flag1 = false;
            bool flag2 = false;
            string[] input = Console.ReadLine().Split(' ');
            if(input.Length == 2)
            {
                flag1 = int.TryParse(input[0], out r);
                flag2 = int.TryParse(input[1], out k);
            }
            while (!flag1 || !flag2)
            {
                Console.WriteLine("Ошибка.Введите снова:");
                input = Console.ReadLine().Split(' ');
                if (input.Length == 2)
                {
                    flag1 = int.TryParse(input[0], out r);
                    flag2 = int.TryParse(input[1], out k);
                }
            }
          
        }
        static void Subtraction(ref Money a,ref Money b)
        {
            int totalKopecksA = a.Rubles * 100 + a.Kopeks;
            int totalKopecksB = b.Rubles * 100 + b.Kopeks;
            int resultKopecks = totalKopecksA - totalKopecksB;
            a = new Money(resultKopecks / 100, resultKopecks % 100);
        }
        static int IncArr(ref MoneyArray userMoneyArray, int size)
        {
            MoneyArray temp = new MoneyArray(userMoneyArray.Size() + size, userMoneyArray);
            userMoneyArray = new MoneyArray(temp.Size(), temp);
            return size + 1;
        }
        static void Main(string[] args)
        {
            int number = 0;
            bool flag = false;
            MoneyArray forcase4 = new MoneyArray();
            Money a = new Money();
            Money b = new Money();
            Money c = new Money();
            Money.ObjectCount = -3;
            while (!flag)
            {
                Interface.Inp();
                int forswitch = 0;
                forswitch = Checknumber(forswitch);
                while (forswitch != 1 && forswitch != 2 && forswitch != 3 && forswitch != 4 && forswitch != 5 && forswitch != 0)
                {
                    Console.WriteLine("Ошибка.Введите снова");
                    forswitch = Checknumber(forswitch);
                }
                switch (forswitch)
                {
                    case 1:
                        bool flg = false;
                        int r = 0;
                        int k = 0;
                        Money.ObjectCount = 2;
                        while (!flg)
                        {
                            a.Reset();
                            Console.WriteLine($"Введите рубли и копейки для элемента 1 (формат: рубли копейки):");
                            Input(ref r, ref k);
                             a.Rubles = r;a.Kopeks = k;
                            b.Reset();
                            Console.WriteLine($"Введите рубли и копейки для элемента 2 (формат: рубли копейки):");
                            Input(ref r, ref k);
                             b.Rubles = r;b.Kopeks = k;
                            if ((a.Rubles * 100 + a.Kopeks) == (b.Rubles * 100 + b.Kopeks))
                            {
                                Console.WriteLine($"{a.Rubles} руб {a.Kopeks} коп  = {b.Rubles} руб {b.Kopeks} коп");
                            }
                            else
                            {
                                flg = a > b;
                                if (flg)
                                {
                                    Console.WriteLine($"{a.Rubles} руб {a.Kopeks} коп  > {b.Rubles} руб {b.Kopeks} коп");
                                }
                                else
                                {
                                    Console.WriteLine($"{b.Rubles} руб {b.Kopeks} коп  > {a.Rubles} руб {a.Kopeks} коп");
                                }
                            }

                            Console.WriteLine("1.Возобновить\n"+"0.Выход");
                            forswitch = Checknumber(forswitch);
                            while (forswitch != 1 && forswitch != 0)
                            {
                                Console.WriteLine("Ошибка.Введите снова.");
                                forswitch = Checknumber(forswitch);
                            }
                            if (forswitch == 0)
                            {
                                flg = true;
                                Console.Clear();
                            }
                        }
                        break;
                    case 2:
                        r = 0;
                        k = 0;
                        Money.ObjectCount = 1;
                        Console.WriteLine($"Введите рубли и копейки для элемента (формат: рубли копейки):");
                        Input(ref r, ref k);
                        c.Rubles = r;
                        c.Kopeks = k;
                        Console.WriteLine("\n\t<Унарные операции>");  // Унарные операции
                        Console.Write("Увеличиваем на 1 копейку: ");
                        c++;   // Увеличиваем на 1 копейку            
                        c.Write();
                        Console.Write("Уменьшаем на 1 копейку: ");
                        c--;   // Уменьшаем на 1 копейку
                        c.Write();
                        Console.WriteLine("\n\t<Приведение типов>");  // Приведение типов
                        int rubles = c;   // Неявное приведение
                        Console.Write("Неявное приведение: ");
                        Console.WriteLine(rubles);

                        double kopekAsDouble = (double)c.Kopeks;   // Явное приведение
                        kopekAsDouble = kopekAsDouble / 100;
                        Console.Write("Явное приведение: ");
                        Console.WriteLine(kopekAsDouble);

                        Console.WriteLine("\n\t<Бинарная операция>");   // Бинарная операция
                        c = c - 75;   // Вычитаем 75 копеек
                        Console.Write("Вычитаем 75 копеек: ");
                        c.Write();
                        Console.WriteLine("\n\t<Бинарная операция>");   // Бинарная операция
                        k = 1000 - c;   // Вычитаем 75 копеек
                        Console.Write("Вычитаем предыдущее значение из 1000 копеек: ");
                        Console.WriteLine(k);
                        break;
                    case 3:
                        // Создание массива с случайными значениями
                        int n = 0;
                        Console.Write("Введите размер массива с случайными значениями:");
                        n = Checknumber(n);
                        MoneyArray moneyArrayRandom = new MoneyArray(n);
                        moneyArrayRandom.Display();

                        // Создание массива с пользовательским вводом
                        Console.Write("\nВведите количество элементов в пользовательском массиве:");
                        int size = 0;
                        size = Checknumber(size); 
                        Money[] userMoneyArray = new Money[size];
                        int rubless = 0;
                        int kopekss = 0;
                        for (int i = 0; i < size ; i++)
                        {
                            Console.WriteLine($"Введите рубли и копейки для элемента {i+1} (формат: рубли копейки):");
                            Input(ref rubless, ref kopekss);
                            userMoneyArray[i] = new Money(rubless, kopekss);
                        }
                        MoneyArray moneyArrayUser = new MoneyArray(size, userMoneyArray);
                        Console.WriteLine("\nМассив с пользовательским вводом:");
                        moneyArrayUser.Display();                       
                        Console.WriteLine($"\nКоличество созданных объектов Money: {Money.GetObjectCount()}");   // Вывод количества созданных объектов
                        flg = false;
                        while (!flg) {
                            Console.WriteLine("1.Добавить еще\n" + "2.Показать массив\n" + "0.Выход\n");
                            forswitch = Checknumber(forswitch);
                            while (forswitch != 1 && forswitch != 2 && forswitch != 0)
                            {
                                Console.WriteLine("Ошибка.Введите снова.");
                                forswitch = Checknumber(forswitch);
                            }
                            switch (forswitch)
                            {
                                case 0:
                                    flg = true;
                                    Console.Clear();
                                    break;
                                case 1:
                                    Console.Write("Введите количество элементов:");
                                    size = Checknumber(size);
                                    int oldsize = moneyArrayUser.Size();
                                    IncArr(ref moneyArrayUser, size);
                                    Console.WriteLine($"Существующих элементов:{oldsize}");
                                    for (int i = 0; i < size; i++)
                                    {
                                        Console.WriteLine($"Введите рубли и копейки для элемента {oldsize + i} (формат: рубли копейки):");
                                        Input(ref rubless, ref kopekss);
                                        moneyArrayUser[oldsize + i] = new Money(rubless, kopekss);
                                    }
                                    break;
                                case 2:
                                    moneyArrayUser.Display();
                                    break;
                            }                           
                        }
                        forcase4 = new MoneyArray(moneyArrayUser.Size(), moneyArrayUser);
                        break;
                    case 4:
                        if (forcase4.Size() != 0)
                        {
                            int rub = forcase4[0].Rubles;
                            int kop = forcase4[0].Kopeks;
                            int indx = 0;
                            for (int i = 0; i < forcase4.Size(); i++)
                            {
                                if (forcase4[i].Rubles < rub)
                                {
                                    rub = forcase4[i].Rubles;
                                    kop = forcase4[i].Kopeks;
                                    indx = i;
                                }
                                if (forcase4[i].Rubles == rub && forcase4[i].Kopeks < kop)
                                {
                                    rub = forcase4[i].Rubles;
                                    kop = forcase4[i].Kopeks;
                                    indx = i;
                                }
                            }
                            forcase4.Display();
                            Console.WriteLine($"Минимальный элемент с номером:{indx + 1}");
                        }
                        else
                        {
                            Console.WriteLine("Массив пустой");
                        }
                        break;
                    case 5:
                        int index = 0;
                        if (forcase4.Size() != 0)
                        {
                            forcase4.Display();
                            Console.Write("Введите номер элемента:");
                            index = Checknumber(index);
                            while(index-1 >= forcase4.Size() || index -1<0)
                            {
                                Console.WriteLine("Выход за пределы массива");
                                Console.Write("Введите индекс элемента:");
                                index = Checknumber(index);
                            }
                            forcase4[index -1].Write();
                        }
                        else
                        {
                            Console.WriteLine("Пустой массив");
                        }
                        break;
                    case 0:
                        flag = true;
                        break;
                    default:
                        Console.WriteLine("Ошибка");
                        break;
                }
            }
        }
    }
}
