using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVLTreeLib;

namespace TimeTests
{
    class Program
    {
        static void Main(string[] args)
        {
            AVLTree<int, int> tree2 = new AVLTree<int, int>();
            AVLTree<int, int> tree = new AVLTree<int, int>();
            var elements = GenerateRandomIntegers(100000);

            for (int j = 0; j < elements.Count; j++)
            {
                tree2.Add(elements[j], elements[j]);
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int j = 0; j < elements.Count; j++)
            {
                tree.Add(elements[j], elements[j]);
            }
            stopWatch.Stop();
            Console.WriteLine($"AVL tree add: {stopWatch.ElapsedMilliseconds}");

            stopWatch.Restart();
            for (int j = 5000; j < 7000; j++)
            {
                tree.Remove(elements[j]);
            }
            stopWatch.Stop();
            Console.WriteLine($"AVL tree remove: {stopWatch.ElapsedMilliseconds}");

            stopWatch.Restart();
            for (int j = 0; j < elements.Count; j++)
            {
                tree.ContainsKey(elements[j]);
            }
            stopWatch.Stop();

            Console.WriteLine($"AVL tree find: {stopWatch.ElapsedMilliseconds}");
            SortedDictionary<int, int> sd = new SortedDictionary<int, int>();
            stopWatch.Restart();
            for (int j = 0; j < elements.Count; j++)
            {
                sd.Add(elements[j], elements[j]);
            }
            stopWatch.Stop();
            Console.WriteLine($"Sorted dictionary add:{stopWatch.ElapsedMilliseconds}");

            stopWatch.Restart();
            for (int j = 5000; j < 7000; j++)
            {
                sd.Remove(elements[j]);
            }
            stopWatch.Stop();
            Console.WriteLine($"Sorted dictionary remove:{stopWatch.ElapsedMilliseconds}");

            stopWatch.Restart();
            for (int j = 0; j < elements.Count; j++)
            {
                sd.ContainsKey(elements[j]);
            }
            stopWatch.Stop();
            Console.WriteLine($"Sorted dictionary tree find: {stopWatch.ElapsedMilliseconds}");

            Console.ReadKey();
        }

        private static List<int> GenerateRandomIntegers(int count)
        {
            List<int> randomIntegers = new List<int>();
            Random random = new Random(DateTime.Now.Millisecond);

            while (randomIntegers.Count < count)
            {
                int newRandomInteger = random.Next(-2 * count, 2 * count);

                if (!randomIntegers.Contains(newRandomInteger))
                    randomIntegers.Add(newRandomInteger);
            }

            Console.WriteLine("Integers generated");

            return randomIntegers;
        }

    }
}
