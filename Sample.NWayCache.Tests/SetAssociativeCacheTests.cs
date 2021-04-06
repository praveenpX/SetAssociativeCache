using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Sample.NWayCache.Tests
{
    /// <summary>
    /// SetAssociativeCacheTests Unit Tests
    /// </summary>
    [TestClass]
    public class SetAssociativeCacheTests
    {
        /// <summary>
        /// Gets or sets the set1 way associative cache.
        /// </summary>
        /// <value>
        /// The set1 way associative cache.
        /// </value>
        public SetAssociativeCache<int, int> Set1WayAssociativeCache { get; set; }

        /// <summary>
        /// Gets or sets the set2 way associative cache.
        /// </summary>
        /// <value>
        /// The set2 way associative cache.
        /// </value>
        public SetAssociativeCache<int, int> Set2WayAssociativeCache { get; set; }

        /// <summary>
        /// Gets or sets the one way cache capacity.
        /// </summary>
        /// <value>
        /// The one way cache capacity.
        /// </value>
        public int OneWayCacheCapacity { get; set; }

        /// <summary>
        /// Gets or sets the two way cache capacity.
        /// </summary>
        /// <value>
        /// The two way cache capacity.
        /// </value>
        public int TwoWayCacheCapacity { get; set; }

        /// <summary>
        /// Gets or sets the one way number of ways.
        /// </summary>
        /// <value>
        /// The one way number of ways.
        /// </value>
        public int OneWayNumberOfWays { get; set; }

        /// <summary>
        /// Gets or sets the two way number of ways.
        /// </summary>
        /// <value>
        /// The two way number of ways.
        /// </value>
        public int TwoWayNumberOfWays { get; set; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            OneWayCacheCapacity = 8;
            TwoWayCacheCapacity = 3;

            OneWayNumberOfWays = 1;
            TwoWayNumberOfWays = 2;

            Set1WayAssociativeCache = new SetAssociativeCache<int, int>(OneWayNumberOfWays, OneWayCacheCapacity);

            Set2WayAssociativeCache = new SetAssociativeCache<int, int>(TwoWayNumberOfWays, TwoWayCacheCapacity);
        }

        [TestMethod]
        public void Test1WayAssociation()
        {
            //add items to cache
            for (int i = 1; i <= 10; i++)
            {
                Set1WayAssociativeCache.Add(i, i);
            }

            //read items from cache and print
            for (int i = 1; i <= 10; i++)
            {
                int j;
                Set1WayAssociativeCache.TryGetValue(i, out j);
                Console.WriteLine(i + " " + j);
            }

            int result;

            Set1WayAssociativeCache.TryGetValue(1, out result);
            Assert.AreEqual(1, 0);

            Set1WayAssociativeCache.TryGetValue(2, out result);
            Assert.AreEqual(2, 0);

            Set1WayAssociativeCache.TryGetValue(3, out result);
            Assert.AreEqual(3, 3);

            Set1WayAssociativeCache.TryGetValue(4, out result);
            Assert.AreEqual(4, 4);

            Set1WayAssociativeCache.TryGetValue(5, out result);
            Assert.AreEqual(5, 5);

            Set1WayAssociativeCache.TryGetValue(6, out result);
            Assert.AreEqual(6, 6);

            Set1WayAssociativeCache.TryGetValue(7, out result);
            Assert.AreEqual(7, 7);

            Set1WayAssociativeCache.TryGetValue(8, out result);
            Assert.AreEqual(8, 8);

            Set1WayAssociativeCache.TryGetValue(9, out result);
            Assert.AreEqual(9, 9);

            Set1WayAssociativeCache.TryGetValue(10, out result);
            Assert.AreEqual(10, 10);

        }

        [TestMethod]
        public void CachBlockSetCountShouldBeEqualToCacheCapacityForOneWay()
        {
            //add items to cache
            for (int i = 1; i <= 10; i++)
            {
                Set1WayAssociativeCache.Add(i, i);
            }

            int collectioncount = Set1WayAssociativeCache.Count();

            Assert.AreEqual(OneWayCacheCapacity, collectioncount);
        }

        [TestMethod]
        public void Test2WayAssociation()
        {
            //add items to cache
            for (int i = 1; i <= 10; i++)
            {
                Set2WayAssociativeCache.Add(i, i);
            }

            //read items from cache and print
            for (int i = 1; i <= 10; i++)
            {
                int j;
                Set2WayAssociativeCache.TryGetValue(i, out j);
                Console.WriteLine(i + " " + j);
            }

            int result;
            Set2WayAssociativeCache.TryGetValue(1, out result);
            Assert.AreEqual(1, 0);

            Set2WayAssociativeCache.TryGetValue(2, out result);
            Assert.AreEqual(2, 0);

            Set2WayAssociativeCache.TryGetValue(3, out result);
            Assert.AreEqual(3, 0);

            Set2WayAssociativeCache.TryGetValue(4, out result);
            Assert.AreEqual(4, 0);

            Set2WayAssociativeCache.TryGetValue(4, out result);
            Assert.AreEqual(4, 0);

            Set2WayAssociativeCache.TryGetValue(5, out result);
            Assert.AreEqual(5, 5);

            Set2WayAssociativeCache.TryGetValue(6, out result);
            Assert.AreEqual(6, 6);

            Set2WayAssociativeCache.TryGetValue(7, out result);
            Assert.AreEqual(7, 7);

            Set2WayAssociativeCache.TryGetValue(8, out result);
            Assert.AreEqual(8, 8);

            Set2WayAssociativeCache.TryGetValue(9, out result);
            Assert.AreEqual(9, 9);

            Set2WayAssociativeCache.TryGetValue(10, out result);
            Assert.AreEqual(10, 10);
        }

        [TestMethod]
        public void CachBlockSetCountShouldBeEqualToCacheCapacityForTwoWay()
        {
            //add items to cache
            for (int i = 1; i <= 10; i++)
            {
                Set2WayAssociativeCache.Add(i, i);
            }

            int collectioncount = Set2WayAssociativeCache.Count();

            Assert.AreEqual(TwoWayCacheCapacity, collectioncount);
        }

        [TestCleanup]
        public void Dispose()
        {
            Set2WayAssociativeCache.Clear();

            Set1WayAssociativeCache.Clear();
        }
    }
}
