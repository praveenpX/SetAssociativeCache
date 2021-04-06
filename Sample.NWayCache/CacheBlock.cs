namespace Sample.NWayCache
{
    /// <summary>
    /// Cache Block Data structure to hold data (key-value pair)
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class CacheBlock<TKey, TValue> : ICacheBlock<TKey, TValue>
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public TKey Key { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public TValue Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheBlock{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public CacheBlock(TKey key, TValue value)
        {
            Key = key;
            Value = value;
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

            CacheBlock<TKey, TValue> other = (CacheBlock<TKey, TValue>)obj;
            return this.Key.Equals(other.Key) && this.Value.Equals(other.Value);
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

            hash = (hash * 7) + Key.GetHashCode();
            hash = (hash * 7) + Value.GetHashCode();

            return hash;
        }
        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("CacheBlock {0},{1}", Key, Value);
        }
    }
}
