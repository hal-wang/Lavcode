using Hubery.Lavcode.Uwp;
using Hubery.Lavcode.Uwp.Helpers;
using Hubery.Lavcode.Uwp.Helpers.Api;
using Hubery.Lavcode.Uwp.Helpers.Logger;
using Hubery.Lavcode.Uwp.Model.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hubery.Yt.Uwp.Helpers
{
    public static partial class ApiExtendHelper
    {
        public async static Task<bool> IsSuccess(this HttpResponseMessage httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode)
            {
                return true;
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                try
                {
                    content = (int)(httpResponse.StatusCode) + "  " + httpResponse.StatusCode.ToString();
                }
                catch (Exception ex)
                {
                    LogHelper.Instance.Log(ex);
                    content = "未知错误";
                }
            }
            Debug.WriteLine("错误" + content);
            MessageHelper.ShowDanger(content);
            return false;
        }

        public async static Task<T> GetContent<T>(this HttpResponseMessage httpResponse)
        {
            var str = await httpResponse.Content.ReadAsStringAsync();
            if (typeof(T) == typeof(string))
            {
                if (str.Length > 0 && str[0] != '"')
                {
                    str = '"' + str + '"';
                }
            }

            return JsonConvert.DeserializeObject<T>(str);
        }

        public static async Task<string> GetAccessToken(string account, string password)
        {
            using HttpClient client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(20)
            };
            var res = await client.PostAsync($"{Global.GiteeUrl}/oauth/token", new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("grant_type","password"),
                new KeyValuePair<string, string>("username",account),
                new KeyValuePair<string, string>("password",password),
                new KeyValuePair<string, string>("client_id",Global.GiteeClientId),
                new KeyValuePair<string, string>("client_secret",Global.GiteeClientSecret),
                new KeyValuePair<string, string>("scope","notes"),
            }));
            if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                MessageHelper.ShowDanger("账号或密码错误");
                return null;
            }
            if (!await res.IsSuccess())
            {
                return null;
            }
            var resData = await res.GetContent<JObject>();
            return resData.Value<string>("access_token");
        }

        public static async Task<Issue> GetIssue(string issueId)
        {
            var res = await ApiHelper.Repos.Get($"{Global.GiteeAccount}/{Global.Repos}/issues/{issueId}");
            if (!await res.IsSuccess())
            {
                return null;
            }

            return await res.GetContent<Issue>();
        }

        public static async Task<Repository> GetRepos()
        {
            var res = await ApiHelper.Repos.Get($"{Global.GiteeAccount}/{Global.Repos}");
            if (!await res.IsSuccess())
            {
                return null;
            }

            return await res.GetContent<Repository>();
        }
    }
}
