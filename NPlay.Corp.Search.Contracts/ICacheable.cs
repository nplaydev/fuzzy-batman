
namespace NPlay.Corp.Search.Contracts
{
    /// <summary>
    /// Identifies an object as cacheable for a given cache client.
    /// </summary>
    public interface ICacheable
    {
        /// <summary>
        /// The Id or cacheKey to use when fetching and inserting an object.
        /// </summary>
        string Id { get; set; }
    }
}
