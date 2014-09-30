using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NPlay.Corp.Search.Contracts;

namespace NPlay.Corp.Search.Core
{
    /// <summary>
    /// Concrete implementation of an ICacheClient using MongoDB as the cache.
    /// </summary>
    public class MongoCacheClient : ICacheClient
    {
        #region Private Members

        private MongoClient _client;

        private MongoServer _server;

        private MongoDatabase _database; 

        #endregion Private Members

        #region Constructor

        /// <summary>
        /// Instantiates a new CacheClient with a give connection string and database name.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        public MongoCacheClient(string connectionString, string databaseName)
        {
            try
            {
                _client = new MongoClient(connectionString);
                _server = _client.GetServer();
                _database = _server.GetDatabase(databaseName);
            }
            catch (System.Exception)
            {
                throw;
            }

        } 

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Inserts the specified object of type T (implementing ICacheable) into the cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Put<T>(string collectionName, T obj) where T : class, ICacheable, new()
        {
            try
            {
                var collection = _database.GetCollection<T>(collectionName);
                collection.Insert(obj);
                return obj.Id;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the object with the specified Id from the specified collection in the cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T Get<T>(string collectionName, string Id) where T : class, ICacheable, new()
        {
            try
            {
                var collection = _database.GetCollection<T>(collectionName);
                var query = Query<T>.EQ(e => e.Id, Id);
                return collection.FindOne(query);
            }
            catch (System.Exception)
            {
                throw;
            }
        } 

        #endregion Public Methods
    }
}
