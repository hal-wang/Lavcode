using HTools;
using Lavcode.IService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lavcode.Service.Api
{
    public class ConService : IConService
    {
        private string _token = null;
        private string _version = null;
        private string _baseUrl = null;
        public Func<bool> UseProxy { private set; get; } = null;

        public Task<bool> Connect(object args)
        {
            var obj = DynamicHelper.ToExpandoObject(args);
            _version = obj.Version;
            _baseUrl = obj.BaseUrl;

            if (obj.Token != null)
            {
                _token = obj.Token;
            }
            if (string.IsNullOrEmpty(_token))
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public void Dispose()
        {

        }

        public void SetProxy(Func<bool> useProxy)
        {
            UseProxy = useProxy;
        }


        private TimeSpan? _timeout;
        public IConService SetTimeout(TimeSpan timeout)
        {
            _timeout = timeout;
            return this;
        }
        public IConService SetTimeout(int timeout)
        {
            _timeout = TimeSpan.FromSeconds(timeout);
            return this;
        }

        private IList<KeyValuePair<string, string>> CommonHeaders => new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("platform", "uwp"),
            new KeyValuePair<string, string>("version", _version??""),
            new KeyValuePair<string, string>("Authorization", _token??""),
        };

        private HttpClient GetHttpClient()
        {
            HttpClient client = new(new HttpClientHandler()
            {
                UseProxy = UseProxy?.Invoke() ?? false,
            })
            {
                Timeout = _timeout ?? TimeSpan.FromSeconds(20),
                BaseAddress = new Uri(_baseUrl.TrimEnd('/') + "/")
            };
            foreach (var kvp in CommonHeaders)
            {
                if (!string.IsNullOrEmpty(kvp.Value))
                {
                    client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
                }
            }
            return client;
        }

        public async Task<string> Login(string password)
        {
            var obj = await GetAsync<JObject>("auth", query: new
            {
                password = Convert.ToBase64String(Encoding.UTF8.GetBytes(password))
            });
            return obj["token"].Value<string>();
        }

        #region BaseRequest
        private async Task<T> SendAsync<T>(string url, string method, object content = null, object param = null, object query = null)
        {
            try
            {
                using var hc = GetHttpClient();
                using var res = await hc.SendAsync(url, method, content, param, query);
                var contentStr = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(contentStr);
                }
                else
                {
                    if (string.IsNullOrEmpty(contentStr))
                    {
                        throw new HttpRequestException($"{res.StatusCode}  {res.ReasonPhrase}");
                    }
                    else
                    {
                        try
                        {
                            var jObj = JsonConvert.DeserializeObject<JObject>(contentStr);
                            if (jObj.ContainsKey("message"))
                            {
                                throw new HttpRequestException(jObj["message"].Value<string>());
                            }
                            else
                            {
                                throw new HttpRequestException(jObj.Value<string>());
                            }
                        }
                        catch (JsonReaderException)
                        {
                            throw new HttpRequestException(contentStr);
                        }
                    }
                }
            }
            catch (TimeoutException)
            {
                throw new TimeoutException("请求超时，请检查网络设置");
            }
            catch (TaskCanceledException)
            {
                throw new TaskCanceledException("请求失败，请检查网络设置");
            }
            catch (HttpRequestException)
            {
                throw new HttpRequestException("请求失败，请检查网络设置");
            }
            catch (UriFormatException)
            {
                throw new Exception("地址格式错误");
            }
            catch (Exception ex)
            {
                throw new Exception("未知错误：" + ex.Message);
            }
        }

        public Task<T> GetAsync<T>(string url = "", object content = null, object param = null, object query = null)
        {
            return SendAsync<T>(url, "GET", content, param, query);
        }

        public async Task<object> GetAsync(string url = "", object content = null, object param = null, object query = null)
        {
            return await GetAsync<object>(url, content, param, query);
        }

        public Task<T> PostAsync<T>(string url = "", object content = null, object param = null, object query = null)
        {
            return SendAsync<T>(url, "POST", content, param, query);
        }

        public async Task<object> PostAsync(string url = "", object content = null, object param = null, object query = null)
        {
            return await PostAsync<object>(url, content, param, query);
        }

        public Task<T> PatchAsync<T>(string url = "", object content = null, object param = null, object query = null)
        {
            return SendAsync<T>(url, "PATCH", content, param, query);
        }

        public async Task<object> PatchAsync(string url = "", object content = null, object param = null, object query = null)
        {
            return await PatchAsync<object>(url, content, param, query);
        }

        public Task<T> DeleteAsync<T>(string url = "", object content = null, object param = null, object query = null)
        {
            return SendAsync<T>(url, "DELETE", content, param, query);
        }

        public async Task<object> DeleteAsync(string url = "", object content = null, object param = null, object query = null)
        {
            return await DeleteAsync<object>(url, content, param, query);
        }

        public Task<T> PutAsync<T>(string url = "", object content = null, object param = null, object query = null)
        {
            return SendAsync<T>(url, "PUT", content, param, query);
        }

        public async Task<object> PutAsync(string url = "", object content = null, object param = null, object query = null)
        {
            return await PutAsync<object>(url, content, param, query);
        }
        #endregion
    }
}
