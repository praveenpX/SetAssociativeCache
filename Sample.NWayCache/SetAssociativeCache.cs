using System.Collections.Generic;
using System.Linq;

namespace Sample.NWayCache
{
    /// <summary>
    /// Set Associative Class Implementation using LRU Cache Policy
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class SetAssociativeCache<TKey, TValue> : ISetAssociativeCache<TKey, TValue>
    {
        /// <summary>
        /// Gets or sets the number of ways.
        /// </summary>
        /// <value>
        /// The number of ways.
        /// </value>
        private int NumberOfWays { get; set; }

        /// <summary>
        /// Gets or sets the cache capacity.
        /// </summary>
        /// <value>
        /// The cache capacity.
        /// </value>
        private int CacheCapacity { get; set; }

        /// <summary>
        /// Gets or sets the block sets lookup.
        /// </summary>
        /// <value>
        /// The block sets lookup.
        /// </value>
        private Dictionary<TKey, CacheBlockSet<TKey, TValue>> BlockSetsLookup { get; set; }

        /// <summary>
        /// The lru list
        /// </summary>
        public readonly LinkedList<CacheBlock<TKey, TValue>> lruList = new LinkedList<CacheBlock<TKey, TValue>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SetAssociativeCache{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="numberOfWays">The number of ways.</param>
        /// <param name="cacheCapacity">The cache capacity.</param>
        public SetAssociativeCache(int numberOfWays, int cacheCapacity)
        {
            this.NumberOfWays = numberOfWays;
            this.CacheCapacity = cacheCapacity;

            this.BlockSetsLookup = new Dictionary<TKey, CacheBlockSet<TKey, TValue>>(cacheCapacity);

            this.lruList = new LinkedList<CacheBlock<TKey, TValue>>();
        }

        /// <summary>
        /// Adds a specified key-value pair to the block set
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(TKey key, TValue value)
        {
            lock (BlockSetsLookup)
            {
                //cache is full
                if (this.BlockSetsLookup.Count >= this.CacheCapacity)
                {
                    if (BlockSetsLookup.Values.Any(a => a.Blocks.Count != NumberOfWays)) //there is still space left
                    {
                        AddBlockSetToCache(key, value, FindInsertableBlockSet());
                    }
                    else
                    {
                        InsertIntoFullCache(key, value);
                    }
                }
                else
                {
                    AddBlockSetToCache(key, value, FindInsertableBlockSet());
                }
            }
        }

        /// <summary>
        /// Finds the insertable block set.
        /// </summary>
        /// <returns></returns>
        private CacheBlockSet<TKey, TValue> FindInsertableBlockSet()
        {
            foreach (var currentBlockSetNode in BlockSetsLookup.Values)
            {
                if (currentBlockSetNode != null)
                {
                    if (!currentBlockSetNode.IsFull())
                    {
                        return currentBlockSetNode;
                    }
                }
            }
            return new CacheBlockSet<TKey, TValue>(NumberOfWays);
        }

        /// <summary>
        /// Adds the block set to cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="blockSet">The block set.</param>
        private void AddBlockSetToCache(TKey key, TValue value, CacheBlockSet<TKey, TValue> blockSet)
        {
            CacheBlock<TKey, TValue> cacheItem = null;

            if (blockSet != null)
            {
                cacheItem = blockSet.Add(key, value);
            }

            var blocksets = BlockSetsLookup.Values;

            if (!blocksets.Any(a => a.Blocks.Any(b => b.Key.Equals(key))))
            {
                BlockSetsLookup.Add(key, blockSet);
            }

            this.lruList.AddLast(cacheItem);
        }

        /// <summary>
        /// Inserts key-value into full cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        private void InsertIntoFullCache(TKey key, TValue value)
        {
            if (BlockSetsLookup.ContainsKey(key))
            {
                CacheBlockSet<TKey, TValue> blockSet = BlockSetsLookup[key];

                if (blockSet != null)
                {
                    blockSet.Add(key, value);
                }
            }
            else
            {
                CacheBlockSet<TKey, TValue> insertableBlockSet = discardBlock(BlockSetsLookup);
                AddBlockSetToCache(key, value, insertableBlockSet);
            }
        }

        /// <summary>
        /// Always Removes the Item based on the LRU Cache Policy
        /// </summary>
        public void Remove()
        {
            // Remove from LRUPriority
            LinkedListNode<CacheBlock<TKey, TValue>> node = this.lruList.First;

            this.lruList.RemoveFirst();

            var removedblock = node.Value;

            foreach (var set in BlockSetsLookup.Values)
            {
                if (set.Blocks.Any(a => a.Value.Key.Equals(removedblock.Key)))
                {
                    foreach (var blocks in set.Blocks)
                    {
                        if (blocks.Key.Equals(removedblock.Key))
                        {
                            set.RemoveBlock(removedblock);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Discards the block based on the LRU algorithm
        /// </summary>
        /// <param name="blockSetsLookup">The block sets lookup.</param>
        /// <returns></returns>
        private CacheBlockSet<TKey, TValue> discardBlock(Dictionary<TKey, CacheBlockSet<TKey, TValue>> blockSetsLookup)
        {
            lock (BlockSetsLookup)
            {
                // Remove from LRUPriority
                LinkedListNode<CacheBlock<TKey, TValue>> node = this.lruList.First;

                this.lruList.RemoveFirst();

                var removedblock = node.Value;

                foreach (var set in blockSetsLookup.Values)
                {
                    if (set.Blocks.Any(a => a.Value.Key.Equals(removedblock.Key)))
                    {
                        foreach (var blocks in set.Blocks)
                        {
                            if (blocks.Key.Equals(removedblock.Key))
                            {
                                set.RemoveBlock(removedblock);
                                return set;
                            }
                        }
                    }
                }

                return new CacheBlockSet<TKey, TValue>(NumberOfWays);
            }
        }

        /// <summary>
        /// Returns the value and also a flag indicating if given key is present or not and a default value if not key not found
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);

            lock (this.BlockSetsLookup)
            {
                CacheBlockSet<TKey, TValue> node = null;

                CacheBlock<TKey, TValue> mblock = null;

                foreach (var set in BlockSetsLookup.Values)
                {
                    foreach (var block in set.Blocks)
                    {
                        if (block.Key.Equals(key))
                        {
                            node = set;

                            mblock = block.Value;
                            break;
                        }
                    }
                }

                if (mblock != null)
                {
                    TValue blockvalue = default(TValue);

                    if (node.TryGetValue(key, out blockvalue))
                    {
                        node.TryGetValue(key, out blockvalue);

                        value = blockvalue;

                        var lrublock = this.lruList.Find(mblock);

                        this.lruList.Remove(lrublock);
                        this.lruList.AddLast(lrublock);
                        return true;
                    }
                }

                value = default(TValue);
                return false;
            }
        }

        /// <summary>
        /// Removes all the entries in the collection
        /// </summary>
        public void Clear()
        {
            this.BlockSetsLookup.Clear();
            this.lruList.Clear();
        }

        /// <summary>
        /// Returns the total number of cache block sets
        /// </summary>
        public int Count()
        {
            return this.BlockSetsLookup.Count();
        }

        /// <summary>
        /// Returns the total number of cache blocks
        /// </summary>
        public int ItemsCount()
        {
            int count = 0;

            foreach(var set in BlockSetsLookup.Values)
            {
                count = count + set.Blocks.Count;
            }

            return count;
        }

        /// <summary>
        /// returns a flag indicating if the key is present or not
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return this.BlockSetsLookup.ContainsKey(key);
        }

        /// <summary>
        /// Gets the <see cref="TValue"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="TValue"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException"></exception>
        public TValue this[TKey key]
        {
            get
            {
                if (!ContainsKey(key))
                {
                    throw new KeyNotFoundException(string.Format("key doesn't exist in the set {0}", key));
                }

                TValue value = default(TValue);
                TryGetValue(key, out value);
                return value;
            }
        }
    }
}
