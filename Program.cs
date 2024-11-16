using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Калькулятор2
{
    internal class Program
    {
        class Dict
        {
            public char name;
            public List<int> set;
            public Dict(char n, List<int> a) { name = n; set = a;}
        };
        static void Random(ref List<int> Name,int right,int left)
        {
            var x = new Random();
            int size = x.Next(1, right - left + 1);
            for (int i = 0; i < size; i++)
            {
                Name.Add(x.Next(left, right));
            }
            deleteDublicates(ref Name);
        }
        static void universum(ref List<int> Name,ref int left,ref int right)
        {
            bool iscorrect = false;
            Console.WriteLine("Введите границы юниверсума");
            while (!iscorrect)
            {
                Console.WriteLine("Левая");
                iscorrect = int.TryParse(Console.ReadLine(), out left);
            }
            iscorrect = false;
            while (!iscorrect)
            {
                Console.WriteLine("Правая");
                iscorrect = int.TryParse(Console.ReadLine(), out right);
            }
            for (int i = left; i < right; i++)
            {
                Name.Add(i);
            }
        }
        static void SeterAr(ref List<int> Name, int left,int right)           
        {
            Console.WriteLine("Введите массив,при завершении введите D");
            string input;
            while (true)
            {
                input = Console.ReadLine();
                if (input.ToUpper() == "D")
                {
                    break;
                }

                if (int.TryParse(input, out int number) && (number > left-1) && (number < right+1))
                {
                    Name.Add(number);
                }
                else
                {
                    Console.WriteLine("Неверные данные, повторите попытку.");
                }
            }
            deleteDublicates(ref Name);

        }
        static void GeterAr(List<int> Name)
        {
            Name.Sort();
            if (Name.Count() == 0)
            {
                Console.WriteLine("Пустое");
            }
            else
            {
                string str = " ";
                foreach (int i in Name)
                {
                    str += i + " ";
                }
                Console.WriteLine(str);
            }
        }
        static void deleteDublicates(ref List<int> L) //Удаление дубликатов
        {
            L.Sort();
            for(int i = 0; i < L.Count - 1; i++)
            {
                if (L[i] == L[i + 1])
                {
                    L.RemoveAt(i);
                    i--;
                }
            }
        }
        static void Union(List<int> L1, List<int> L2) //Обьеденение
        {
            deleteDublicates(ref L1);
            deleteDublicates(ref L2);
            List <int> temp = new List<int>();
            foreach (int i in L1)
                temp.Add(i);
            foreach (int i in L2)
                temp.Add(i);
            if (temp.Count == 0)
                Console.Write("Пустое множество");
            else
            {
                deleteDublicates(ref temp);
                Console.WriteLine("Результат объединения");
                GeterAr(temp);
            }
            Console.WriteLine();
        }
        static void Intersaction(List<int> L1, List<int> L2) //Пересечение
        {
            List<int> temp = new List<int>();
            deleteDublicates(ref L1);
            deleteDublicates(ref L2);
            int i = 0, j = 0;
            while (i < L1.Count && j < L2.Count)
            {
                if (L1[i] < L2[j])
                {
                    i++;
                }
                else if (L1[i] > L2[j])
                {
                    j++;
                }
                else
                {
                    temp.Add(L1[i]);
                    i++;
                    j++;
                }
            }

            if (temp.Count == 0)
            {
                Console.WriteLine("Пустое множество");
            }
            else
            {
                Console.WriteLine("Результат пересечения");
                GeterAr(temp);
        }
    }
        static void Difference(List<int> L1, List<int> L2) //Разность
        {
            List<int> temp = new List<int>();
            deleteDublicates(ref L1);
            deleteDublicates(ref L2);
            int j = 0;
            for (int i = 0; i < L1.Count; i++)
            {
                while (j < L2.Count && L2[j] < L1[i])
                {
                    j++;
                }
                if (j < L2.Count && L1[i] == L2[j])
                {
                    j++;
                }
                else
                {
                    temp.Add(L1[i]);
                }
            }
            if (temp.Count == 0)
                Console.Write("Пустое множество");
            else
            {
                Console.WriteLine("Результат разности");
                GeterAr(temp);
            }
            Console.WriteLine();
        }
        static void SymmetricDifference(List<int> L1, List<int> L2) //Симетрическа разность
        {
            List<int> temp = new List<int>();
            deleteDublicates(ref L1);
            deleteDublicates(ref L2);
            for (int i = 0; i < L1.Count; i++)
            {
                if (!L2.Contains(L1[i]))
                {
                    temp.Add(L1[i]);
                }
            }
            for (int i = 0; i < L2.Count; i++)
            {
                if (!L1.Contains(L2[i]))
                {
                    temp.Add(L2[i]);
                }
            }
            if (temp.Count == 0)
                Console.Write("Пустое множество");
            else
            {
                Console.WriteLine("Результат симметрической разности");
                GeterAr(temp);
            }
            Console.WriteLine();
        }
        static void Inversion(List<int> U, List<int> L) //Дополнение
        {
            List<int> temp = new List<int>();
            deleteDublicates(ref L);
            U.Sort();
            int j = 0;
            for (int i = 0; i < U.Count(); i++)
            {
                if (j < L.Count && L[j] == U[i])
                {
                    j++;
                }
                else temp.Add(U[i]);
            }
            if (temp.Count == 0)
                Console.Write("Пустое множество");
            else
            {
                Console.WriteLine("Результат дополнения");
                GeterAr(temp);
            }
            Console.WriteLine();
        }
        static void EnteringSet(List<int> L1, List<int> L2) //Входит ли одно подмножество(L2) в другое(l1)
        {
            int counter = 0;
            for (int i = 0; i < L2.Count; i++) { 
                if (L1.Contains(L2[i])) { counter++; }
            }
            if(counter == L2.Count) { Console.WriteLine("Входит"); }
            else { Console.WriteLine("Не входит"); }
        }
        static void EnteringTwoSets(ref char temp,ref char temp1,ref Dictionary<int,Dict> setMapping,ref List<List<int>>Switch) //Ввод двух множеств
        {
            bool iscorrect = false;
            bool fwhile = false;
            string input;
            while (!iscorrect && !fwhile)
            {
                Console.WriteLine("Введите первое множество для действия");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    temp = char.ToUpper(input[0]);
                    if (temp == 'A' || temp == 'B' || temp == 'C' || temp == 'D' || temp == 'E')
                    {
                        iscorrect = true;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка.Введите из существующих");
                    }
                }
                else 
                {
                    Console.WriteLine("Пожалуйста, введите  один символ.");
                }
                for (int i = 0; i < setMapping.Count(); i++)
                {
                    if (temp == setMapping[i].name)
                    {
                        Switch[0] = setMapping[i].set;
                        GeterAr(Switch[0]);
                        fwhile = true;
                        break;
                    }
                    
                }
            }
            iscorrect = false;
            fwhile= false;
            while (!iscorrect && ! fwhile)
            {
                Console.WriteLine("Введите второе множество для действия");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    temp1 = char.ToUpper(input[0]);
                    if (temp1 == 'A' || temp1 == 'B' || temp1 == 'C' || temp1 == 'D' || temp1 == 'E')
                    {
                        iscorrect = true;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка.Введите из существующих");
                    }
                }
                else
                {
                    Console.WriteLine("Пожалуйста, введите  один символ.");
                }
                for (int i = 0; i < setMapping.Count(); i++)
                {
                    if (temp1 == setMapping[i].name)
                    {
                        Switch[1] = setMapping[i].set;
                        GeterAr(Switch[1]);
                        fwhile = true;
                        break;
                    }
                }
            }
        }
        static void EnteringOneSet(ref Dictionary<int, Dict> setMapping, ref List<List<int>> Switch) //Ввод одного множества
        {
            bool iscorrect = false;
            string input;
            char temp;
            while (!iscorrect)
            {
                Console.WriteLine("Введите множество");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    temp = char.ToUpper(input[0]);
                    if (temp == 'A' || temp == 'B' || temp == 'C' || temp == 'D' || temp == 'E')
                    {
                        for (int i = 0; i < setMapping.Count(); i++)
                        {
                            if (temp == setMapping[i].name)
                            {
                                Switch[0] = setMapping[i].set;
                                break;
                            }
                        }
                        iscorrect = true;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка.Введите из существующих");
                    }
                }
                else
                {
                    Console.WriteLine("Пожалуйста, введите  один символ.");
                }
            }
        }
        static int Number()
        {
            int number = 0;
            bool iscorrect = false;
            iscorrect = int.TryParse(Console.ReadLine(), out number);
            while (!iscorrect)
            {
                Console.WriteLine("Ошибка.Введите снова");
                iscorrect = int.TryParse(Console.ReadLine(), out number);
            }
            return number;
        }
        static void Main(string[] args)
        {
            int number = 0;
            bool fwhile = false;
            bool iscorrect = false;
            int left = 0; int right = 0;
            List<int> U = new List<int>();
            universum(ref U, ref left, ref right);
            List<int> A = new List<int>();
            List<int> B = new List<int>();
            List<int> C = new List<int>();
            List<int> D = new List<int>(); 
            List<int> E = new List<int>();
            List<int> case7 = new List<int>();
            List<List<int>> Switch = new List<List<int>>(3);
            Dict A1 = new Dict('A', A);
            Dict B1 = new Dict('B', B);
            Dict C1 = new Dict('C', C);
            Dict D1 = new Dict('D', D);
            Dict E1 = new Dict('E', E);
            var setMapping = new Dictionary<int, Dict>()
             {
                    
                { 0 , A1},
                { 1 , B1 },
                { 2 , C1 },
                { 3 , D1 },
                { 4 , E1 }
            };
            for (int i = 0; i < setMapping.Count(); i++)
            {
                Console.WriteLine($"Как вы хотите ввести множество {setMapping[i].name}");
                iscorrect = false;
                bool input = false;
                int SwitchNumber;
                while (!iscorrect)
                {
                    do
                    {
                        Console.WriteLine("1.Консоль" + "\n" + "2.Рандом");
                        input = int.TryParse(Console.ReadLine(), out SwitchNumber);
                    }
                    while (!input);
                    switch (SwitchNumber)
                    {
                        case 1:
                            SeterAr(ref setMapping[i].set, left, right);
                            iscorrect = true;
                            break;
                        case 2:
                            Random(ref setMapping[i].set, right, left);
                            iscorrect = true;
                            break;
                        default:
                            Console.WriteLine("Такого нет =)");
                            break;
                    }
                }
            }
            Switch.Add(A);
            Switch.Add(B);
            while (!fwhile)
            {
                char temp = ' ';
                char temp1 =' ';
                Console.WriteLine("Выберете действие:");
                Console.WriteLine("1.Обьеденение"+"\n"+ "2.Пересечение"+"\n"+ "3.Разность"+"\n"+ "4.Симетрическа разность"+"\n"+ "5.Дополнение" + "\n" + "6.Принадлежит ли элемент" + "\n" + "7.Принадлежит ли множество"+"\n"+"8.Выход");
                switch (Number())
                {
                    case 1:
                        EnteringTwoSets(ref temp, ref temp1, ref setMapping, ref Switch);
                        Union(Switch[0], Switch[1]);
                        break;
                    case 2:
                        EnteringTwoSets(ref temp, ref temp1, ref setMapping, ref Switch);
                        Intersaction(Switch[0], Switch[1]);
                        break;
                    case 3:
                        EnteringTwoSets(ref temp, ref temp1, ref setMapping, ref Switch);
                        Difference(Switch[0], Switch[1]);
                        break;
                    case 4:
                        EnteringTwoSets(ref temp, ref temp1, ref setMapping, ref Switch);
                        SymmetricDifference(Switch[0], Switch[1]);
                        break;
                    case 5:
                        EnteringOneSet(ref setMapping,ref Switch);
                        Inversion(U, Switch[0]);
                        break;
                    case 6:
                        EnteringOneSet(ref setMapping, ref Switch);
                        Console.WriteLine("Введите число");
                        if ((Switch[0]).Contains(Number()))
                        {
                            Console.WriteLine("Содержит");
                        }
                        else
                        {
                            Console.WriteLine("Не содержит");
                        }
                        break;
                    case 7:
                        Console.WriteLine("1.Ввести вручную" + "\n" + "2.Из существующих");
                        iscorrect = false;
                        while (!iscorrect)
                        {
                            switch (Number())
                            {
                                case 1:
                                    SeterAr(ref case7, left, right);
                                    Console.WriteLine("В какое множество оно должно входить:");
                                    EnteringOneSet(ref setMapping, ref Switch);
                                    EnteringSet(Switch[0], case7);
                                    iscorrect = true;
                                    break;
                                case 2:
                                    EnteringTwoSets(ref temp, ref temp1, ref setMapping, ref Switch);
                                    EnteringSet(Switch[1], Switch[0]);
                                    iscorrect = true;
                                    break;
                                default:
                                    Console.WriteLine("Такого нет");
                                    break;
                            }
                        }
                        break;
                    case 8:
                        fwhile = true;
                        break;
                    default:
                        Console.WriteLine("Такого нет");
                        break;
                        
                }
            }
        }
    }
}
