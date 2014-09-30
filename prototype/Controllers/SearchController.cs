using NPlay.Corp.Search.Contracts;
using NPlay.Corp.Search.Core;
using PlainElastic.Net;
using PlainElastic.Net.Queries;
using PlainElastic.Net.Serialization;
using PlainElastic.Net.Utils;
using System;
using System.Configuration;
using System.Net;
using System.Web.Mvc;


namespace Prototype.Controllers
{
    /// <summary>
    /// Contains methods used to interact with the Elastic Search service via Plain Elastic.NET.
    /// </summary>
    public class SearchController : Controller
    {
        #region Private Members

        private ICacheClient _cacheClient;

        #endregion Private Members

        #region Properties

        public ICacheClient CacheClient
        {
            get
            {
                if (_cacheClient == null)
                {
                    _cacheClient = new MongoCacheClient(ConfigurationManager.AppSettings["MongoConnectionString"], ConfigurationManager.AppSettings["MongoDatabaseName"]);
                }
                return _cacheClient;
            }
        }

        /// <summary>
        /// The URL to the Elastic Search server.
        /// </summary>
        public string ElasticSearchHost
        {
            get
            {
                return ConfigurationManager.AppSettings["ElasticSearchHost"];
            }
        }

        /// <summary>
        /// The port to connect to Elastic Search (9200, typically)
        /// </summary>
        public int ElasticSearchPort
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["ElasticSearchPort"]);
            }
        }

        /// <summary>
        /// If provided, the login credential used to authenticate
        /// </summary>
        public string ElasticSearchLogin
        {
            get
            {
                return ConfigurationManager.AppSettings["ElasticSearchLogin"];
            }
        }

        /// <summary>
        /// If provided, the password credential used to authenticate
        /// </summary>
        public string ElasticSearchPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["ElasticSearchPassword"];
            }
        }

        #endregion Properties

        #region Controller Actions

        /// <summary>
        /// Uses the provided query string to perform a fuzzy search against Elastic Search.
        /// </summary>
        /// <param name="query">The HTML encoded query string</param>
        /// <returns>JsonResult</returns>
        [HttpGet]
        [Route("listings/search/{query}")]
        public JsonResult Search(string query)
        {
            // Format the query to make Elastic happy
            query = query.Replace("+", " AND ");

            try
            {
                string elasticQuery = new QueryBuilder<string>()
                    .Query(q => q
                        .Bool(b => b
                           .Must(m => m
                               .FuzzyLikeThis(flt => flt
                                   .Fields("in_public_remarks", "full_address")
                                   .LikeText(query)
                                   .MaxQueryTerms(12)
                               )
                           )
                        )
                    )
                    .Size(100)
                    .Build();

                // Create a new Elastic Search connection and Json serializer, used to serialize the query
                var connection = new ElasticConnection(ElasticSearchHost, ElasticSearchPort);
                var serializer = new JsonNetSerializer();

                // If we have credentials, supply them.
                if (!string.IsNullOrWhiteSpace(ElasticSearchLogin) && !string.IsNullOrWhiteSpace(ElasticSearchPassword))
                {
                    connection.Credentials = new NetworkCredential(ElasticSearchLogin, ElasticSearchPassword);
                }

                // Post the search and obtain the JSON Operation Result
                string result = connection.Post(Commands.Search("listings", "listing"), elasticQuery);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(exc.Message, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Returns a collection of image URLs for a given listing id.
        /// </summary>
        /// <param name="listingId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("listings/photos/{listingId:int}")]
        public JsonResult Photos(int listingId)
        {
            try
            {
                string elasticQuery = new QueryBuilder<string>()
                    .Query(q => q
                        .Bool(b => b
                           .Must(m => m
                               .Term(t => t
                                   .Field("listing_photo.listing_id").Value(listingId.AsString())
                                )
                           )
                        )
                    )
                    .Size(100)
                    .Build();

                // Create a new Elastic Search connection and Json serializer, used to serialize the query
                var connection = new ElasticConnection(ElasticSearchHost, ElasticSearchPort);
                var serializer = new JsonNetSerializer();

                // If we have credentials, supply them.
                if (!string.IsNullOrWhiteSpace(ElasticSearchLogin) && !string.IsNullOrWhiteSpace(ElasticSearchPassword))
                {
                    connection.Credentials = new NetworkCredential(ElasticSearchLogin, ElasticSearchPassword);
                }

                // Post the search and obtain the JSON Operation Result
                string result = connection.Post(Commands.Search("listing_photos", "listing_photo"), elasticQuery);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(exc.Message, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Returns a single listing using an Elastic Search term query.
        /// </summary>
        /// <param name="listingId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("listings/listing/{listingId:int}")]
        public JsonResult Listing(int listingId)
        {
            try
            {
                string elasticQuery = new QueryBuilder<string>()
                    .Query(q => q
                        .Bool(b => b
                           .Must(m => m
                               .Term(t => t
                                   .Field("listing.listing_id").Value(listingId.AsString())
                                )
                           )
                        )
                    )
                    .Size(1)
                    .Build();

                // Create a new Elastic Search connection and Json serializer, used to serialize the query
                var connection = new ElasticConnection(ElasticSearchHost, ElasticSearchPort);
                var serializer = new JsonNetSerializer();

                // If we have credentials, supply them.
                if (!string.IsNullOrWhiteSpace(ElasticSearchLogin) && !string.IsNullOrWhiteSpace(ElasticSearchPassword))
                {
                    connection.Credentials = new NetworkCredential(ElasticSearchLogin, ElasticSearchPassword);
                }

                // Post the search and obtain the JSON Operation Result
                string result = connection.Post(Commands.Search("listings", "listing"), elasticQuery);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(exc.Message, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Controller Actions
    }
}