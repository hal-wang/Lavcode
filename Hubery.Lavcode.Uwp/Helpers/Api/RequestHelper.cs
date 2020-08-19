using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hubery.Lavcode.Uwp.Helpers.Api
{
    public class RequestHelper
    {
        private readonly string _url;
        private readonly string _controller;

        public RequestHelper(string url, string controller = null)
        {
            _url = url;
            _controller = controller;
        }

        private string GetUrl(string func)
        {
            return _url + (_controller == null ? "" : ("/" + _controller)) + "/" + func;
        }

        private HttpClient GetHttpClient(KeyValuePair<string, string>[] header)
        {
            HttpClient client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(20)
            };
            if (header != null)
            {
                foreach (var kvp in header)
                {
                    client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
                }
            }
            return client;
        }

        private async Task<HttpResponseMessage> Request(string funcName, string method, object param, KeyValuePair<string, string>[] header)
        {
            using HttpClient client = GetHttpClient(header);
            string url = GetUrl(funcName);

            Debug.WriteLine($"{url}, {method}: {JsonConvert.SerializeObject(param)}");
            var response = method switch
            {
                "POST" => await client.PostAsync(url, new StringContent(param == null ? "{}" : JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json")),
                "GET" => await client.GetAsync(url),
                _ => throw new NotSupportedException()
            };
            Debug.WriteLine($"{url}, result:{response.StatusCode}, {await response.Content.ReadAsStringAsync()}");

            return response;
        }

        public async Task<HttpResponseMessage> Post(string funcName, object param = null, params KeyValuePair<string, string>[] header)
        {
            return await this.Request(funcName, "POST", param, header);
        }

        public async Task<HttpResponseMessage> Get(string funcName, params KeyValuePair<string, string>[] header)
        {
            return await this.Request(funcName, "GET", null, header);
        }
    }
}
