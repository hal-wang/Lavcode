using HTools;
using Lavcode.Uwp.Model;
using Newtonsoft.Json.Linq;
using Octokit;
using System;
using System.Net.Http;
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
