namespace Sample.NWayCache
{
    /// <summary>
    /// ICacheBlock interface
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface ICacheBlock<TKey, TValue>
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        TKey Key { get; }
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        TValue Value { get; }
    }
}