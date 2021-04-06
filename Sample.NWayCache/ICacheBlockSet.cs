using System.Collections.Generic;

namespace Sample.NWayCache
{
    /// <summary>
    /// ICacheBlockSet interface
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface ICacheBlockSet<TKey, TValue>
    {
        /// <summary>
        /// Gets or sets the blocks.
        /// </summary>
        /// <value>
        /// The blocks.
        /// </value>
        Dictionary<TKey, CacheBlock<TKey, TValue>> Blocks { get; set; }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        CacheBlock<TKey, TValue> Add(TKey key, TValue value);

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();
        IList<CacheBlock<TKey, TValue>> GetBlocks();

        /// <summary>
        /// Determines whether this instance is full.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is full; otherwise, <c>false</c>.
        /// </returns>
        bool IsFull();

        /// <summary>
        /// Removes the block.
        /// </summary>
        /// <param name="block">The block.</param>
        void RemoveBlock(CacheBlock<TKey, TValue> block);

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        bool TryGetValue(TKey key, out TValue value);
    }
}