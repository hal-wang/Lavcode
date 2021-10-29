using HTools;
using HTools.Uwp.Helpers;
using System;
using System.Net.Http;

namespace Lavcode.Uwp.Helpers
{
    public static class HttpClientExtend
    {
        public static bool IsResErr(this HttpResponseMessage httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode) return false;

            new Action(async () => MessageHelper.ShowDanger(await httpResponse.GetErrorMessage())).Invoke();
            return true;
        }

        public static HttpClient HttpClient
        {
            get
            {
                HttpClient httpClient = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(20)
                };
                httpClient.DefaultRequestHeaders.Add("version", Global.Version);
                httpClient.DefaultRequestHeaders.Add("app", "lavcode");
                httpClient.DefaultRequestHeaders.Add("platform", "uwp");
                return httpClient;
            }
        }
    }
}
