﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
#if SILVERLIGHT
using System.Windows;
#endif

namespace LinqToTwitter
{
    public class AnonymousAuthorizer : OAuthAuthorizer, ITwitterAuthorizer
    {
        public void Authorize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the specified request URI.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public new WebRequest Get(string url)
        {
#if SILVERLIGHT
            url = ProxyUrl + url;
#endif

            Uri requestUri = new Uri(url);
            
            var req = WebRequest.Create(requestUri);

            this.InitializeRequest(req);
            return req;
        }

        /// <summary>
        /// Posts the specified request URI.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <returns></returns>
        public new HttpWebRequest Post(string url)
        {
#if SILVERLIGHT
            url = ProxyUrl + url;
#endif

            Uri requestUri = new Uri(url);
            var req = WebRequest.Create(requestUri) as HttpWebRequest;
            this.InitializeRequest(req);
            req.Method = HttpMethod.POST.ToString();
            return req;
        }

        /// <summary>
        /// Posts the specified request URI.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public new HttpWebResponse Post(string url, Dictionary<string, string> args)
        {
#if SILVERLIGHT
            url = ProxyUrl + url;
#endif

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            Uri requestUri = new Uri(url);

            string queryString = Utilities.BuildQueryString(args);
            byte[] queryStringBytes = Encoding.UTF8.GetBytes(queryString);

            var req = WebRequest.Create(requestUri) as HttpWebRequest;

            this.InitializeRequest(req);

            req.Method = HttpMethod.POST.ToString();
            //req.ServicePoint.Expect100Continue = false;
            req.Headers[HttpRequestHeader.Expect] = null;
            req.ContentType = "x-www-form-urlencoded";
            req.ContentLength = queryStringBytes.Length;

            var resetEvent = new ManualResetEvent(initialState: false);

            req.BeginGetRequestStream(
                new AsyncCallback(
                    ar =>
                    {
                        using (var requestStream = req.EndGetRequestStream(ar))
                        {
                            requestStream.Write(queryStringBytes, 0, queryStringBytes.Length);
                        }
                        resetEvent.Set();
                    }), null);

            resetEvent.WaitOne();
            resetEvent.Reset();

            HttpWebResponse res = null;

            req.BeginGetResponse(
                new AsyncCallback(
                    ar =>
                    {
                        res = req.EndGetResponse(ar) as HttpWebResponse;
                        resetEvent.Set();
                    }), null);

            resetEvent.WaitOne();

            return res;
        }
    }
}
