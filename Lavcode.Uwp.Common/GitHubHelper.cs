using Octokit;

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
    }
}
