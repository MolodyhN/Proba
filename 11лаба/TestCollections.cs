using lib10;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаба_11
{
    public class TestCollections<TKey, TValue>
         where TKey : Engine // Ограничение: тип ключа должен быть или наследовать Engine
         where TValue : ICEngine, new() // Ограничение: TValue должен наследовать TKey и иметь конструктор без параметров
    {
        public Stack<TValue> Collection1 { get; private set; } // Коллекция 1
        public Stack<string> Collection2 { get; private set; } // Коллекция 2
        public Dictionary<TKey, TValue> Collection3 { get; private set; } // Коллекция 3
        public Dictionary<string, TValue> Collection4 { get; private set; } // Коллекция 4

        public TestCollections(int count)
        {
            Collection1 = new Stack<TValue>();
            Collection2 = new Stack<string>();
            Collection3 = new Dictionary<TKey, TValue>();
            Collection4 = new Dictionary<string, TValue>();

            for (int i = 0; i < count; i++)
            {
                TValue item = new TValue();
                item.RandomInit();

                TKey baseObject = (TKey)item.BaseEngine;

                string uniqueKey = item.Model;
                
                Collection1.Push(item);
                Collection3.Add(baseObject, item);
                while (Collection4.ContainsKey(uniqueKey))
                {
                    item.RandomInit_Model();
                    uniqueKey = item.Model;
                }
                Collection2.Push(uniqueKey);
                Collection4.Add(item.ToString(), item);
            }
        }
        public int AddElement(TValue item)
        {
           if (Collection1.Contains(item))
            {
                return -1;
            }
           else
            {
                TKey baseObject = (TKey)item.BaseEngine;

                string uniqueKey = item.Model;

                Collection1.Push(item);
                Collection2.Push(uniqueKey);
                Collection3.Add(baseObject, item);              
                Collection4.Add(item.ToString(), item);
                return 1;
            }
        }
        public void RemoveElement(string key)
        {
            TValue item = null;
            foreach(TValue i in Collection1)
            {
                if (i.Model == key)
                {
                    item = i;     
                }
            }
            if (item != null){
                Stack<TValue> Copy1 = new Stack<TValue>(Collection1);
                Stack<string> Copy2 = new Stack<string>(Collection2);
                Collection1.Clear();
                while (Copy1.Count > 1)
                {
                    if (Copy1.Peek() == item) { }
                    else
                        Collection1.Push(Copy1.Pop());
                }

                Collection2.Clear();
                foreach (string i in Copy2)
                {
                    if (i == item.Model) { }
                    else
                        Collection2.Push(i);
                }

                TKey baseObject = (TKey)item.BaseEngine;
                Console.WriteLine(Collection3.ContainsKey(baseObject));
                if (Collection3.ContainsKey(baseObject))
                {
                    Collection3.Remove(baseObject);
                }

                if (Collection4.ContainsKey(item.ToString()))
                {
                    Collection4.Remove(item.ToString());
                }
            }
            else
            {
                Console.WriteLine("Не существует");
            }
        }
        public void MeasureSearchTime()
        {
            TValue first = Collection1.Peek();

            TValue[] itemsArray = Collection1.ToArray();

            TValue middle = itemsArray[itemsArray.Length / 2];
            TValue last = itemsArray[itemsArray.Length - 1];
            TValue notInCollection = new TValue();
            notInCollection.RandomInit();

            var searchElements = new[] { first, middle, last, notInCollection };

            Console.WriteLine($"first:{first}\n" + $"middle:{middle}\n" + $"last:{last}\n" + $"notInCollection:{notInCollection}\n");
            Console.WriteLine("Measuring search time...");
            MeasureTimeForCollection1(searchElements);
            MeasureTimeForCollection2(searchElements);
            MeasureTimeForCollection3(searchElements);
            MeasureTimeForCollection4(searchElements);
        }

        private void MeasureTimeForCollection1(TValue[] searchElements)
        {
            int i = 1;
            Console.WriteLine("Collection1<TValue>: Stack<TValue>");
            foreach (var element in searchElements)
            {
                Stopwatch sw = Stopwatch.StartNew();
                bool found = Collection1.Contains(element);
                sw.Stop();
                double timeInMicroseconds = (sw.ElapsedTicks / (double)Stopwatch.Frequency) * 1_000_000;
                Console.WriteLine($"{i}: Found = {found}, Time = {timeInMicroseconds} мкс");
                i++;
            }
        }

        private void MeasureTimeForCollection2(TValue[] searchElements)
        {
            int i = 1;
            Console.WriteLine("\nCollection2<string>: Stack<string>");
            foreach (var element in searchElements)
            {
                Stopwatch sw = Stopwatch.StartNew();
                bool found = Collection2.Contains(element.ToString());
                sw.Stop();
                double timeInMicroseconds = (sw.ElapsedTicks / (double)Stopwatch.Frequency) * 1_000_000;
                Console.WriteLine($"{i}: Found = {found}, Time = {timeInMicroseconds} мкс");
                i++;
            }
        }

        private void MeasureTimeForCollection3(TValue[] searchElements)
        {
            int i = 1;
            Console.WriteLine("\nCollection3<TKey, TValue>: Dictionary<TKey, TValue>");
            foreach (var element in searchElements)
            {
                TKey key = (TKey)(object)element.BaseEngine;
                Stopwatch sw = Stopwatch.StartNew();
                bool found = Collection3.ContainsKey(key);
                sw.Stop();
                double timeInMicroseconds = (sw.ElapsedTicks / (double)Stopwatch.Frequency) * 1_000_000;
                Console.WriteLine($"{i}: Found = {found}, Time = {timeInMicroseconds} мкс");
                i++;
            }
        }

        private void MeasureTimeForCollection4(TValue[] searchElements)
        {
            int i = 1;
            Console.WriteLine("\nCollection4<string, TValue>: Dictionary<string, TValue>");
            foreach (var element in searchElements)
            {
                string key = element.ToString();
                Stopwatch sw1 = Stopwatch.StartNew();
                bool foundKey = Collection4.ContainsKey(key);
                sw1.Stop();

                Stopwatch sw2 = Stopwatch.StartNew();
                bool foundValue = Collection4.ContainsValue(element);
                sw2.Stop();
                double timeInMicroseconds1 = (sw1.ElapsedTicks / (double)Stopwatch.Frequency) * 1_000_000;
                double timeInMicroseconds2 = (sw2.ElapsedTicks / (double)Stopwatch.Frequency) * 1_000_000;
                Console.WriteLine($"{i}: FoundKey = {foundKey}, FoundValue = {foundValue}, TimeKey = {timeInMicroseconds1} мкс, TimeValue = {timeInMicroseconds2} мкс");
                i++;
            }
        }
        public void DisplayCollections()
        {
            Console.WriteLine("Collection1 (Stack<TValue>):");
            foreach (var item in Collection1)
            {
                item.Show();
            }

            Console.WriteLine("\nCollection2 (Stack<string>):");
            foreach (var key in Collection2)
            {
                Console.WriteLine($"Key: {key}");
            }

            Console.WriteLine("\nCollection3 (Dictionary<TKey, TValue>):");
            foreach (var pair in Collection3)
            {
                Console.WriteLine($"Key: {pair.Key.Model}, Value: {pair.Value.Model}");
            }

            Console.WriteLine("\nCollection4 (Dictionary<string, TValue>):");
            foreach (var pair in Collection4)
            {
                Console.WriteLine($"Key: {pair.Key}, Value: {pair.Value.Model}");
            }
        }
    }
}
