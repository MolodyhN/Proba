using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _5Лаба
{
    internal class Program
    {
        static int Checknumber(int number)  //Проверка ввода
        {
            number = 0;
            while (!int.TryParse(Console.ReadLine(), out number))
            {
                Console.WriteLine("Ошибка");
            }
            return number;
        }
        static int CheckNumberSwitch()  //Проверка ввода для Switch
        {
            int forswitch = 0;
            forswitch = Checknumber(forswitch);
            while (forswitch != 1 && forswitch != 2 && forswitch != 3)
            {
                Console.WriteLine("Ошибка");
                forswitch = Checknumber(forswitch);               
            }
            return forswitch;
        }
        static int CheckNumberCase()  //Проверка ввода для Switch
        {
            int number = 0;
            number = Checknumber(number);
            while (number != 1 && number != 2)
            {
                Console.WriteLine("Ошибка");
                number = Checknumber(number);
            }
            return number;
        }
        static void Rand(ref int[]temp,int k)  //Ввод ДСЧ для одномерного
        {
            Random rnd = new Random();
            for (int i = 0; i < k; i++)
            {
                temp[i] = rnd.Next(1,20);
            }
        }
        static void Rand(ref int[,] temp, int n, int k)  //Ввод ДСЧ для двумерного
        {
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    temp[i,j] = rnd.Next(1, 20);
                }
            }
        }
        static void Rand(ref int[][] temp,int n,int k)  //Ввод ДСЧ для рваного
        {
            Random rnd = new Random();
            for (int j = 0; j < n; j++)
            {
                temp[j] = new int[k];
                for (int i = 0; i < k; i++)
                {
                    temp[j][i] = rnd.Next(1,20);
                }
            }
        }
        
        static void Rand(ref int[] temp,int n,int k)  //Ввод ДСЧ для одномерного в определенном индексе
        {
            Random rnd = new Random();
            for (int i = n; i < n+k; i++)
            {
                temp[i] = rnd.Next(1, 20);
            }
        }
        static void Print(params int[]temp)  //Вывод одномерного
        {
            string s = " ";
            foreach (int i in temp)
            {
                s += i;
                s += " ";
            }
            Console.WriteLine(s);
        }
        static void Print(int n,int k ,ref int[,] temp)  //Вывод двумерного
        {
            string s = " ";
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    s += temp[i,j];
                    s += " ";
                }
                Console.WriteLine(s);
                s = " ";
            }
        }
        static void Print(int n,params int[][] temp)  //Вывод рваного
        {
            string s = " ";
            for (int j = 0; j < n; j++) {
                foreach (int i in temp[j])
                {
                    s += i;
                    s += " ";
                }
                Console.WriteLine(s);
                s = " ";
            }
        }
        static void EnteringTwoВimensional(ref int[,]temp,int n,int k)
        {
            int number = 0;
            Console.WriteLine("Введите элементы:");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine(i);
                for (int j = 0; j < k; i++)
                {
                    temp[i, j] = Checknumber(number);
                }
            }
        }
        static void Addingelements(int n,int k,ref int[]temp,ref int[]temp2)
        {            
            for(int i = 0; i < n; i++)
            {
                temp2[i] = temp[i];
            }
            for (int i = n+k; i < temp.Length + k; i++)
            {
                temp2[i] = temp[i-k];
            }
            temp = temp2;
        }
        static int RemoveEven(int n,int k ,ref int[,]temp)
        {
            int stringstemp2 = 0;
            int[,] temp2 = new int[n,k];
            for (int i = 0; i < n; i+=2)
            {
                for (int j = 0; j < k; j++)
                {
                    temp2[stringstemp2,j] = temp[i,j];
                }
                stringstemp2++;
            }
            temp = temp2.Clone() as int[,];
            return stringstemp2;
        }
        static int Addingline(int n, int k, ref int[][] temp, params int[] temp2)
        {
            int[][] temp3 = new int[n+1][];
            
            Array.Copy(temp, temp3, temp.Length);
            temp3[n] = new int[k];
            for (int i = 0; i < k; i++)
            {
                temp3[n][i] = temp2[i];
            }
            temp = temp3.Clone() as int[][];
            return n + 1;
        }
        static void Manualinput(ref int[]temp,int size)
        {
            int number = 0;
            Console.WriteLine("Введите элементы:");
            temp = new int[size];
            for (int i = 0;i < size; i++)
            { 
                temp[i] = Checknumber(number);
            }
        }
        static int ForCase1(int size,ref int[] arr1,ref bool flg)
        {
            int n = 0;
            int k = 0;
            int forswitch = 0;
            Print(arr1);
            Console.WriteLine("1.Ручной ввод\n" + "2.ДСЧ\n" + "3.Выход");
            if ((forswitch = CheckNumberSwitch()) == 3)
            {
                flg = true;
                return size;
            }
            Console.Write("Введите после какого номера вставить: ");
            n = Checknumber(n); 
            while (n < 0 || n > size)
            {
                Console.WriteLine("Ошибка");
                n = Checknumber(n);
            }
            Console.Write("Введите кол-во элементов для вставки: ");
            k = Checknumber(k);
            while (k < 0)
            {
                Console.WriteLine("Ошибка");
                k = Checknumber(k);
            }
            int[] arr2 = new int[size+k];
            switch (forswitch) {
                case 1:
                    int number = 0;
                    Console.WriteLine("Введите элементы:");
                    for (int i = n; i < n+k; i++)
                    {
                        arr2[i] = Checknumber(number);
                    }
                    Addingelements(n, k, ref arr1,ref arr2);
                    break;
                case 2:
                    Rand(ref arr2, n, k);
                    Addingelements(n, k, ref arr1, ref arr2);
                    break;
                default: break;
            }
            Print(arr1);
            return size + k;
        }
        static int ForCase2(int n,int k,ref int[,]arr2)
        {
            Print(n ,k,ref arr2);
            Console.WriteLine("После удаления четных:");
            n = RemoveEven(n, k, ref arr2);
            Print(n, k,ref arr2);
            return n;
        }
        static int ForCase3(int n,int k,ref int[][]arr3,ref bool flg)
        {
            int[] newarr3 = new int[k];
            Print(n, arr3);
            int forswitch = 0;
            Console.WriteLine("1.Ручной ввод\n" + "2.ДСЧ\n" + "3.Выход");
            if ((forswitch = CheckNumberSwitch()) == 3)
            {
                flg = true;
                return n;
            }
            switch (forswitch)
            {
                case 1:
                    Console.WriteLine("Введите доп строку");
                    Manualinput(ref newarr3, k);
                    Console.WriteLine("После добавления:");
                    n = Addingline(n, k, ref arr3, newarr3);
                    Print(n, arr3);
                    break;
                case 2:
                    Rand(ref newarr3, k);
                    Console.WriteLine("После добавления:");
                    n = Addingline(n, k, ref arr3, newarr3);
                    Print(n, arr3);
                    break;
            }
            return n;
        }
        static void Main(string[] args)
        
        {
            int n = -1;
            int k = -1;
            int forswitch = 0;
            bool flag = false;
            while (!flag) {
                Console.Clear();
                Console.WriteLine("1.Работа с одномерным массивом\n" + "2.Удаление четных строк в двумерном массиве\n" + "3.Добавление строки в конец двумерного массива\n" + "4.Выход");
                switch (Checknumber(forswitch)) {
                    case 1:
                        Console.WriteLine("1.Ручной ввод\n" + "2.ДСЧ\n" + "3.Выход");
                        if ((forswitch = CheckNumberSwitch()) == 3)
                        {
                            break;
                        }
                        int size = 0;
                        Console.Write("Введите размер массива: ");
                        size = Checknumber(size);
                        while (size <= 0)
                        {
                            Console.WriteLine("Ошибка");
                            size = Checknumber(size);
                        }
                        int[] arr1 = new int[size];
                        switch (forswitch)
                        {
                            case 1:
                                bool flg = false;
                                Manualinput(ref arr1, size);
                                while (!flg)
                                {
                                    size = ForCase1(size, ref arr1,ref flg);
                                    if (flg)
                                    {
                                        break;
                                    }
                                    Console.Write("1.Добавить еще раз\n"+ "2.Выход\n");
                                    if (CheckNumberCase() == 2)
                                    {
                                        flg = true;
                                    }

                                }
                                break;
                            case 2:
                                flg = false;
                                Rand(ref arr1, size);
                                while (!flg)
                                {
                                    size = ForCase1(size, ref arr1,ref flg);
                                    if (flg)
                                    {
                                        break;
                                    }
                                    Console.Write("1.Добавить еще раз\n"+ "2.Выход\n");
                                    if (CheckNumberCase() == 2)
                                    {
                                        flg = true;
                                    }
                                }
                                 break;
                            default:
                                 Console.WriteLine("Ошибка");
                                 break;
                        }
                        break;                  
                    case 2:
                        n = -1;
                        k = -1;
                        Console.WriteLine("1.Ручной ввод\n" + "2.ДСЧ\n" + "3.Выход");
                        if ((forswitch = CheckNumberSwitch()) == 3)
                        {
                            break;
                        }
                        Console.Write("Введите кол-во строк: ");
                        n = Checknumber(n);
                        while (n <= 0)
                        {
                            Console.WriteLine("Ошибка");
                            n = Checknumber(n);
                        }
                        Console.Write("Введите кол-во элементов в строке: ");
                        k = Checknumber(k);
                        while (k <= 0) {
                            Console.WriteLine("Ошибка");
                            k = Checknumber(k);
                        }
                        int[,] arr2 = new int[n,k]; 
                        switch (forswitch)
                        {
                            case 1:
                                bool flg = false;
                                for (int i = 0; i < n; i++)
                                {
                                    EnteringTwoВimensional(ref arr2,n,k);
                                }
                                while (!flg)
                                {
                                    n = ForCase2(n, k, ref arr2);
                                    Console.Write("1.Удалить еще раз\n"+ "2.Выход\n");
                                    if (CheckNumberCase() == 2)
                                    {
                                        flg = true;
                                    }
                                }
                                
                                break;
                            case 2:
                                flg = false;
                                Rand(ref arr2, n, k);
                                while (!flg)
                                {
                                    n = ForCase2(n, k, ref arr2);
                                    if(n == 1)
                                    {
                                        Console.ReadKey();
                                        break;
                                    }
                                    Console.Write("1.Удалить еще раз\n" + "2.Выход\n");
                                    if (CheckNumberCase() == 2)
                                    {
                                        flg = true;
                                    }
                                }
                                break;
                            case 3:
                            default:
                                Console.WriteLine("Ошибка");
                                break;
                        }
                        flag = false;
                        break;
                    case 3:
                        Console.WriteLine("1.Ручной ввод\n" + "2.ДСЧ\n" + "3.Выход");
                        if ((forswitch = CheckNumberSwitch()) == 3)
                        {
                            break;
                        }
                        Console.Write("Введите кол-во строк: ");
                        n = Checknumber(n);
                        while (n <= 0)
                        {
                            Console.WriteLine("Ошибка");
                            n = Checknumber(n);
                        }
                        Console.Write("Введите кол-во элементов в строке: ");
                        k = Checknumber(k);
                        while (k <= 0)
                        {
                            Console.WriteLine("Ошибка");
                            k = Checknumber(k);
                        }
                        int[][] arr3 = new int[n][];
                        switch (forswitch)
                        {
                            case 1:
                                bool flg = false;
                                for (int i = 0; i < n; i++)
                                {
                                    Manualinput(ref arr3[i], k);
                                }
                                while (!flg)
                                {
                                    n = ForCase3(n, k, ref arr3,ref flg);
                                    if (flg)
                                    {
                                        break;
                                    }
                                    Console.Write("1.Добавить еще раз\n"+"2.Выход\n");
                                    if (CheckNumberCase() == 2)
                                    {
                                        flg = true;
                                    }
                                }
                                break;
                            case 2:
                                flg = false;
                                Rand(ref arr3, n, k);
                                while (!flg)
                                {
                                    n = ForCase3(n, k, ref arr3, ref flg);
                                    if (flg)
                                    {
                                        break;
                                    }
                                    Console.Write("1.Добавить еще раз\n" + "2.Выход\n");
                                    if (CheckNumberCase() == 2)
                                    {
                                        flg = true;
                                    }
                                }
                                break;
                            default:
                                Console.WriteLine("Ошибка");
                                break;
                        }
                    flag = false;
                    break;
                    case 4:
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
