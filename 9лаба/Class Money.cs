using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Лаба9
{
    public class Money
    {
        private int rubles;
        private int kopeks;
        private static int objectCount = 0;

        public int Rubles
        {
            get { return rubles; }
            set 
            {
                if (value >= 0)
                    rubles = value;
                else
                    throw new ArgumentException("Рубли не могут быть <0 ");
            }
        }
        public int Kopeks
        {
            get { return kopeks; }
            set  
            {
                if (value >= 0)
                {
                    kopeks = value % 100;
                    rubles += value / 100;
                }
                else
                    throw new ArgumentException("Рубли не могут быть <0 ");
            }
        }
        public static int ObjectCount
        {
            get { return objectCount; }
            set { objectCount += value; }
        }
        public Money() 
        {
            Rubles = 0;
            Kopeks = 0;
            objectCount +=1;
        }
        public Money(int rub, int kop)
        {
            this.Rubles = rub;
            this.Kopeks = kop;
            objectCount++;
        }
        public static int GetObjectCount()
        {
            return objectCount;
        }
      
        public void Reset()
        {
            Rubles = 0; 
            Kopeks = 0;
        }

        
        public static bool operator < (Money a, Money b)
        {
            int totalKopecksA = a.Rubles * 100 + a.Kopeks;
            int totalKopecksB = b.Rubles * 100 + b.Kopeks;
            return totalKopecksA < totalKopecksB;
        }

        public static bool operator >(Money a, Money b)
        {
            int totalKopecksA = a.Rubles * 100 + a.Kopeks;
            int totalKopecksB = b.Rubles * 100 + b.Kopeks;
            return totalKopecksA > totalKopecksB;
        }
        public static Money operator --(Money money)  // Унарная операция -- (уменьшение на 1 копейку)
        {
            if (money.Kopeks > 0)
            {
                money.Kopeks--;
            }
            else if (money.Rubles > 0)
            {
                money.Rubles--;
                money.Kopeks = 99; // Устанавливаем копейки на 99
            }
            return money;
        }
        public static Money operator ++(Money money)  // Унарная операция ++ (увеличение на 1 копейку)
        {
            if (money.Kopeks < 99)
            {
                money.Kopeks++;
            }
            else
            {
                money.Rubles++;
                money.Kopeks = 0; // Сбрасываем копейки
            }
            return money;
        }       
        public static implicit operator int(Money money) // Приведение к int (неявное)
        {
            return money.Rubles; // Возвращаем количество рублей
        }        
        public static explicit operator double(Money money) // Приведение к double (явное)
        {
            double temp = ((double)money.Kopeks % 100);
            return temp; // Возвращаем копейки как  число
        }
        public static Money operator -(Money money, int kopecks)
        {
            int totalKopecks = money.Rubles * 100 + money.Kopeks - kopecks;
            if (totalKopecks < 0)
                totalKopecks = 0; // Не допускаем отрицательных значений
            objectCount--;
            return new Money(totalKopecks / 100, totalKopecks % 100); // Возвращаем новый объект Money

        }
        public static int operator -(int kopecks,Money money)
        {
            int totalKopecks = kopecks - money.Rubles * 100 - money.Kopeks;
            if (totalKopecks < 0)
                totalKopecks = 0; // Не допускаем отрицательных значений
            objectCount--;
            return totalKopecks; 

        }
        public void Write()
        {
            Console.Write(Rubles + " руб ");
            Console.WriteLine(Kopeks + " коп ");
        }

    }
}
