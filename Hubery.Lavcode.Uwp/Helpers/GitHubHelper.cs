using Octokit;

namespace Hubery.Lavcode.Uwp.Helpers
{
    public static class GitHubHelper
    {
        public static GitHubClient GetBaseClient()
        {
            return new GitHubClient(new ProductHeaderValue(Global.Repos));
        }

        public static GitHubClient GetAuthClient(string account, string pwd)
        {
            var credentials = new Credentials(account, pwd, AuthenticationType.Basic);
            return new GitHubClient(new ProductHeaderValue(Global.Repos)) { Credentials = credentials };
        }
    }
}
