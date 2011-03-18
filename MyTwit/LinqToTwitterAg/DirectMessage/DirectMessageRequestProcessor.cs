﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections;
using System.Globalization;
using System.Xml.Linq;

namespace LinqToTwitter
{
    /// <summary>
    /// processes Twitter Direct Messages
    /// </summary>
    public class DirectMessageRequestProcessor<T> : IRequestProcessor<T>
    {
        #region IRequestProcessor Members

        /// <summary>
        /// base url for request
        /// </summary>
        public virtual string BaseUrl { get; set; }

        /// <summary>
        /// Type of Direct Message
        /// </summary>
        private DirectMessageType Type { get; set; }

        /// <summary>
        /// since this message ID
        /// </summary>
        private ulong SinceID { get; set; }

        /// <summary>
        /// max ID to return
        /// </summary>
        private ulong MaxID { get; set; }

        /// <summary>
        /// page number to return
        /// </summary>
        private int Page { get; set; }

        /// <summary>
        /// number of items to return (works for SentBy and SentTo
        /// </summary>
        private int Count { get; set; }

        /// <summary>
        /// ID of DM
        /// </summary>
        private ulong ID { get; set; }
        
        /// <summary>
        /// extracts parameters from lambda
        /// </summary>
        /// <param name="lambdaExpression">lambda expression with where clause</param>
        /// <returns>dictionary of parameter name/value pairs</returns>
        public virtual Dictionary<string, string> GetParameters(LambdaExpression lambdaExpression)
        {
            var paramFinder =
               new ParameterFinder<DirectMessage>(
                   lambdaExpression.Body,
                   new List<string> { 
                       "Type",
                       "SinceID",
                       "MaxID",
                       "Page",
                       "Count",
                       "ID"
                   });

            var parameters = paramFinder.Parameters;

            return parameters;
        }

        /// <summary>
        /// builds url based on input parameters
        /// </summary>
        /// <param name="parameters">criteria for url segments and parameters</param>
        /// <returns>URL conforming to Twitter API</returns>
        public virtual string BuildURL(Dictionary<string, string> parameters)
        {
            string url = null;

            if (parameters == null || !parameters.ContainsKey("Type"))
            {
                throw new ArgumentException("You must set Type.", "Type");
            }

            Type = RequestProcessorHelper.ParseQueryEnumType<DirectMessageType>(parameters["Type"]);

            switch (Type)
            {
                case DirectMessageType.SentBy:
                    url = BuildSentByUrl(parameters);
                    break;
                case DirectMessageType.SentTo:
                    url = BuildSentToUrl(parameters);
                    break;
                case DirectMessageType.Show:
                    url = BuildShowUrl(parameters);
                    break;
                default:
                    throw new InvalidOperationException("The default case of BuildUrl should never execute because a Type must be specified.");
            }

            return url;
        }

        private string BuildShowUrl(Dictionary<string, string> parameters)
        {
            if (!parameters.ContainsKey("ID"))
            {
                throw new ArgumentNullException("ID", "ID is required.");
            }

            var url = BaseUrl + "direct_messages/show.xml";

            url = BuildUrlHelper.TransformIDUrl(parameters, url);
            ID = ulong.Parse(parameters["ID"]);

            return url;
        }

        /// <summary>
        /// builds an url for getting a list of direct message sent to a user
        /// </summary>
        /// <param name="parameters">parameters to add</param>
        /// <returns>new url with parameters</returns>
        private string BuildSentToUrl(Dictionary<string, string> parameters)
        {
            var url = BaseUrl + "direct_messages.xml";

            if (parameters != null)
            {
                url = BuildSentUrlParameters(parameters, url);
            }

            return url;
        }

        /// <summary>
        /// builds an url for getting a list of direct message sent by a user
        /// </summary>
        /// <param name="parameters">parameters to add</param>
        /// <returns>new url with parameters</returns>
        private string BuildSentByUrl(Dictionary<string, string> parameters)
        {
            var url = BaseUrl + "direct_messages/sent.xml";

            if (parameters != null)
            {
                url = BuildSentUrlParameters(parameters, url);
            }

            return url;
        }

        /// <summary>
        /// common code for building parameter list for both sent by and sent to urls
        /// </summary>
        /// <param name="parameters">parameters to add</param>
        /// <param name="url">url to start with</param>
        /// <returns>new url with parameters</returns>
        private string BuildSentUrlParameters(Dictionary<string, string> parameters, string url)
        {
            if (parameters == null)
            {
                return url;
            }

            var urlParams = new List<string>();

            if (parameters.ContainsKey("SinceID"))
            {
                SinceID = ulong.Parse(parameters["SinceID"]);
                urlParams.Add("since_id=" + parameters["SinceID"]);
            }

            if (parameters.ContainsKey("MaxID"))
            {
                MaxID = ulong.Parse(parameters["MaxID"]);
                urlParams.Add("max_id=" + parameters["MaxID"]);
            }

            if (parameters.ContainsKey("Page"))
            {
                Page = int.Parse(parameters["Page"]);
                urlParams.Add("page=" + parameters["Page"]);
            }

            if (parameters.ContainsKey("Count"))
            {
                Count = int.Parse(parameters["Count"]);
                urlParams.Add("count=" + parameters["Count"]);
            }

            if (urlParams.Count > 0)
            {
                url += "?" + string.Join("&", urlParams.ToArray());
            }

            return url;
        }

        /// <summary>
        /// transforms XML into IQueryable of DirectMessage
        /// </summary>
        /// <param name="responseXml">xml with Twitter response</param>
        /// <returns>List of DirectMessage</returns>
        public virtual List<T> ProcessResults(string responseXml)
        {
            XElement twitterResponse = XElement.Parse(responseXml);
            var responseItems = twitterResponse.Elements("direct_message").ToList();

            // if we get only a single response back,
            // make sure we get it
            if (twitterResponse.Name == "direct_message")
            {
                responseItems.Add(twitterResponse);
            }

            var dmList =
                from dm in responseItems
                let sender =
                    dm.Element("sender")
                let recipient =
                    dm.Element("recipient")
                let createdAtDate =
                    DateTime.ParseExact(
                        dm.Element("created_at").Value,
                        "ddd MMM dd HH:mm:ss %zzzz yyyy",
                        CultureInfo.InvariantCulture)
                select new DirectMessage
                {
                    Type = Type,
                    SinceID = SinceID,
                    MaxID = MaxID,
                    Page = Page,
                    Count = Count,
                    ID = ulong.Parse(dm.Element("id").Value),
                    SenderID = ulong.Parse(dm.Element("sender_id").Value),
                    Text = dm.Element("text").Value,
                    RecipientID = ulong.Parse(dm.Element("recipient_id").Value),
                    CreatedAt = createdAtDate,
                    SenderScreenName = dm.Element("sender_screen_name").Value,
                    RecipientScreenName = dm.Element("recipient_screen_name").Value,
                    Sender = User.CreateUser(sender),
                    Recipient = User.CreateUser(recipient)
                };

            return dmList.OfType<T>().ToList();
        }

        #endregion
    }
}
