using Octokit;

namespace Lavcode.Uwp.Helpers
{
    public static class GitHubHelper
    {
        public static GitHubClient GetBaseClient(string name)
        {
            return new GitHubClient(new ProductHeaderValue(name));
        }

        public static GitHubClient GetAuthClient(string name, string token)
        {
            var credentials = new Credentials(token, AuthenticationType.Oauth);
            return new GitHubClient(new ProductHeaderValue(name)) { Credentials = credentials };
        }
    }
}
