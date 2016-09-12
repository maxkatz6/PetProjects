namespace Lain.Xaml.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using AngleSharp;
    using AngleSharp.Dom;
    using AngleSharp.Dom.Html;
    using AngleSharp.Parser.Html;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public static class WebHelper
    {
        public static readonly HtmlParser HtmlParser = new HtmlParser(new Configuration().WithCss());

        private static readonly HttpClientHandler Handler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer(),
                UseCookies = true,
                AllowAutoRedirect = true
            };

        private static HttpClient httpClient = new HttpClient(Handler);

        public static Uri DomainUrl
        {
            get
            {
                return httpClient.BaseAddress;
            }
            set
            {
                if (value == null
                    || httpClient.BaseAddress == value)
                    return;
                httpClient = new HttpClient(Handler)
                    {
                        BaseAddress = value
                    };
                StorageHelper.SetSetting("DomainUrl", value.ToString());
            }
        }

        #region Get        

        public static async Task<HttpResponseMessage> GetAsync(
            string url,
            CancellationToken ct = default(CancellationToken))
        {
            Uri link;
            return Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out link) &&
                   (!link.IsAbsoluteUri || link.Scheme.Contains("http"))
                       ? await MakeRequestAsync(link, HttpMethod.Get, null, null, ct).ConfigureAwait(false)
                       : null;
        }

        public static async Task<HttpResponseMessage> GetAsync(
            string url,
            IEnumerable<KeyValuePair<string, string>> args,
            CancellationToken ct = default(CancellationToken))
        {
            Uri link;
            return Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out link) &&
                   (!link.IsAbsoluteUri || link.Scheme.Contains("http"))
                       ? await MakeRequestAsync(link, HttpMethod.Get, args, null, ct).ConfigureAwait(false)
                       : null;
        }

        public static async Task<HttpResponseMessage> GetAsync(
            string url,
            IEnumerable<KeyValuePair<string, string>> args,
            IEnumerable<KeyValuePair<string, object>> headers,
            CancellationToken ct = default(CancellationToken))
        {
            Uri link;
            return Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out link) &&
                   (!link.IsAbsoluteUri || link.Scheme.Contains("http"))
                       ? await MakeRequestAsync(link, HttpMethod.Get, args, headers, ct).ConfigureAwait(false)
                       : null;
        }


        public static async Task<HttpResponseMessage> GetAsync(Uri url, CancellationToken ct)
        {
            return await MakeRequestAsync(url, HttpMethod.Get, null, null, ct)
                             .ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> GetAsync(
            Uri url,
            IEnumerable<KeyValuePair<string, string>> args = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await MakeRequestAsync(url, HttpMethod.Get, args, null, ct).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> GetAsync(
            Uri url,
            IEnumerable<KeyValuePair<string, string>> args,
            IEnumerable<KeyValuePair<string, object>> headers = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await MakeRequestAsync(url, HttpMethod.Get, args, headers, ct).ConfigureAwait(false);
        }

        #endregion

        #region Post

        public static async Task<HttpResponseMessage> PostAsync(
            string url,
            CancellationToken ct)
        {
            Uri link;
            return Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out link) &&
                   (!link.IsAbsoluteUri || link.Scheme.Contains("http"))
                       ? await MakeRequestAsync(link, HttpMethod.Post, null, null, ct).ConfigureAwait(false)
                       : null;
        }

        public static async Task<HttpResponseMessage> PostAsync(
            string url,
            IEnumerable<KeyValuePair<string, string>> args = null,
            CancellationToken ct = default(CancellationToken))
        {
            Uri link;
            return Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out link) &&
                   (!link.IsAbsoluteUri || link.Scheme.Contains("http"))
                       ? await MakeRequestAsync(link, HttpMethod.Post, args, null, ct).ConfigureAwait(false)
                       : null;
        }

        public static async Task<HttpResponseMessage> PostAsync(
            string url,
            IEnumerable<KeyValuePair<string, string>> args,
            IEnumerable<KeyValuePair<string, object>> headers,
            CancellationToken ct = default(CancellationToken))
        {
            Uri link;
            return Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out link) &&
                   (!link.IsAbsoluteUri || link.Scheme.Contains("http"))
                       ? await MakeRequestAsync(link, HttpMethod.Post, args, headers, ct).ConfigureAwait(false)
                       : null;
        }

        public static async Task<HttpResponseMessage> PostAsync(Uri url, CancellationToken ct)
        {
            return await MakeRequestAsync(url, HttpMethod.Post, null, null, ct).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> PostAsync(
            Uri url,
            IEnumerable<KeyValuePair<string, string>> args,
            CancellationToken ct = default(CancellationToken))
        {
            return await MakeRequestAsync(url, HttpMethod.Post, args, null, ct).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> PostAsync(
            Uri url,
            IEnumerable<KeyValuePair<string, string>> args,
            IEnumerable<KeyValuePair<string, object>> headers,
            CancellationToken ct = default(CancellationToken))
        {
            return await MakeRequestAsync(url, HttpMethod.Post, args, headers, ct).ConfigureAwait(false);
        }

        #endregion

        #region Head

        public static async Task<HttpResponseMessage> HeadAsync(string url, CancellationToken ct)
        {
            Uri link;
            return Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out link) &&
                   (!link.IsAbsoluteUri || link.Scheme.Contains("http"))
                       ? await MakeRequestAsync(link, HttpMethod.Head, null, null, ct).ConfigureAwait(false)
                       : null;
        }

        public static async Task<HttpResponseMessage> HeadAsync(Uri url, CancellationToken ct)
        {
            return await MakeRequestAsync(url, HttpMethod.Head, null, null, ct).ConfigureAwait(false);
        }

        #endregion

        private static async Task<HttpResponseMessage> MakeRequestAsync(
            Uri url,
            HttpMethod method,
            IEnumerable<KeyValuePair<string, string>> args,
            IEnumerable<KeyValuePair<string, object>> headers,
            CancellationToken ct)
        {
            if (url == null)
                return null;
            try
            {
                var request = new HttpRequestMessage(method ?? HttpMethod.Get, url)
                    {
                        Headers =
                            {
                                Accept =
                                    {
                                        new MediaTypeWithQualityHeaderValue("text/html"),
                                        new MediaTypeWithQualityHeaderValue("application/xhtml+xml"),
                                        new MediaTypeWithQualityHeaderValue("application/json"),
                                        new MediaTypeWithQualityHeaderValue("text/javascript"),
                                        new MediaTypeWithQualityHeaderValue("image/webp"),
                                        new MediaTypeWithQualityHeaderValue("*/*"),
                                    },
                                UserAgent =
                                    {
                                        new ProductInfoHeaderValue("Mozilla", "5.0"),
                                        new ProductInfoHeaderValue("AppleWebKit", "537.36"),
                                        new ProductInfoHeaderValue("Chrome", "51.0.2704.63"),
                                        new ProductInfoHeaderValue("Safari", "537.36"),
                                        new ProductInfoHeaderValue("OPR", "38.0.2220.25"),
                                        new ProductInfoHeaderValue("Lain", "1")
                                    }
                            }
                    };
                if (args != null)
                {
                    if (method == HttpMethod.Get
                        || method == HttpMethod.Head)
                    {
                        var parts = args.Select(p => p.Key + "=" + Uri.EscapeUriString(p.Value)).ToArray();
                        var argLine = "?" + string.Join("&", parts);
                        request.RequestUri = new Uri(request.RequestUri + argLine);
                    }
                    else
                    {
                        request.Content = new FormUrlEncodedContent(args);
                    }
                }
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        var prop = request.Headers
                            .GetType()
                            .GetProperty(
                                header.Key,
                                BindingFlags.Public |
                                BindingFlags.NonPublic |
                                BindingFlags.Instance);
                        if (prop != null)
                        {
                            prop.SetValue(request.Headers, header.Value);
                        }
                        else
                        {
                            if (request.Headers.Contains(header.Key))
                                request.Headers.Remove(header.Key);
                            request.Headers.Add(header.Key, header.Value.ToString());
                        }
                    }
                }

                var response = await httpClient.SendAsync(request, ct).ConfigureAwait(false);

                return ct.IsCancellationRequested
                           ? null
                           : response;
            }
            catch (OperationCanceledException)
            {
            }
            catch (IOException)
            {
            }
            catch (HttpRequestException)
            {
                await MessageBox.ShowAsync(
                    "Ошибка соединения.\nПроверьте подключение к интернету или связь с сайтом через браузер.\nВ случае проблем с последним попробуйте в настройках клиента сменить подключение на альтернативное.");
            }
            return null;
        }

        public static async Task<Stream> AsStream(this Task<HttpResponseMessage> responceTask)
        {
            var response = await responceTask.ConfigureAwait(false);
            if (response == null)
                return null;
            return await response.AsStream().ConfigureAwait(false);
        }

        public static async Task<Stream> AsStream(this HttpResponseMessage response)
        {
            try
            {
                return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }
            catch (NullReferenceException)
            {

            }
            return null;
        }

        public static async Task<IHtmlDocument> AsHtml(this Task<HttpResponseMessage> responceTask)
        {
            var responce = await responceTask.ConfigureAwait(false);
            if (responce == null)
                return null;
            return await responce.AsHtml().ConfigureAwait(false);
        }

        public static async Task<IHtmlDocument> AsHtml(this HttpResponseMessage responce)
        {
            try
            {
                var stream = await responce.AsStream().ConfigureAwait(false);
                if (stream == null)
                    return null;
                return await HtmlParser.ParseAsync(stream).ConfigureAwait(false);
            }
            catch (NullReferenceException)
            {

            }
            catch (DomException)
            {
            }
            return null;
        }

        public static async Task<JToken> AsJson(this Task<HttpResponseMessage> responceTask)
        {
            var responce = await responceTask.ConfigureAwait(false);
            if (responce == null)
                return null;
            return await responce.AsJson();
        }

        public static async Task<JToken> AsJson(this HttpResponseMessage responce)
        {
            try
            {
                if (responce == null)
                    return null;
                var text = await responce.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JToken.Parse(text);
            }
            catch (NullReferenceException)
            {

            }
            catch (JsonException)
            {
            }
            return null;
        }

        public static string GetCookies(string key, Uri domain = null)
        {
            if (domain == null
                && DomainUrl == null)
                return null;
            var c = Handler.CookieContainer.GetCookies(domain ?? DomainUrl);
            return c[key]?.Value;
        }

        public static void SetCookie(string key, string value, Uri domain = null)
        {
            if (domain == null
                && DomainUrl == null)
                return;
            Handler.CookieContainer.Add(domain ?? DomainUrl, new Cookie(key, value));
        }

        public static void SaveCookies(params string[] cookKeys)
        {
            var c = Handler.CookieContainer.GetCookies(DomainUrl);
            StorageHelper.SetSetting("SavedCookies", string.Join(";", cookKeys));
            foreach (var key in cookKeys)
            {
                var cookie = c[key]?.Value;
                if (cookie != null)
                    StorageHelper.SetSetting(key, cookie);
            }
        }

        public static void DeleteCookies(params string[] cookDelKeys)
        {
            var c = Handler.CookieContainer.GetCookies(DomainUrl);
            var cookOldKeys = StorageHelper.GetSetting<string>("SavedCookies")?.Split(';')?.ToList();
            if (cookOldKeys != null
                && cookOldKeys.Count != 0)
            {
                foreach (var key in cookDelKeys)
                {
                    if (c[key] != null)
                        c[key].Expired = true;
                    cookOldKeys.Remove(key);
                    StorageHelper.SetSetting(key, "");
                }
                StorageHelper.SetSetting("SavedCookies", string.Join(";", cookOldKeys));
            }
        }

        public static void LoadCookies()
        {
            var cookKeys = StorageHelper.GetSetting<string>("SavedCookies")?.Split(';');
            if (cookKeys == null)
                return;
            foreach (var key in cookKeys)
            {
                var cookie = StorageHelper.GetSetting<string>(key);
                if (cookie != null)
                    Handler.CookieContainer.Add(DomainUrl, new Cookie(key, cookie));
            }
        }

        // TODO
        //public static IEnumerable<Cookie> GetAllCookies()  
        //{
        //    var c = Handler.CookieContainer;
        //    var k = (Hashtable)c.GetType().GetField("m_domainTable", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(c);
        //    foreach (DictionaryEntry element in k)
        //    {
        //        SortedList l = (SortedList)element.Value.GetType().GetField("m_list", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(element.Value);
        //        foreach (var e in l)
        //        {
        //            var cl = (CookieCollection)((DictionaryEntry)e).Value;
        //            foreach (Cookie fc in cl)
        //            {
        //                yield return fc;
        //            }
        //        }
        //    }
        //}
    }
}