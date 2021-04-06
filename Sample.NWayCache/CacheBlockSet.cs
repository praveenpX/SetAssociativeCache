using System.Collections.Generic;
using System.Linq;

namespace Sample.NWayCache
{
    /// <summary>
    /// CacheBlockSet to hold groups of cache blocks (key-value pairs) using LRU Cache Policy
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class CacheBlockSet<TKey, TValue> : ICacheBlockSet<TKey, TValue>
    {
        /// <summary>
        /// Gets or sets the blocks.
        /// </summary>
        /// <value>
        /// The blocks.
        /// </value>
        public Dictionary<TKey, CacheBlock<TKey, TValue>> Blocks { get; set; }

        /// <summary>
        /// The lru list
        /// </summary>
        public readonly LinkedList<CacheBlock<TKey, TValue>> lruList = new LinkedList<CacheBlock<TKey, TValue>>();

        /// <summary>
        /// Gets or sets the block set capacity.
        /// </summary>
        /// <value>
        /// The block set capacity.
        /// </value>
        private int BlockSetCapacity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheBlockSet{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="blockSetCapacity">The block set capacity.</param>
        public CacheBlockSet(int blockSetCapacity)
        {
            this.BlockSetCapacity = blockSetCapacity;

            this.Blocks = new Dictionary<TKey, CacheBlock<TKey, TValue>>(blockSetCapacity);

            this.lruList = new LinkedList<CacheBlock<TKey, TValue>>();
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public CacheBlock<TKey, TValue> Add(TKey key, TValue value)
        {
            lock (this.Blocks)
            {
                if (this.Blocks.Count >= this.BlockSetCapacity)
                {
                    this.RemoveFirst();
                }

                if (this.Blocks.ContainsKey(key))
                {
                    var r = this.Blocks[key];

                    this.lruList.Remove(r);
                    //this.lruList.AddLast(r);
                    this.Blocks.Remove(key);
                }

                CacheBlock<TKey, TValue> cacheItem = new CacheBlock<TKey, TValue>(key, value);
                LinkedListNode<CacheBlock<TKey, TValue>> node = new LinkedListNode<CacheBlock<TKey, TValue>>(cacheItem);
                this.lruList.AddLast(node);
                this.Blocks.Add(key, cacheItem);

                return cacheItem;
            }
        }

        /// <summary>
        /// Removes the block.
        /// </summary>
        /// <param name="block">The block.</param>
        public void RemoveBlock(CacheBlock<TKey, TValue> block)
        {
            lock (Blocks)
            {
                Blocks.Remove(block.Key);
            }
        }

        /// <summary>
        /// Removes the first block base on LRU algorithm
        /// </summary>
        private void RemoveFirst()
        {
            lock (Blocks)
            {
                // Remove from LRUPriority
                LinkedListNode<CacheBlock<TKey, TValue>> node = this.lruList.First;
                this.lruList.RemoveFirst();

                // Remove from cache
                this.Blocks.Remove(node.Value.Key);
            }
        }

        /// <summary>
        /// Gets the blocks.
        /// </summary>
        /// <returns></returns>
        public IList<CacheBlock<TKey, TValue>> GetBlocks()
        {
            return Blocks.Values.Select(s => s).ToList();
        }

        /// <summary>
        /// return true if the blockset is full
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            return Blocks.Count == BlockSetCapacity;
        }

        /// <summary>
        /// Returns the value and also a flag indicating if given key is present or not and a default value if not key not found
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (this.Blocks)
            {
                CacheBlock<TKey, TValue> cacheItem;

                if (this.Blocks.TryGetValue(key, out cacheItem))
                {
                    value = cacheItem.Value;

                    var node = this.lruList.Find(cacheItem);

                    this.lruList.Remove(node);
                    this.lruList.AddLast(node);

                    return true;
                }

                value = default(TValue);
                return false;
            }
        }

        /// <summary>
        /// Clears the Cache Block Set 
        /// </summary>
        public void Clear()
        {
            this.Blocks.Clear();
            this.lruList.Clear();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hash = 13;

            hash = (hash * 7) + Blocks.GetHashCode();
            hash = (hash * 7) + BlockSetCapacity.GetHashCode();

            return hash;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            CacheBlockSet<TKey, TValue> other = (CacheBlockSet<TKey, TValue>)obj;
            return this.Blocks.Equals(other.Blocks) && this.BlockSetCapacity.Equals(other.BlockSetCapacity);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("CacheBlockSet {0},{1}", Blocks, BlockSetCapacity);
        }
    }
}
