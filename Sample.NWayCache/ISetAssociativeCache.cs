namespace Sample.NWayCache
{
    /// <summary>
    /// ISetAssociativeCache interface
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface ISetAssociativeCache<TKey, TValue>
    {
        /// <summary>
        /// Gets the <see cref="TValue"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="TValue"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        TValue this[TKey key] { get; }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void Add(TKey key, TValue value);
        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();
        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsKey(TKey key);
        /// <summary>
        /// Counts this instance.
        /// </summary>
        /// <returns></returns>
        int Count();
        /// <summary>
        /// Itemses the count.
        /// </summary>
        /// <returns></returns>
        int ItemsCount();
        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        bool TryGetValue(TKey key, out TValue value);

        /// <summary>
        /// Always Removes the Item based on the LRU Cache Policy
        /// </summary>
        void Remove();
    }
}