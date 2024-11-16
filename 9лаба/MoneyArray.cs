using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаба9
{
    public class MoneyArray
    {
        private Money[] arr;

        public MoneyArray()
        {
            arr = new Money[0];
        }
        public MoneyArray(int size)
        {
            arr = new Money[size];
            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                arr[i] = new Money(rand.Next(0, 100), rand.Next(0, 100));
            }
        }
        public MoneyArray(int size, MoneyArray temp)
        {
            arr = new Money[size];
            for (int i = 0; i < temp.Size(); i++)
            {
                arr[i] = temp[i];
            }
        }
        // Конструктор с параметрами (пользовательский ввод)
        public MoneyArray(int size, Money[] moneyArray)
        {
            arr = new Money[size];
            Array.Copy(moneyArray, arr, size);
        }

        // Индексатор
        public Money this[int index]
        {
            get
            {
                if (index < 0 || index >= arr.Length)
                {
                    throw new ArgumentException("Индекс выходит за пределы массива.");
                }
                return arr[index];
            }
            set
            {
                if (index < 0 || index >= arr.Length)
                {
                    throw new ArgumentException("Индекс выходит за пределы массива.");
                }
                arr[index] = value;
            }
        }
        public int Size()
        {
            return arr.Length;
        }

        // Метод для просмотра элементов массива
        public void Display()
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write($"Элемент {i+1}: ");
                arr[i].Write();
            }
        }
    }
}
