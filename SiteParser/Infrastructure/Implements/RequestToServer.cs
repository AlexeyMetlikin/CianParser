using SiteParser.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SiteParser.Infrastructure.Implements
{
    public class RequestToServer : IRequestToServer
    {
        private IWebProxy _webProxy;

        public bool IsRequestByProxy { get; set; }  // Запрос через прокси сервер

        public RequestToServer()
        {
            IsRequestByProxy = false;
        }

        public void SetWebProxy(string address, int port)
        {
            _webProxy = new WebProxy(address, port);
        }

        public string GetPageContent(string url)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.AllowAutoRedirect = false;
                httpWebRequest.Method = "GET";

                if (IsRequestByProxy)
                {
                    httpWebRequest.Proxy = _webProxy;
                }

                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var stream = httpWebResponse.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream, Encoding.GetEncoding(httpWebResponse.CharacterSet)))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                throw new HttpRequestException("Ошибка при запросе на сервер: " + exp);
            }
        }
    }
}
