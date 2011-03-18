﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.Globalization;

namespace LinqToTwitter
{
    /// <summary>
    /// processes Twitter Status requests
    /// </summary>
    public class RawRequestProcessor<T> : IRequestProcessor<T>
    {
        /// <summary>
        /// base url for request
        /// </summary>
        public virtual string BaseUrl { get; set; }
        
        /// <summary>
        /// Actual query string sent to twitter
        /// </summary>
        public string QueryString { get; set; }

        /// <summary>
        /// TweetID
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// extracts parameters from lambda
        /// </summary>
        /// <param name="lambdaExpression">lambda expression with where clause</param>
        /// <returns>dictionary of parameter name/value pairs</returns>
        public virtual Dictionary<string, string> GetParameters(LambdaExpression lambdaExpression)
        {
            return new ParameterFinder<Raw>(
               lambdaExpression.Body,
               new List<string> { 
                   "QueryString"
               })
               .Parameters;
        }

        /// <summary>
        /// builds url based on input parameters
        /// </summary>
        /// <param name="parameters">criteria for url segments and parameters</param>
        /// <returns>URL conforming to Twitter API</returns>
        public virtual string BuildURL(Dictionary<string, string> parameters)
        {
            if (parameters.ContainsKey("QueryString"))
            {
                QueryString = parameters["QueryString"].Trim();
            }
            else
            {
                throw new ArgumentNullException("QueryString", "QueryString parameter is required.");
            }

            if (QueryString == string.Empty)
            {
                throw new ArgumentException("Blank QueryString isn't valid.", "QueryString");
            }

            string url = BaseUrl.TrimEnd('/') + "/" + QueryString.TrimStart('/');

            return url;
        }

        /// <summary>
        /// transforms XML into IQueryable of Status
        /// </summary>
        /// <param name="responseXml">xml with Twitter response</param>
        /// <returns>List of Status</returns>
        public virtual List<T> ProcessResults(string responseXml)
        {
            var rawList = new List<Raw>
            {
                new Raw
                {
                    QueryString = QueryString,
                    Result = responseXml
                }
            };

            return rawList.OfType<T>().ToList();
        }
    }
}
