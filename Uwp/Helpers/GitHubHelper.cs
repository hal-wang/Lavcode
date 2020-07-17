using Octokit;

namespace Hubery.Lavcode.Uwp.Helpers
{
    public static class GitHubHelper
    {
        private static GitHubClient GetClient(Credentials credentials)
        {
            return new GitHubClient(new ProductHeaderValue(Global.Repos))
            {
                Credentials = credentials
            };
        }

        public static GitHubClient GetBaseClient() => GetClient(new Credentials(Global.GitHubToken));
        public static GitHubClient GetAuthClient(string account, string pwd) => GetClient(new Credentials(account, pwd, AuthenticationType.Basic));
    }
}
