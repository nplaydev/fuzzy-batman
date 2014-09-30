namespace NPlay.Corp.Search.Contracts
{
    /// <summary>
    /// Defines an implementation guide for a cache client.
    /// </summary>
    public interface ICacheClient
    {
        /// <summary>
        /// Adds the specified object of type T to the cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        string Put<T>(string collectionName, T obj) where T : class, ICacheable, new();

        /// <summary>
        /// Fetchs the specified object of type T from the cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        T Get<T>(string collectionName, string Id) where T : class, ICacheable, new();
    }
}
