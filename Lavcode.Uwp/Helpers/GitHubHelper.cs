using Newtonsoft.Json;
using Octokit;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lavcode.Uwp.Helpers
{
    public static class GitHubHelper
    {
        public static GitHubClient GetBaseClient()
        {
            return new GitHubClient(new ProductHeaderValue(Global.Repos));
        }

        public static GitHubClient GetAuthClient(string token)
        {
            var credentials = new Credentials(token, AuthenticationType.Oauth);
            return new GitHubClient(new ProductHeaderValue(Global.Repos)) { Credentials = credentials };
        }

        public static async Task<string> GetLoginUrl()
        {
            HttpClient httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(20)
            };
            httpClient.DefaultRequestHeaders.Add("version", Global.Version);
            httpClient.DefaultRequestHeaders.Add("app", "lavcode");
            httpClient.DefaultRequestHeaders.Add("platform", "uwp");

            var res = await httpClient.PostAsync($"{Global.ToolsApiUrl}/github/getOAuthLoginUrl", new StringContent(JsonConvert.SerializeObject(new
            {
                login = "lavcode",
                scopes = new string[0]
            }), Encoding.UTF8, "application/json"));
            if (!res.IsSuccessStatusCode) return null;
            return await res.Content.ReadAsStringAsync();
        }
    }
}
