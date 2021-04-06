using System;

namespace Sample.NWayCache.Client
{
    /// <summary>
    /// Driver Class for thge Console Application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point to the console
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Test1WayCache();

            Test2WayCache();

            Console.ReadKey();
        }

        /// <summary>
        /// Adds the items into1 way cache.
        /// </summary>
        private static void Test1WayCache()
        {
            SetAssociativeCache<int, int> setAssociativeCache = new SetAssociativeCache<int, int>(1, 8);

            Console.WriteLine("Adding Items into 1 Way Cache with a cache capacity of 8 and trying to add 10 items from 1 to 10 using LRU Cache Policy");

            //add items to cache
            for (int i = 1; i <= 10; i++)
            {
                setAssociativeCache.Add(i, i);
            }

            Console.WriteLine("Done Adding Items into 1 Way Cache with a cache capacity of 8 and trying to add 10 items from 1 to 10 using LRU Cache Policy");

            Console.WriteLine("Reading Items from the 1 Way Cache with a cache capacity of 8 and reading 10 items from 1 to 10 using LRU Cache Policy");

            //read items from cache and print
            for (int key = 1; key <= 10; key++)
            {
                int value;

                setAssociativeCache.TryGetValue(key, out value);

                Console.WriteLine("Key- " + key + ": " + "Value- "+ value);
            }

            Console.WriteLine("Cache Sets Count: " + setAssociativeCache.Count());

            Console.WriteLine("Cache Items Count: " + setAssociativeCache.ItemsCount());

            Console.WriteLine("Done Reading Items from the 1 Way Cache with a cache capacity of 8 and reading 10 items from 1 to 10 using LRU Cache Policy");
        }

        /// <summary>
        /// Adds the items into2 way cache.
        /// </summary>
        private static void Test2WayCache()
        {
            SetAssociativeCache<int, int> setAssociativeCache = new SetAssociativeCache<int, int>(2, 3);

            Console.WriteLine("Adding Items into 2 Way Cache with a cache capacity of 3 and trying to add 10 items from 1 to 10 using LRU Cache Policy");

            //add items to cache
            for (int i = 1; i <= 10; i++)
            {
                setAssociativeCache.Add(i, i);
            }

            Console.WriteLine("Done Adding Items into 2 Way Cache with a cache capacity of 3 and trying to add 10 items from 1 to 10 using LRU Cache Policy");

            Console.WriteLine("Reading Items from the 2 Way Cache with a cache capacity of 3 and reading 10 items from 1 to 10 using LRU Cache Policy");

            //read items from cache and print
            for (int key = 1; key <= 10; key++)
            {
                int value;

                setAssociativeCache.TryGetValue(key, out value);

                Console.WriteLine("Key- " + key + ": " + "Value- " + value);
            }

            Console.WriteLine("Cache Sets Count: " + setAssociativeCache.Count());

            Console.WriteLine("Cache Items Count: " + setAssociativeCache.ItemsCount());

            Console.WriteLine("Done Reading Items from the 2 Way Cache with a cache capacity of 3 and reading 10 items from 1 to 10");
        }
    }
}
