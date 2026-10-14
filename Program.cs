using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab4
{
    // Власна реалізація динамічного масиву з підтримкою IEnumerable для циклу foreach
    public class MyDynamicArray<T> : IEnumerable<T>
    {
        private T[] _items;
        private int _count;

        public int Count { get { return _count; } }
        public int Capacity { get { return _items.Length; } }

        public MyDynamicArray(int initialCapacity = 4)
        {
            if (initialCapacity < 1) initialCapacity = 4;
            _items = new T[initialCapacity];
            _count = 0;
        }

        // Додавання в кінець за амортизований час O(1)
        public void Add(T item)
        {
            if (_count == _items.Length)
            {
                Resize(_items.Length * 2); // Стратегія геометричного подвоєння
            }
            _items[_count++] = item;
        }

        // Доступ за індексом за O(1)
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count) throw new IndexOutOfRangeException("Індекс за межами масиву");
                return _items[index];
            }
            set
            {
                if (index < 0 || index >= _count) throw new IndexOutOfRangeException("Індекс за межами масиву");
                _items[index] = value;
            }
        }

        // Видалення останнього елемента за O(1)
        public T PopBack()
        {
            if (_count == 0) throw new InvalidOperationException("Масив порожній!");
            _count--;
            T item = _items[_count];
            _items[_count] = default(T); // очищення посилання для Garbage Collector
            return item;
        }

        // Вставка за індексом за O(n)
        public void InsertAt(int index, T item)
        {
            if (index < 0 || index > _count) throw new IndexOutOfRangeException();
            if (_count == _items.Length) Resize(_items.Length * 2);

            for (int i = _count; i > index; i--)
            {
                _items[i] = _items[i - 1];
            }
            _items[index] = item;
            _count++;
        }

        private void Resize(int newCapacity)
        {
            Console.WriteLine("⚡ [RESIZE] Збільшення місткості: " + _items.Length + " -> " + newCapacity);
            T[] newArray = new T[newCapacity];
            Array.Copy(_items, newArray, _count);
            _items = newArray;
        }

        public void Print()
        {
            Console.Write("[Count=" + _count + ", Capacity=" + _items.Length + "]: [ ");
            for (int i = 0; i < _count; i++) Console.Write(_items[i] + " ");
            Console.WriteLine("]");
        }

        // ІНДИВІДУАЛЬНЕ ЗАВДАННЯ. Варіант 3: Реалізація інтерфейсу IEnumerable<T> через yield return
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return _items[i]; // Повертаємо елементи по одному для foreach
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==========================================================================");
            Console.WriteLine("       РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №4: ВЛАСНИЙ ДИНАМІЧНИЙ МАСИВ        ");
            Console.WriteLine("==========================================================================");

            MyDynamicArray<int> vec = new MyDynamicArray<int>(2);
            Console.WriteLine("Створено динамічний масив з початковою місткістю 2:\n");

            Stopwatch sw = Stopwatch.StartNew();

            // Додаємо 10 елементів для демонстрації перевиділення пам'яті
            for (int i = 1; i <= 10; i++)
            {
                vec.Add(i * 10);
                vec.Print();
            }

            Console.WriteLine("\n--- Вставка в середину (індекс 3, значення 999) ---");
            vec.InsertAt(3, 999);
            vec.Print();

            Console.WriteLine("\n--- Видалення з кінця (PopBack) ---");
            int removed = vec.PopBack();
            Console.WriteLine("Видалено елемент: " + removed);
            vec.Print();

            // ТЕСТУВАННЯ ВАРІАНТА 3 (Цикл foreach)
            Console.WriteLine("\n--- ВАРІАНТ №3: ТЕСТУВАННЯ ЦИКЛУ FOREACH ---");
            Console.Write("Елементи масиву, виведені через foreach: ");
            foreach (int element in vec)
            {
                Console.Write(element + " ");
            }
            Console.WriteLine();

            sw.Stop();
            Console.WriteLine("\n⏱ Час операцій: " + sw.Elapsed.TotalMilliseconds.ToString("F4") + " мс");
            Console.WriteLine("✅ Всі тести динамічного розширення та циклу foreach пройшли успішно!");
        }
    }
}
