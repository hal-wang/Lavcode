using Lavcode.Model;

namespace Lavcode.Uwp.Helpers
{
    public class CommandLauncher
    {
        private readonly string _arguments;
        public CommandLauncher(string arguments)
        {
            _arguments = arguments;
        }

        public string[] Arguments => _arguments?.Split(' ');

        public Provider? Provider
        {
            get
            {
                if (Arguments == null || Arguments.Length < 1)
                {
                    return null;
                }

                var pStr = Arguments[1];
                if (
                    pStr.ToLower() == Model.Provider.Gitee.ToString().ToLower() ||
                    pStr.ToLower() == "码云")
                {
                    return Model.Provider.Gitee;
                }
                else if (
                    pStr.ToLower() == Model.Provider.GitHub.ToString().ToLower() ||
                    pStr.ToLower() == "git" ||
                    pStr.ToLower() == "gayhub") // LOL
                {
                    return Model.Provider.GitHub;
                }
                else if (
                    pStr.ToLower() == Model.Provider.Sqlite.ToString().ToLower() ||
                    pStr.ToLower() == "local" ||
                    pStr.ToLower() == "本地")
                {
                    return Model.Provider.Sqlite;
                }
                else if (
                    pStr.ToLower() == Model.Provider.Api.ToString().ToLower() ||
                    pStr.ToLower() == "net" ||
                    pStr.ToLower() == "network")
                {
                    return Model.Provider.Api;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
